using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;

public class GameControl : MonoBehaviour {

    private static GameObject[] players;

    public TMP_Text whoWinsText;
    public TMP_Text playerMoveText;

    private int whoseTurn = 1;
    private int numOfPlayers;

    private bool gameOver = false;

    // Use this for initialization
    void Start () {

        //whoWinsText = GameObject.Find("WhoWinsText");

        players = GameObject.FindGameObjectsWithTag("Player");
        players = players.OrderBy(go => go.GetComponent<FollowThePath>().playerNumber).ToArray();
        foreach (GameObject obj in players)
        {
            Debug.Log(obj.GetComponent<FollowThePath>().playerNumber);
        }
        numOfPlayers = players.Length;


        whoWinsText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Calls the Move function from the followThePath class for the player object whose turn it currently is.
    /// </summary>
    public void MovePlayer(int rollValue)
    {
        Debug.Log("Game over is " + gameOver);
        if (!gameOver)
        {
            StartCoroutine(players[whoseTurn - 1].GetComponent<FollowThePath>().Move(rollValue));
            if (whoseTurn >= numOfPlayers)
            {
                whoseTurn = 1;
            }
            else
            {
                whoseTurn++;
            }
        }
    
    }
    /// <summary>
    /// Updates the player turn text, called after a piece is finished moving
    /// </summary>
    public void UpdatePlayerTurnText()
    {
        if (!gameOver)
        {
            playerMoveText.text = "Player " + whoseTurn.ToString() + "'s turn";
        }
    }
    /// <summary>
    /// Checks if the player with the passed in player number has reached the goal, declares them the winner if they have
    /// </summary>
    public void CheckForGameOver(int playerNum)
    {
        if (players[playerNum-1].GetComponent<FollowThePath>().waypointIndex == players[playerNum-1].GetComponent<FollowThePath>().waypoints.Count - 1)
        {
            whoWinsText.gameObject.SetActive(true);
            playerMoveText.gameObject.SetActive(false);
            whoWinsText.text = "Player " + playerNum.ToString() + " Wins";
            gameOver = true;
        }
    }
}
