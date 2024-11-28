using UnityEngine;
using TMPro;
using Photon.Pun;

public class RollDice : MonoBehaviour
{
    // public int sides;
    private int rollValue;
    public TMP_Text rollResult;
    private GameObject _gameController, _multiplayerGameController;
    PhotonView view;
 

    void Awake()
    {
        _gameController = GameObject.Find("GameControl");
        _multiplayerGameController = GameObject.Find("MultiplayerGameControl");
    }

    private void Start()
    {
        // view = GameObject.Find("Player1").GetComponent<PhotonView>();
        // view = GameObject.GetComponentInChildren<PhotonView>();
        // if (StateNameController.OnlineMultiplayerTrue){
        //     view = GetComponent<PhotonView>();
        //     Debug.Log("The view is " + view.ToString());
        // }
        
    }

    /// <summary>
    /// Rolls a dices based on the sides value of the dice object
    /// </summary>
    private int Roll(int sides)
    {
        return Random.Range(sides, 0);
    }

    /// <summary>
    /// Calls roll() and then calls the MovePlayer function of _gameController to move the player the number of spaces rolled by the die
    /// </summary>
    public void RollAndMovePlayer(int sides)
    {
        // if (view.IsMine)
        // {
            rollValue = Roll(sides);
            Debug.Log("rollValue = " + rollValue);
            rollResult.text = rollValue.ToString();
            Debug.Log("rollResult.text = " + rollResult.text);
            if (StateNameController.OnlineMultiplayerTrue) {
                _multiplayerGameController.GetComponent<MultiplayerGameControl>().MovePlayer(rollValue);
            } else {
                _gameController.GetComponent<GameControl>().MovePlayer(rollValue);
            }
            // _gameController.GetComponent<GameControl>().MovePlayer(rollValue);
            // StartCoroutine(_gameController.players[whoseTurn - 1].GetComponent<FollowThePath>().Move(rollValue));
        // }
    }
}

