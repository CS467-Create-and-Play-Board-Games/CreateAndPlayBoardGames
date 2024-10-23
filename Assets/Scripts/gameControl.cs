using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsText, player1MoveText, player2MoveText;

    private static GameObject player1, player2, player3;

    private int whoseTurn = 1;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;
    public static int player3StartWaypoint = 0;
    public static int player4StartWaypoint = 0;

    public static bool gameOver = false;

    // Use this for initialization
    void Start () {

        whoWinsText = GameObject.Find("WhoWinsText");
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

    }

    public void MovePlayer(int roll_value)
    {
        switch (whoseTurn) { 
            case 1:
                StartCoroutine(player1.GetComponent<FollowThePath>().Move(roll_value));
                //player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 2:
                StartCoroutine(player2.GetComponent<FollowThePath>().Move(roll_value));
                //player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 3:
                StartCoroutine(player3.GetComponent<FollowThePath>().Move(roll_value));
                //player3.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            // case 4:
            //    player4.GetComponent<FollowThePath>().moveAllowed = true;
            //    break;

        }
        if (whoseTurn >= 3)
        {
            whoseTurn = 1;
        }
        else
        {
            whoseTurn++;
        }
    }
}
