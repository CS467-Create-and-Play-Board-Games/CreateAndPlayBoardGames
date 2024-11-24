using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileCount : MonoBehaviour
{
    public TMP_Text TileCount;
    // Tilemap tilemap = gameObject.GetComponent<Tilemap>();
    // Start is called before the first frame update
    void Start()
    {
        // string count = GetComponent<Tilemap>().GetTilesRangeCount(new Vector3Int(0,0,0), new Vector3Int(50,50,0)).ToString();
        // string count = gameObject.GetComponent<Tilemap>().GetUsedTilesCount().ToString();
        int count = 0;
        BoundsInt bounds = gameObject.GetComponent<Tilemap>().cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (gameObject.GetComponent<Tilemap>().GetTile<Tile>(pos) != null)
            {
                count += 1;
            } 
            
        }
        TileCount.SetText(count.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
