using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollDice : MonoBehaviour
{
    public int sides;
    private int rollValue;
    public TMP_Text rollResult;
    private GameObject _gameController;

    void Awake()
    {
        _gameController = GameObject.Find("GameControl");
    }

    /// <summary>
    /// Rolls a dices based on the sides value of the dice object
    /// </summary>
    private int Roll()
    {
        return Random.Range(1, sides+1);
    }

    /// <summary>
    /// Calls roll() and then calls the MovePlayer function of _gameController to move the player the number of spaces rolled by the die
    /// </summary>
    public void RollAndMovePlayer()
    {
        rollValue = Roll();
        rollResult.text = rollValue.ToString();
        _gameController.GetComponent<GameControl>().MovePlayer(rollValue);
    }
}

