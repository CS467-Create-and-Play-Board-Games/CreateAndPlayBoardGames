using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCount : MonoBehaviour
{
    // [SerializeField] private Tilemap;
    // Start is called before the first frame update
    void Start()
    {
        string _text = GetComponent<Tilemap>().GetTilesRangeCount(new Vector3Int(0,0,0), new Vector3Int(50,50,0)).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
