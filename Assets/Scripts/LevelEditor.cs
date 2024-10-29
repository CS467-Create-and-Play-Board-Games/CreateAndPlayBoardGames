using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class levelEditor : MonoBehaviour
{
    [SerializeField] Tilemap currentTilemap;
    [SerializeField] TileBase currentTile;
    [SerializeField] Camera cam;

    private void Update()
    {
        Vector3Int pos = currentTilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
        if (EventSystem.current.IsPointerOverGameObject()) return;  //https://www.youtube.com/watch?v=rATAnkClkWU
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
}
