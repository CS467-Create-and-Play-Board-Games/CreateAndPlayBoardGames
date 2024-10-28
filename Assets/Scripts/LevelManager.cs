using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Tilemaps;
using System.IO;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public Tilemap tilemap;
    public TMP_InputField saveAsInput, loadAsInput;
    public GameObject saveButton, loadButton;
    public TextMeshProUGUI saveSuccess, loadSuccess;
    private float saveSuccessTime = 0;
    private float loadSuccessTime = 0;
    

    private void Update()
    {
        if (saveSuccess.gameObject.activeSelf == true){
            if (Time.time - saveSuccessTime > 5.0) saveSuccess.gameObject.SetActive(false);
        }
        if (loadSuccess.gameObject.activeSelf == true){
            if (Time.time - loadSuccessTime > 5.0) loadSuccess.gameObject.SetActive(false);
        }

        if (loadAsInput.gameObject.activeSelf){
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
        if (saveAsInput.gameObject.activeSelf)
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
    }

    Dictionary<Vector3Int, Tile> GetGameTilesDict()
    {
        Dictionary<Vector3Int, Tile> result = new Dictionary<Vector3Int, Tile>();
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null){
                result.Add(pos, tile);
            }
        }
        return result;
    }

    public void ShowLoadDetails()
    {
        loadAsInput.gameObject.SetActive(true);
    }
    public void ShowSaveDetails()
    {
        saveAsInput.gameObject.SetActive(true);
    }

    // TODO: validate user input
    private bool PassLoadValidation()
    {
        return true;
    }
    private bool PassSaveValidation()
    {
        return true;
    }
    public void SaveLevel()
    {
       
        LevelData levelData = new LevelData();
        Dictionary<Vector3Int, Tile> levelDict = GetGameTilesDict();
        foreach(KeyValuePair<Vector3Int, Tile> entry in levelDict)
        {
            levelData.tilePositions.Add(entry.Key);
            levelData.tiles.Add(entry.Value);
        }

        string json = JsonUtility.ToJson(levelData, true);
        string fileName = saveAsInput.text;
        File.WriteAllText(Application.dataPath + "/Boards/" + fileName + ".json" , json);
        saveAsInput.gameObject.SetActive(false);
        saveButton.SetActive(false);
        saveSuccess.gameObject.SetActive(true);
        saveSuccessTime = Time.time;
    }

    public void LoadLevel()
    {
        string fileName = loadAsInput.text;
        string json = File.ReadAllText(Application.dataPath + "/Boards/"+ fileName + ".json");
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        tilemap.ClearAllTiles();

        for (int i = 0; i < levelData.tilePositions.Count; i++)
        {
            tilemap.SetTile(levelData.tilePositions[i], levelData.tiles[i]);
        }
        loadAsInput.gameObject.SetActive(false);
        loadButton.SetActive(false);
        loadSuccess.gameObject.SetActive(true);
        loadSuccessTime = Time.time;

    }

}

public class LevelData
{
    public List<TileBase> tiles = new List<TileBase>();
    public List<Vector3Int> tilePositions = new List<Vector3Int>();
}