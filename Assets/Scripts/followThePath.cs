using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowThePath : MonoBehaviour {

    // public GameObject _waypoints;
    // public Transform[] waypoints = _waypoints.GetComponentsInChildren<Transform>();
    // public Tilemap _tilemap;
    //public Transform[].position waypoints = GameObject.Find("Tilemap").GetComponent<tileLocations>()._gamePieceLocation;
    // public List<Vector3Int> waypoints;
    public List<UnityEngine.Vector3> waypoints;
    private UnityEngine.Vector3 player1Offset = new UnityEngine.Vector3(0.05f, 0.20f, 0.0f);
    private UnityEngine.Vector3 player2Offset = new UnityEngine.Vector3(0.20f, 0.20f, 0.0f);
    private UnityEngine.Vector3 player3Offset = new UnityEngine.Vector3(0.05f, 0.05f, 0.0f);
    private UnityEngine.Vector3 player4Offset = new UnityEngine.Vector3(0.20f, 0.05f, 0.0f);
    private UnityEngine.Vector3 locationToMoveTo;

    [SerializeField]
    private float moveSpeed = 1f;
    public float playerNumber = 0;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;
    public bool isMoving;

	// Use this for initialization
	private void Start () {
        
        waypoints = GameObject.Find("Tilemap").GetComponent<tileLocations>()._worldGameTileLocation;
        if (playerNumber == 1)
        {
            locationToMoveTo = waypoints[waypointIndex] + player1Offset;
        }
        else if (playerNumber == 2)
        {
            locationToMoveTo = waypoints[waypointIndex] + player2Offset;
        }
        else if (playerNumber == 3)
        {
            locationToMoveTo = waypoints[waypointIndex] + player3Offset;
        }
        else
        {
            locationToMoveTo = waypoints[waypointIndex] + player4Offset;
        }
        transform.position = locationToMoveTo;
        // UnityEngine.Vector3 t = waypoints[waypointIndex];
        // transform.position = t;
        // Debug.Log(waypoints[waypointIndex].ToString() + "translated to " + t.ToString());
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
            if (playerNumber == 1)
            {
                locationToMoveTo = waypoints[waypointIndex] + player1Offset;
            }
            else if (playerNumber == 2)
            {
                locationToMoveTo = waypoints[waypointIndex] + player2Offset;
            }
            else if (playerNumber == 3)
            {
                locationToMoveTo = waypoints[waypointIndex] + player3Offset;
            }
            else
            {
                locationToMoveTo = waypoints[waypointIndex] + player4Offset;
            }
            Debug.Log("current: " + transform.position);
            Debug.Log("moving to: " + locationToMoveTo);
            while (MoveToNextNode(locationToMoveTo)) { yield return null; }
            yield return new WaitForSeconds(0.1f);
            spaces--;
        }
        isMoving = false;
    }
    bool MoveToNextNode(UnityEngine.Vector3 goal)
    {
        return goal != (transform.position = UnityEngine.Vector3.MoveTowards(transform.position,locationToMoveTo,moveSpeed * Time.deltaTime));       
    }
}
