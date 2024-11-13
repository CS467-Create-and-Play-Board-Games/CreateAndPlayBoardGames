using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Tilemaps;
using System.IO;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<CustomTile> tiles = new List<CustomTile>();
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        // 
        if (StateNameController.filePathForGame != null)
        {
            LoadFromStateName();
        }    
        //
    }

    public Tilemap tilemap;
    public TMP_InputField saveAsInput, loadAsInput;
    public GameObject saveButton, loadButton, cancelButton;
    public TextMeshProUGUI saveSuccess, loadSuccess, selectedTileName, validationMessage;
    private float saveSuccessTime = 0;
    private float loadSuccessTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        StateNameController.saveGameClicked = false;
    }

    private void Update()
    {
        if (saveSuccess != null && saveSuccess.gameObject.activeSelf == true){
            if (Time.time - saveSuccessTime > 5.0) saveSuccess.gameObject.SetActive(false);
        }
        if (loadSuccess != null && loadSuccess.gameObject.activeSelf == true){
            if (Time.time - loadSuccessTime > 5.0) loadSuccess.gameObject.SetActive(false);
        }

        if (loadAsInput != null && loadAsInput.gameObject.activeSelf){
            if (PassLoadValidation())
            {
                loadButton.SetActive(true);
            }
            else
            {
                loadButton.SetActive(false);
            }
        }

        // TODO: Input validation for file name save
        if (saveAsInput != null && saveAsInput.gameObject.activeSelf)
        {
            if (PassSaveValidation())
            {
                saveButton.SetActive(true);
            }
            else
            {
                saveButton.SetActive(false);
            }
        }
        if (selectedTileName != null) {
            selectedTileName.text = tiles[GameObject.Find("Grid").GetComponentInChildren<LevelEditor>()._selectedTileIndex].name; 
        }
        
        
    }

    Dictionary<Vector3Int, string> GetGameTilesDict()
    {
        Dictionary<Vector3Int, string> result = new Dictionary<Vector3Int, string>();
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase temp = tilemap.GetTile<Tile>(pos);
            CustomTile tempTile = tiles.Find(t => t.tile == temp);
            if (tempTile != null){
                result.Add(pos, tempTile.id);
            }
        }
        return result;
    }

    public void ShowLoadDetails()
    {
        loadAsInput.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
    }
    public void ShowSaveDetails()
    {
        StateNameController.saveGameClicked = true;
        cancelButton.gameObject.SetActive(true);
        if (PassSaveValidation()) {
            validationMessage.gameObject.SetActive(false);
            saveAsInput.gameObject.SetActive(true);
        } else {
            if (validationMessage != null) {
                validationMessage.gameObject.SetActive(true);
            }
        }
    }
    public void CancelSaveOrLoad()
    {
        if (saveAsInput != null){
            StateNameController.saveGameClicked = false;
            saveAsInput.gameObject.SetActive(false);
            loadAsInput.gameObject.SetActive(false);
            saveButton.gameObject.SetActive(false);
            loadButton.gameObject.SetActive(false);
        }
        cancelButton.gameObject.SetActive(false);
    }

    // TODO: validate user input
    private bool PassLoadValidation()
    {
        return true;
    }
    private bool PassSaveValidation()
    {
        // Start and Finish tile check - 1 and only 1
        LevelData levelData = GetLevelData();
        int startTileCount = 0;
        if (levelData.tiles.Where(x => x.ToLower().Contains("start")) != null){
            startTileCount = levelData.tiles.Where(x => x.ToLower().Contains("start")).Count();
        }
        int finishTileCount = 0;
        if (levelData.tiles.Where(x => x.ToLower().Contains("finish")) != null){
            finishTileCount = levelData.tiles.Where(x => x.ToLower().Contains("finish")).Count();
        }

        if (startTileCount != 1 || finishTileCount != 1) {
            if (validationMessage != null) {
                validationMessage.text = "Invalid - 1 Start, 1 Finish";
            }
            return false;
        }

        // Number of neighbors check
        // Start and Finish tiles should have only 1.
        Vector3Int startPosition = levelData.tilePositions[levelData.tiles.FindIndex(x => x.ToLower().Contains("start"))];
        Vector3Int finishPosition = levelData.tilePositions[levelData.tiles.FindIndex(x => x.ToLower().Contains("finish"))];
        if (CountNeighbors(startPosition, levelData.tilePositions) != 1 || CountNeighbors(finishPosition, levelData.tilePositions) != 1){
            NeighborErrorMessage();
            return false;
        }

        // All other tiles should have 2.  
        // Make a copy of the positions but remove the start and finish so the other tiles can be checked.
        List<Vector3Int> otherTilePositions = new List<Vector3Int>(levelData.tilePositions);
        foreach (Vector3Int position in otherTilePositions)
        {
            if (position != startPosition && position != finishPosition) {
                if (CountNeighbors(position, otherTilePositions) != 2) {
                    NeighborErrorMessage();
                    return false;
                }
            }
        }
        
        return true;
    }

    private int CountNeighbors(Vector3Int targetPos, List<Vector3Int> positions)
    {
        int neighbors = -1;
        foreach (Vector3Int pos in positions)
        {
            if (Vector3.Distance(targetPos, pos) < 1.1) {
                neighbors += 1;
            }
        }
        return neighbors;
    }

    private void NeighborErrorMessage()
    {
        if (validationMessage != null) {
            validationMessage.text = "Invalid - Number of Neighbors";
        }
    }
    public void SaveLevel()
    {
        LevelData levelData = GetLevelData();
        string json = JsonUtility.ToJson(levelData, true);
        string fileName = saveAsInput.text;
        File.WriteAllText(Application.dataPath + "/Boards/" + fileName + ".json" , json);
        saveAsInput.gameObject.SetActive(false);
        saveButton.SetActive(false);
        saveSuccess.gameObject.SetActive(true);
        saveSuccessTime = Time.time;
        StateNameController.saveGameClicked = false;
        if (cancelButton != null) {
            cancelButton.gameObject.SetActive(false);
        }
    }

    public void LoadLevel()
    {
        string fileName = loadAsInput.text;
        string filePathForGame = Application.dataPath + "/Boards/"+ fileName + ".json";
        StateNameController.filePathForGame = filePathForGame;
        LoadFromStateName();  
        if (cancelButton != null) {
            cancelButton.gameObject.SetActive(false);
        }
    }

    public void LoadFromStateName()
    {
        string json = File.ReadAllText(StateNameController.filePathForGame);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        tilemap.ClearAllTiles();
        PlaceTiles(levelData);
        if (loadAsInput != null) {
            loadAsInput.gameObject.SetActive(false);
            loadButton.SetActive(false);
            loadSuccess.gameObject.SetActive(true);
            loadSuccessTime = Time.time;
        }
        
        StateNameController.filePathForGame = null;
    }

    public void PlaceTiles(LevelData levelData)
    {
        for (int i = 0; i < levelData.tilePositions.Count; i++)
        {
            TileBase tempTile = tiles.Find(t => t.name == levelData.tiles[i]).tile;
            tilemap.SetTile(levelData.tilePositions[i], tempTile);
        }
    }

    private LevelData GetLevelData()
    {
        LevelData result = new LevelData();
        Dictionary<Vector3Int, string> levelDict = GetGameTilesDict();
        foreach(KeyValuePair<Vector3Int, string> entry in levelDict)
        {
            result.tilePositions.Add(entry.Key);
            result.tiles.Add(entry.Value);
        }

        return result;
    }


}

public class LevelData
{
    public List<string> tiles = new List<string>();
    public List<Vector3Int> tilePositions = new List<Vector3Int>();
}