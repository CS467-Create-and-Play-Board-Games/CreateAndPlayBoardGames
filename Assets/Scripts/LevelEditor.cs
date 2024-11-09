using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    // https://www.youtube.com/watch?v=aDrgzsUdiSc
    [SerializeField] Tilemap currentTilemap;
    TileBase currentTile
    {
        get
        {
            return LevelManager.instance.tiles[_selectedTileIndex].tile;
        }
    }
    [SerializeField] Camera cam;

    public int _selectedTileIndex;

    private void Update()
    {
        Vector3Int pos = currentTilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
        if (EventSystem.current.IsPointerOverGameObject()) return;  //https://www.youtube.com/watch?v=rATAnkClkWU
        if (Input.GetKeyDown(KeyCode.KeypadPlus)){
            _selectedTileIndex = NextIndexInList(_selectedTileIndex, LevelManager.instance.tiles.Count);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)){
            _selectedTileIndex = PrevIndexInList(_selectedTileIndex, LevelManager.instance.tiles.Count);
        }
        if (Input.GetMouseButton(0)) PlaceTile(pos);
        if (Input.GetMouseButton(1)) ClearTile(pos);
    }

    void PlaceTile(Vector3Int pos)
    {
        currentTilemap.SetTile(pos, currentTile);
    }

    void ClearTile(Vector3Int pos)
    {
        currentTilemap.SetTile(pos, null);
    }

    int NextIndexInList(int currentIndex, int listLength)
    {
        int result = currentIndex + 1;
        if (result >= listLength){
            return 0;
        } else {
            return result;
        }
    }
    int PrevIndexInList(int currentIndex, int listLength)
    {
        int result = currentIndex - 1;
        if (result < 0){
            return listLength - 1;
        } else {
            return result;
        }

    }
}
