// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using System.IO.Enumeration;
using TMPro;

public class ButtonListButton : MonoBehaviour
{

    public void LoadSceneByName()
    {
        string buttonText = GetComponentInChildren<TextMeshProUGUI>().text;
        int tabPos = buttonText.IndexOf("  ");
        string fileName = buttonText[..tabPos] + ".json";
        // string folder = Application.dataPath + "/Boards/";
        string folder = Application.persistentDataPath + "/Boards/";
        string filePath = folder + fileName;
        Debug.Log("You clicked on the button associated with this file path " + filePath);
        StateNameController.filePathForGame = filePath;
        if (StateNameController.clickedButtonText.ToLower() == "play"){
            SceneManager.LoadScene("Play");
        } else {
            SceneManager.LoadScene("CreateEdit");
        }
        
    }

}
