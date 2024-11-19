using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;


public class GameControl : MonoBehaviour {

    // private static GameObject[] players;
    private GameObject[] players;

    public TMP_Text whoWinsText;
    public TMP_Text playerMoveText;

    private int whoseTurn = 1;
    private int numOfPlayers;

    private bool gameOver = false;

    // Use this for initialization
    void Start () {

        // Get the number of players passed from the ChooseGame scene via the StateNameController
        numOfPlayers = StateNameController.numberOfPlayers;

        // Fill in the players GameObject array.  Note, only active game objects can be found.
        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.GetComponent<FollowThePath>().playerNumber).Take(numOfPlayers).ToArray();

        // Remove the non-player tokens from the board by deactivating them
        GameObject[] nonPlayers = GameObject.FindGameObjectsWithTag("Player").Except(players).ToArray();
        foreach (GameObject nonPlayer in nonPlayers) {
            nonPlayer.SetActive(false);
        }

        // No one has won if the game just started.
        whoWinsText.gameObject.SetActive(false);
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
