using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollDice : MonoBehaviour
{
    public int sides;
    private int roll_value;
    public TMP_Text roll_result;
    private GameObject gameController;

    void Awake()
    {
        gameController = GameObject.Find("GameControl");
    }

    public int roll()
    {
        return Random.Range(1, sides+1);
    }

    public void OnButtonClick()
    {
        roll_value = roll();
        roll_result.text = roll_value.ToString();
        gameController.GetComponent<GameControl>().MovePlayer(roll_value);
    }
}

