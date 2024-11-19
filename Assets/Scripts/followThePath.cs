using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowThePath : MonoBehaviour {

    public List<UnityEngine.Vector3> waypoints;
    private UnityEngine.Vector3[] playerOffsets = new UnityEngine.Vector3[8];
    private UnityEngine.Vector3 locationToMoveTo;
    private GameObject _gameController;

    [SerializeField]
    private float moveSpeed = 1f;
    public int playerNumber = 0;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;
    public bool isMoving;
    public bool nextTurnSkipped = false;

	// Use this for initialization
	private void Start () {

        playerOffsets[0] = new UnityEngine.Vector3(0.05f, 0.20f, 0.0f);
        playerOffsets[1] = new UnityEngine.Vector3(0.20f, 0.20f, 0.0f);
        playerOffsets[2] = new UnityEngine.Vector3(0.05f, 0.05f, 0.0f);
        playerOffsets[3] = new UnityEngine.Vector3(0.20f, 0.05f, 0.0f);
        playerOffsets[4] = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(0.05f, 0.20f, 0.0f), new UnityEngine.Vector3(0.7f, 0.7f, 0));
        playerOffsets[5] = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(0.20f, 0.20f, 0.0f), new UnityEngine.Vector3(0.7f, 0.7f, 0));
        playerOffsets[6] = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(0.05f, 0.05f, 0.0f), new UnityEngine.Vector3(0.7f, 0.7f, 0));
        playerOffsets[7] = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(0.20f, 0.05f, 0.0f), new UnityEngine.Vector3(0.7f, 0.7f, 0));
        _gameController = GameObject.Find("GameControl");
        waypoints = GameObject.Find("Tilemap").GetComponent<tileLocations>()._worldGameTileLocation;

        locationToMoveTo = waypoints[waypointIndex] + playerOffsets[playerNumber-1];

        transform.position = locationToMoveTo;
        Debug.Log(waypoints[waypointIndex].ToString());
	}
	
	// Update is called once per frame
	private void Update () 
    {

	}

    public IEnumerator Move(int spaces)
    {
        Debug.Log("Moving Player " + playerNumber);
        Debug.Log("Roll Value: " + spaces);

        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        while (spaces > 0)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Count - 1)
            {
                spaces = 0;
            }
            locationToMoveTo = waypoints[waypointIndex] + playerOffsets[playerNumber-1];
            Debug.Log("current: " + transform.position);
            Debug.Log("moving to: " + locationToMoveTo);
            while (MoveToNextNode(locationToMoveTo)) { yield return null; }
            yield return new WaitForSeconds(0.1f);
            spaces--;
        }
        isMoving = false;
        _gameController.GetComponent<GameControl>().ResolveTile(GameObject.Find("Tilemap").GetComponent<tileLocations>()._tileTypeOrder[waypointIndex]);
        _gameController.GetComponent<GameControl>().NextTurn();
        _gameController.GetComponent<GameControl>().CheckForGameOver(playerNumber);
        _gameController.GetComponent<GameControl>().UpdatePlayerTurnText();

    }

    bool MoveToNextNode(UnityEngine.Vector3 goal)
    {
        return goal != (transform.position = UnityEngine.Vector3.MoveTowards(transform.position,locationToMoveTo,moveSpeed * Time.deltaTime));       
    }

    public IEnumerator MoveForSwapPlayers(UnityEngine.Vector3 goal)
    {
        locationToMoveTo = goal + playerOffsets[playerNumber-1];
        while (MoveToNextNode(locationToMoveTo)) { yield return null; }
            yield return new WaitForSeconds(0.1f);
    }

}
