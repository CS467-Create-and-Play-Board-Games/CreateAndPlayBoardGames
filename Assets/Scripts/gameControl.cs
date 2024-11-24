using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEditor.Rendering;
using Unity.VisualScripting;


public class GameControl : MonoBehaviour {

    // private static GameObject[] players;
    private GameObject[] players;

    public TMP_Text whoWinsText;
    public TMP_Text playerMoveText;

    private int whoseTurn = 1;
    private int numOfPlayers;
    private int winningPlayerNumber;
    public float scaleMultiple = 1.5f;
    private Vector3 originalTokenScale;

    private bool gameOver = false;
    private bool prevGameOver = false;
    [SerializeField] GameObject cameraSystem;

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

        // Increase the size of player 1's token since they go first
        originalTokenScale = players[0].transform.localScale;
        players[0].transform.localScale *= scaleMultiple;
    }

    void Update()
    {
        // Adjust the camera position depending on whose turn it is.
        float threshold = 0.75f;
        float distance = Vector3.Distance(cameraSystem.transform.position, players[whoseTurn -1].transform.position);
        float speed = 0.2f;
        // Debug.Log("Dist b/w camera and player is " + distance);
        if (distance > threshold) {
            while (cameraSystem.transform.position != players[whoseTurn - 1].transform.position){
                cameraSystem.transform.position = Vector3.MoveTowards(cameraSystem.transform.position, players[whoseTurn - 1].transform.position, 
                    speed * Time.deltaTime);
            }
            
        }

        // Game Over mechanics - want the winning token to be bigger than the rest of the tokens.
        // Note this is necessary because each player has their own coroutine so game over isn't captured before the turn is passed to the next player.
        // Only want this code to run once.
        if (gameOver != prevGameOver) {
            foreach (GameObject player in players) {
                if (player == players[winningPlayerNumber - 1]) {
                    player.transform.localScale *= scaleMultiple;
                } else {
                    player.transform.localScale = originalTokenScale;
                }
            }
        }
        prevGameOver = gameOver;

        // Center the winning token and animate it.
        if (gameOver) {
            cameraSystem.transform.position = players[winningPlayerNumber - 1].transform.position;
            players[winningPlayerNumber - 1].transform.Rotate(0.0f, 1.0f, 0.0f);
        }

        
        
        
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
        if (!gameOver){
            int prevTurn = whoseTurn;
            whoseTurn++;
            if (whoseTurn > numOfPlayers)
            {
                whoseTurn = 1;
            }

            // Increase the scale of the token for the player whose turn it is and make sure last token is reduced.
            players[prevTurn - 1].transform.localScale /= scaleMultiple;
            players[whoseTurn - 1].transform.localScale *= scaleMultiple;

            // Skip the player if they had lost their turn
            if (players[whoseTurn - 1].GetComponent<FollowThePath>().nextTurnSkipped)
            {
                players[whoseTurn - 1].GetComponent<FollowThePath>().nextTurnSkipped = false;
                NextTurn();
            }
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
            case "Start Tile":
            case "Finish Tile":
                break;
            case "Skip Tile":
                Debug.Log("Skip Tile");
                SkipRandomPlayerTurn();
                break;
            case "Lose Turn Tile":
                Debug.Log("Lose Turn Tile");
                Debug.Log("Player " + whoseTurn + " lost their turn");
                SkipCurrentPlayerTurn();
                break;
            case "Swap Places Tile":
                SwapPlayers();
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
            winningPlayerNumber = playerNum;
        }
    }

    /// <summary>
    /// Swaps two players using the player numbers passed in
    /// </summary>
    public void SwapPlayers()
    {
        int swapTarget;
                do {
                    swapTarget = Random.Range(1, numOfPlayers+1);
                } while(swapTarget == whoseTurn);
        Debug.Log("Swapping Player " + whoseTurn + " and Player " + swapTarget);
        int waypointIndexA = players[whoseTurn - 1].GetComponent<FollowThePath>().waypointIndex;
        int waypointIndexB = players[swapTarget - 1].GetComponent<FollowThePath>().waypointIndex;
        players[whoseTurn - 1].GetComponent<FollowThePath>().waypointIndex = waypointIndexB;
        players[swapTarget - 1].GetComponent<FollowThePath>().waypointIndex = waypointIndexA;
        UnityEngine.Vector3 locationToMoveTo = players[whoseTurn - 1].GetComponent<FollowThePath>().waypoints[waypointIndexB];
        StartCoroutine(players[whoseTurn - 1].GetComponent<FollowThePath>().MoveForSwapPlayers(locationToMoveTo));
        locationToMoveTo = players[swapTarget - 1].GetComponent<FollowThePath>().waypoints[waypointIndexA];
        StartCoroutine(players[swapTarget - 1].GetComponent<FollowThePath>().MoveForSwapPlayers(locationToMoveTo));
    }
    /// <summary>
    /// Sets a boolean so the current player's next turn is skipped
    /// </summary>
    public void SkipCurrentPlayerTurn()
    {
        players[whoseTurn - 1].GetComponent<FollowThePath>().nextTurnSkipped = true;
    }
        /// <summary>
    /// Sets a boolean so a random player's, not including the current player, next turn is skipped
    /// </summary>
    public void SkipRandomPlayerTurn()
    {
        int playerSkipped;
                do {
                    playerSkipped = Random.Range(1, numOfPlayers+1);
                } while(playerSkipped == whoseTurn);
                Debug.Log("Player " + playerSkipped + " lost their turn");
        players[playerSkipped - 1].GetComponent<FollowThePath>().nextTurnSkipped = true;
    }

}
