using UnityEngine;
using TMPro;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;


public class MultiplayerGameControl : MonoBehaviourPunCallbacks {

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

     void Awake()
    {
        if (!StateNameController.OnlineMultiplayerTrue){
            this.enabled = false;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Use this for initialization
    void Start () {
        if (!this.enabled) return;
        // Get the number of players passed from the ChooseGame scene via the StateNameController
        numOfPlayers = StateNameController.numberOfPlayers;

        if (!StateNameController.OnlineMultiplayerTrue) {
            // Fill in the players GameObject array.  Note, only active game objects can be found.
            players = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.GetComponent<FollowThePath>().playerNumber).Take(numOfPlayers).ToArray();
        } else {
            // players[0] = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.GetComponent<FollowThePath>().playerNumber).First();
            players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length == 0) {
                Debug.Log("No GameObjects tagged wtih Player");
            }
            
        }
        

        // No one has won if the game just started.
        whoWinsText.gameObject.SetActive(false);

        // Increase the size of player 1's token since they go first
        originalTokenScale = players[0].transform.localScale;
        players[0].transform.localScale *= scaleMultiple;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError($"Player {newPlayer} joined the room.");
        StateNameController.numberOfPlayers += 1;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogError($"Player {otherPlayer} left the room.");
        StateNameController.numberOfPlayers -= 1;
    }

    void Update()
    {
        if (!this.enabled) return;

        // Adjust the camera position.  This should follow the player's token around.
        // float threshold = 0.75f;
        // float distance = Vector3.Distance(cameraSystem.transform.position, players[0].transform.position);
        // float speed = 0.2f;
        // // Debug.Log("Dist b/w camera and player is " + distance);
        // if (distance > threshold) {
        //     while (cameraSystem.transform.position != players[0].transform.position){
        //         cameraSystem.transform.position = Vector3.MoveTowards(cameraSystem.transform.position, players[0].transform.position, 
        //             speed * Time.deltaTime);
        //     }
        // }
        // Game Over mechanics - want the winning token to be bigger than the rest of the tokens.
        // Note this is necessary because each player has their own coroutine so game over isn't captured before the turn is passed to the next player.
        // Only want this code to run once.
        // if (gameOver != prevGameOver) {
        //     foreach (GameObject player in players) {
        //         if (player == players[winningPlayerNumber - 1]) {
        //             player.transform.localScale *= scaleMultiple;
        //         } else {
        //             player.transform.localScale = originalTokenScale;
        //         }
        //     }
        // }
        // prevGameOver = gameOver;

        // Center the winning token and animate it.
        if (gameOver) {
            cameraSystem.transform.position = players[0].transform.position;
            players[0].transform.Rotate(0.0f, 1.0f, 0.0f);
        }
        
    }


    /// <summary>
    /// Calls the Move function from the followThePath class for the player object whose turn it currently is.
    /// </summary>
    public void MovePlayer(int rollValue)
    {
        if (!this.enabled) return;
        Debug.Log("Game over is " + gameOver);
        if (!gameOver)
        {
            StartCoroutine(players[0].GetComponent<FollowThePath>().Move(rollValue));
        }
    
    }
    /// <summary>
    /// Increments whoseTurn to the next player
    /// </summary>
    public void NextTurn()
    {
        if (!this.enabled) return;
        if (!gameOver){
            int prevTurn = whoseTurn;
            whoseTurn++;
            if (whoseTurn > numOfPlayers)
            {
                whoseTurn = 1;
            }

            // Increase the scale of the token for the player whose turn it is and make sure last token is reduced.
            players[0].transform.localScale /= scaleMultiple;
            players[0].transform.localScale *= scaleMultiple;

            // Skip the player if they had lost their turn
            if (players[0].GetComponent<FollowThePath>().nextTurnSkipped)
            {
                players[0].GetComponent<FollowThePath>().nextTurnSkipped = false;
                NextTurn();
            }
        }
        
    }
    /// <summary>
    /// Resolves the special tile when a player lands on it
    /// </summary>
    public void ResolveTile(string tileType)
    {
        if (!this.enabled) return;
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
                Debug.Log("Swap Places Tile");
                SwapPlayers();
                break;
        }
    }

    /// <summary>
    /// Updates the player turn text, called after a piece is finished moving
    /// </summary>
    public void UpdatePlayerTurnText()
    {
        if (!this.enabled) return;
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
        if (!this.enabled) return;
        if (players[0].GetComponent<FollowThePath>().waypointIndex == players[0].GetComponent<FollowThePath>().waypoints.Count - 1)
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
        return;
        if (!this.enabled) return;
        if (numOfPlayers < 2){
            return;
        }
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
        return;
        if (!this.enabled) return;
        if (numOfPlayers < 2){
            return;
        }
        players[whoseTurn - 1].GetComponent<FollowThePath>().nextTurnSkipped = true;
    }
        /// <summary>
    /// Sets a boolean so a random player's, not including the current player, next turn is skipped
    /// </summary>
    public void SkipRandomPlayerTurn()
    {
        return;
        if (!this.enabled) return;
        if (numOfPlayers < 2){
            return;
        }
        int playerSkipped;
                do {
                    playerSkipped = Random.Range(1, numOfPlayers+1);
                } while(playerSkipped == whoseTurn);
                Debug.Log("Player " + playerSkipped + " lost their turn");
        players[playerSkipped - 1].GetComponent<FollowThePath>().nextTurnSkipped = true;
    }

}
