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
    private UnityEngine.Vector3 player1Offset = new UnityEngine.Vector3(0.0f, 0.25f, 0.0f);
    private UnityEngine.Vector3 player2Offset = new UnityEngine.Vector3(0.25f, 0.25f, 0.0f);
    private UnityEngine.Vector3 player4Offset = new UnityEngine.Vector3(0.25f, 0.0f, 0.0f);
    private UnityEngine.Vector3 locationToMoveTo;

    [SerializeField]
    private float moveSpeed = 1f;
    public float playerNumber = 0;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;

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
        else if (playerNumber == 4)
        {
            locationToMoveTo = waypoints[waypointIndex] + player4Offset;
        }
        else
        {
            locationToMoveTo = waypoints[waypointIndex];
        }
        transform.position = locationToMoveTo;
        // UnityEngine.Vector3 t = waypoints[waypointIndex];
        // transform.position = t;
        // Debug.Log(waypoints[waypointIndex].ToString() + "translated to " + t.ToString());
        Debug.Log(waypoints[waypointIndex].ToString());
	}
	
	// Update is called once per frame
	private void Update () {
        if (moveAllowed)
            Move();
	}

    private void Move()
    {
        // if (waypointIndex <= waypoints.Length - 1)
        if (waypointIndex <= waypoints.Count - 1)
        {
            // transform.position = Vector2.MoveTowards(transform.position,
            // waypoints[waypointIndex].transform.position,
            // moveSpeed * Time.deltaTime);
            if (playerNumber == 1)
            {
                locationToMoveTo = waypoints[waypointIndex] + player1Offset;
            }
            else if (playerNumber == 2)
            {
                locationToMoveTo = waypoints[waypointIndex] + player2Offset;
            }
            else if (playerNumber == 4)
            {
                locationToMoveTo = waypoints[waypointIndex] + player4Offset;
            }
            else
            {
                locationToMoveTo = waypoints[waypointIndex];
            }
            transform.position = UnityEngine.Vector3.MoveTowards(transform.position,
            locationToMoveTo,
            moveSpeed * Time.deltaTime);
            Debug.Log(waypoints[waypointIndex].ToString());

            // if (transform.position == waypoints[waypointIndex].transform.position)
            if (transform.position == waypoints[waypointIndex])
            {
                waypointIndex += 1;
            }
        }
    }
}
