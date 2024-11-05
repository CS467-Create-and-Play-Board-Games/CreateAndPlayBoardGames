using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Enumeration;
using TMPro;

public class ButtonListButton : MonoBehaviour
{

    public void LoadSceneByName()
    {
        string buttonText = GetComponentInChildren<TextMeshProUGUI>().text;
        Debug.Log("The button text is " + buttonText);
        int tabPos = buttonText.IndexOf('\t') - 1;
        string fileName = buttonText[..tabPos] + ".json";
        string folder = Application.dataPath + "/Boards/";
        string filePath = folder + fileName;
        Debug.Log("You clicked on the button associated with this file path " + filePath);
        StateNameController.filePathForGame = filePath;
        SceneManager.LoadScene("CreateEdit");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
