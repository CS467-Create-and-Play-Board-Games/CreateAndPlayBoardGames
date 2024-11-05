using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewCustomTile", menuName = "BoardEditor/Custom Tile")]
public class CustomTile : ScriptableObject
{
    public TileBase tile;
    public string id;
}
