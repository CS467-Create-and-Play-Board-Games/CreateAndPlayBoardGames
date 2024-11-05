using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameControl : MonoBehaviour {

    private static GameObject player1MoveText, player2MoveText;

    private static GameObject player1, player2, player3;

    public TMP_Text whoWinsText;

    private int whoseTurn = 1;
    public int numOfPlayers;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;
    public static int player3StartWaypoint = 0;
    public static int player4StartWaypoint = 0;

    public static bool gameOver = false;

    // Use this for initialization
    void Start () {

        //whoWinsText = GameObject.Find("WhoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player3 = GameObject.Find("Player3");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;
        player3.GetComponent<FollowThePath>().moveAllowed = false;

        whoWinsText.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.GetComponent<FollowThePath>().waypointIndex == player1.GetComponent<FollowThePath>().waypoints.Count - 1)
        {
            whoWinsText.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsText.text = "Player 1 Wins";
            gameOver = true;
        }
        if (player2.GetComponent<FollowThePath>().waypointIndex == player2.GetComponent<FollowThePath>().waypoints.Count - 1)
        {
            whoWinsText.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsText.text = "Player 2 Wins";
            gameOver = true;
        }
        if (player3.GetComponent<FollowThePath>().waypointIndex == player3.GetComponent<FollowThePath>().waypoints.Count - 1)
        {
            whoWinsText.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsText.text = "Player 3 Wins";
            gameOver = true;
        }
    }

    public void MovePlayer(int roll_value)
    {
        switch (whoseTurn) { 
            case 1:
                if(!gameOver)
                {
                    StartCoroutine(player1.GetComponent<FollowThePath>().Move(roll_value));
                }
                break;

            case 2:
                if(!gameOver)
                {
                    StartCoroutine(player2.GetComponent<FollowThePath>().Move(roll_value));
                }
                break;

            case 3:
                if(!gameOver)
                {
                    StartCoroutine(player3.GetComponent<FollowThePath>().Move(roll_value));
                }
                break;

            // case 4:
            //    if(!gameOver)
            //    {
            //      StartCoroutine(player4.GetComponent<FollowThePath>().Move(roll_value));
            //    }
            //    break;

        }
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
