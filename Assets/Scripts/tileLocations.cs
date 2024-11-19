using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

// https://discussions.unity.com/t/count-the-amount-of-a-certain-tile-in-a-tilemap/228363
// https://discussions.unity.com/t/tilemap-tile-positions-assistance/672652/3


public class tileLocations : MonoBehaviour
{

    public TMP_Text StartLocation, FinishLocation;
    public Sprite StartSprite, FinishSprite;

    public Dictionary<Vector3Int, Tile> _gameTilesDict;
    public List<Vector3Int> _gameTileLocation;
    public List<Vector3> _worldGameTileLocation;
    public List<string> _tileTypeOrder;

    Dictionary<Vector3Int, Tile> GetGameTilesDict()
    {
        Dictionary<Vector3Int, Tile> result = new Dictionary<Vector3Int, Tile>();
        BoundsInt bounds = gameObject.GetComponent<Tilemap>().cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = gameObject.GetComponent<Tilemap>().GetTile<Tile>(pos);
            if (tile != null){
                result.Add(pos, tile);
            }
        }
        return result;
    }

    List <Vector3Int> GetPositionOrder(Dictionary<Vector3Int, Tile> gameTiles)
    {
        List <Vector3Int> result = new List<Vector3Int>();
        // Get the location of the Start Tile
        foreach(var (key, value) in gameTiles)
        {
            if (value.name == "Start Tile"){
                result.Add(key);
                _tileTypeOrder.Add(value.name);
            }
        }
        
        while (result.Count < gameTiles.Count){
            foreach(var (key, value) in gameTiles)
            {
                if (!result.Contains(key) & Vector3.Distance(key, result.Last()) < 1.1){
                    result.Add(key);
                    _tileTypeOrder.Add(value.name);
                }
            }
        }

        return result;
    }

    List <Vector3> GetWorldPositionOrder(List <Vector3Int> localOrderedPositions)
    {
        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        List <Vector3> result = new List<Vector3>();
        foreach (Vector3Int pos in localOrderedPositions)
        {
            Vector3 place = tilemap.CellToWorld(pos);
            result.Add(place);
        } 
        return result;
    }

    List<Tile> GetGameTiles()
    {
        List<Tile> result = new List<Tile>();
        BoundsInt bounds = gameObject.GetComponent<Tilemap>().cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = gameObject.GetComponent<Tilemap>().GetTile<Tile>(pos);
            if (tile != null){
                result.Add(tile);
            }
        }
        return result;
    }

    void SortGameTiles(List<Tile> tiles)
    {
        List<Tile> result = new List<Tile>();
        foreach(Tile tile in tiles)
        {
            Debug.Log(tile.transform.GetPosition().ToString());
        }
        // return result;
    }

    
    string GetTileLocationSprite(Sprite targetSprite)
    {
    BoundsInt bounds = gameObject.GetComponent<Tilemap>().cellBounds;
    foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = gameObject.GetComponent<Tilemap>().GetTile<Tile>(pos);
            if (tile != null){
                if (tile.sprite == targetSprite){
                    // Debug.Log(targetSprite.name);
                    return pos.ToString();
                }
            } 
        }
        return "None";
    }

    void Awake()
    {
        _gameTilesDict = GetGameTilesDict();
        _gameTileLocation = GetPositionOrder(_gameTilesDict);
        _worldGameTileLocation = GetWorldPositionOrder(_gameTileLocation);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartLocation.SetText(GetTileLocationSprite(StartSprite));
        FinishLocation.SetText(GetTileLocationSprite(FinishSprite));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
