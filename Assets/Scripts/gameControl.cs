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
        }
    
    }
    /// <summary>
    /// Increments whoseTurn to the next player
    /// </summary>
    public void NextTurn()
    {
        whoseTurn++;
        if (whoseTurn > numOfPlayers)
        {
            whoseTurn = 1;
        }
        if (players[whoseTurn - 1].GetComponent<FollowThePath>().nextTurnSkipped)
        {
            players[whoseTurn - 1].GetComponent<FollowThePath>().nextTurnSkipped = false;
            NextTurn();
        }
    }
    /// <summary>
    /// Resolves the special tile when a player lands on it
    /// </summary>
    public void ResolveTile(string tileType)
    {
        switch (tileType)
        {
            case "Blank Tile":
                break;
            case "Start Tile":
                break;
            case "Finish Tile":
                break;
            case "Skip Tile":
                Debug.Log("Skip Tile");
                int playerSkipped;
                do {
                    playerSkipped = Random.Range(1, numOfPlayers+1);
                } while(playerSkipped == whoseTurn);
                Debug.Log("Player " + playerSkipped + " lost their turn");
                players[playerSkipped-1].GetComponent<FollowThePath>().nextTurnSkipped = true;
                break;
            case "Lose Turn Tile":
                Debug.Log("Player " + whoseTurn + " lost their turn");
                players[whoseTurn - 1].GetComponent<FollowThePath>().nextTurnSkipped = true;
                break;
            case "Swap Places Tile":
                Debug.Log("Swap Places Tile");
                break;
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
