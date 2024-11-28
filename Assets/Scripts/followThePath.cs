using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FollowThePath : MonoBehaviourPunCallbacks, IPunObservable {

    public List<UnityEngine.Vector3> waypoints;

    private UnityEngine.Vector3 locationToMoveTo;
    private GameObject _gameController;
    public UnityEngine.Vector3 myOffset;
    private PhotonView photonView;

    [SerializeField]
    private float moveSpeed = 1f;
    public int playerNumber = 0;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;
    public bool isMoving;
    public bool nextTurnSkipped = false;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

	// Use this for initialization
	private void Start () {

        _gameController = GameObject.Find("GameControl");
        waypoints = GameObject.Find("Tilemap").GetComponent<TileLocations>()._worldGameTileLocation;

        locationToMoveTo = waypoints[waypointIndex] + myOffset;

        transform.position = locationToMoveTo;
        Debug.Log(waypoints[waypointIndex].ToString());
	}
	
    /// <summary>
    /// Handles moving a token the number of spaces rolled. Calls the MoveToNextNode function to move a token each space.
    /// </summary>
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
            // locationToMoveTo = waypoints[waypointIndex] + playerOffsets[playerNumber-1];
            locationToMoveTo = waypoints[waypointIndex] + myOffset;
            Debug.Log("current: " + transform.position);
            Debug.Log("moving to: " + locationToMoveTo);
            while (MoveToNextNode(locationToMoveTo)) { yield return null; }
            yield return new WaitForSeconds(0.1f);
            spaces--;
        }
        isMoving = false;
        _gameController.GetComponent<GameControl>().ResolveTile(GameObject.Find("Tilemap").GetComponent<TileLocations>()._tileTypeOrder[waypointIndex]);
        _gameController.GetComponent<GameControl>().NextTurn();
        _gameController.GetComponent<GameControl>().CheckForGameOver(playerNumber);
        _gameController.GetComponent<GameControl>().UpdatePlayerTurnText();

    }
    /// <summary>
    /// Moves a token to the goal tile and returns whether or not the token has gotten there yet
    /// </summary>
    bool MoveToNextNode(UnityEngine.Vector3 goal)
    {
        return goal != (transform.position = UnityEngine.Vector3.MoveTowards(transform.position,locationToMoveTo,moveSpeed * Time.deltaTime));       
    }
    /// <summary>
    /// Moves a token to a location, used for the SwapPlayers function.
    /// </summary>
    public IEnumerator MoveForSwapPlayers(UnityEngine.Vector3 goal)
    {
        locationToMoveTo = goal + myOffset;
        while (MoveToNextNode(locationToMoveTo)) { yield return null; }
            yield return new WaitForSeconds(0.1f);
    }

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting){
            stream.SendNext(gameObject.GetComponent<SpriteRenderer>().sprite);
        }
    }

    #endregion

}
