// using System.Collections;
// using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;

// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string _sceneName;
    public void goToScene(string sceneName)
    {
        Debug.Log(gameObject.name + " clicked which has a sceneName of " + sceneName);
        switch (sceneName.ToLower())
        {
            case "exit":
                Debug.Log("Exit clicked");
                toggleExitText(gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
                Application.Quit();
                break;
            
            case "play":
                Debug.Log("Play clicked");
                StateNameController.numberOfPlayers = 2;
                StateNameController.OnlineMultiplayerTrue = false;
                StateNameController.clickedButtonText = sceneName;
                SceneManager.LoadScene("ChooseGame");
                break;
            
            case "loading":
                Debug.Log("Online Multiplayer clicked");
                StateNameController.OnlineMultiplayerTrue = true;
                SceneManager.LoadScene(sceneName);
                break;

            default:
                Debug.Log("Something other than play or online multiplayer was selected");
                StateNameController.clickedButtonText = sceneName;
                if (StateNameController.OnlineMultiplayerTrue) {
                    StateNameController.OnlineMultiplayerTrue = false;
                    if (PhotonNetwork.IsConnected) {
                        PhotonNetwork.Disconnect();
                    }
                }
                SceneManager.LoadScene(sceneName);
                break;
        }

        // if (sceneName.ToLower() == "exit"){
        //     Debug.Log("Exit clicked");
        //     toggleExitText(gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        //     Application.Quit();
        // }
        // if (sceneName.ToLower() == "play"){
        //     Debug.Log("Play clicked");
        //     StateNameController.numberOfPlayers = 2;
        //     SceneManager.LoadScene("ChooseGame");
        // }
        // if (sceneName.ToLower() == "loading"){
        //     Debug.Log("Online Multiplayer clicked");
        //     StateNameController.OnlineMultiplayerTrue = true;
        // }
        // StateNameController.clickedButtonText = sceneName;
        // SceneManager.LoadScene(sceneName);
        
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
