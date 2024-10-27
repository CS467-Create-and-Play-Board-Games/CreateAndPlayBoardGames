using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    public void goToScene()
    {
        Debug.Log(gameObject.name + " clicked.");
        if (gameObject.name == "Exit"){
            toggleExitText(gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        else{
            SceneManager.LoadScene(gameObject.name);
        }
    }

    private void toggleExitText(string buttonText)
    {
        switch (buttonText){
            case "Exit":
                gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("Exiting");
                break;
            default:
                gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("Exit");
                break;
        }
    }
}
