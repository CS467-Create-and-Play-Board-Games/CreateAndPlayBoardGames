using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Tilemaps;


public class CameraSystem : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    private static int MAXINT = 999999999;
    private static int MININT = -999999999;
    private static float MOVESIZE = 0.0625f;
    private bool edgeScrollEnabled = false;
    private static int EDGESCROLLSIZE = 15;
    [SerializeField] private float targetOrthographicSize;
    [SerializeField] private float ORTHOMAX;
    [SerializeField] private float ORTHOMIN;
    private Vector3 home = new Vector3();
    void Start()
    {
        home = FindCenter();
        transform.position = home;
    }

    private Vector3 FindCenter() 
    {
        List<Vector3> tilePosList = GetTileList();
        Vector3 minX = FindMin(tilePosList, 0);
        Vector3 minY = FindMin(tilePosList, 1);
        Vector3 maxX = FindMax(tilePosList, 0);
        Vector3 maxY = FindMax(tilePosList, 1);
        // Debug.Log("minX: " + minX + ", maxX: " + maxX + ", minY: " + minY + ", maxY: " + maxY);
        float width = maxX[0] - minX[0];
        float height = maxY[1] - minY[1];
        // Debug.Log("width: " + width + ", height: " + height);
        Vector3 center = new Vector3((minX[0] + width/2) + 0.125f, (minY[1] + height/2) + 0.125f, 0);
        // Debug.Log("center: " + center);
        
        return center;
    }

    List<Vector3> GetTileList()
    {
        List<Vector3> result = new List<Vector3>();
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) != null){
                result.Add(tilemap.CellToWorld(pos));
            }
        }
        if (result.Count == 0) {
            result.Add(new Vector3(0, 0, 0));
        }
        return result;
    }

    private Vector3 FindMin(List<Vector3> tileList, int axis)
    {
        Vector3 result = new Vector3(MAXINT, MAXINT, 0);

        foreach(Vector3 entry in tileList)
        {
            if (entry[axis] <= result[axis]){
                result = entry;
            }
        }
        return result;
    }

    private Vector3 FindMax(List<Vector3> tileList, int axis)
    {
        Vector3 result = new Vector3(MININT, MININT, 0);

        foreach(Vector3 entry in tileList)
        {
            if (entry[axis] >= result[axis]){
                result = entry;
            }
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // transform.position = Input.mousePosition;
        // transform.position = tilemap.transform.position;
        // https://www.youtube.com/watch?v=pJQndtJ2rk0
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.y = + MOVESIZE;
        if (Input.GetKey(KeyCode.A)) moveDir.x = - MOVESIZE;
        if (Input.GetKey(KeyCode.S)) moveDir.y = - MOVESIZE;
        if (Input.GetKey(KeyCode.D)) moveDir.x = + MOVESIZE;

        if (edgeScrollEnabled) {
            if (Input.mousePosition.y > Screen.height - EDGESCROLLSIZE) moveDir.y = + MOVESIZE;
            if (Input.mousePosition.x < EDGESCROLLSIZE) moveDir.x = - MOVESIZE;
            if (Input.mousePosition.y < EDGESCROLLSIZE) moveDir.y = - MOVESIZE;
            if (Input.mousePosition.x > Screen.width - EDGESCROLLSIZE) moveDir.x = + MOVESIZE;
        }
        
        float moveSpeed = 25f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.C)) transform.position = FindCenter();
        if (Input.GetKey(KeyCode.H)) transform.position = home;

        HandleCameraZoom();
    }

    private void HandleCameraZoom() {
        if (Input.mouseScrollDelta.y > 0) {
            targetOrthographicSize -= 0.1f;
        }
        if (Input.mouseScrollDelta.y < 0) {
            targetOrthographicSize += 0.1f;
        }

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, ORTHOMIN, ORTHOMAX);

        float zoomSpeed = 3f;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = 
            Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

    }
}
