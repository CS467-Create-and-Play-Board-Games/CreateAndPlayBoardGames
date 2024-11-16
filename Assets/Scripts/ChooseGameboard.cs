using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine.UI;
using System;

public class ChooseGameboard : MonoBehaviour
{
    public GameObject buttonContentHolder, buttonTemplate;

    // Awake is called before Start
    void Awake()
    {
        List<string> fileList = ProcessFiles();
        GenerateButtons(buttonContentHolder, fileList);
    }
    
    List<string> ProcessFiles()
    {
        string sourceDir = Application.dataPath + "/Boards/";
        string[] files = Directory.GetFiles(sourceDir);
        List<string> result = new List<string>();
        foreach (string file in files)
        {
            if (file.EndsWith(".json", true, null))
            {
                string name = file.Substring(sourceDir.Length, (file.Length - sourceDir.Length) - 5);
                int linesInFile = File.ReadAllLines(file).Length;
                int tileCount = (int) (linesInFile * 0.1666667 - 0.6666667);
                string lastModified = File.GetLastWriteTime(file).ToString("MM/dd/yy HH:mm");
                string gameInfo = name + "  " + tileCount + " tiles  " + lastModified;
                result.Add(gameInfo);
                Debug.Log(gameInfo);
            }
        }
        return result;
    }

    
    void GenerateButtons(GameObject buttonContentHolder, List<string> files)
    {
        // Button generation based on https://www.youtube.com/watch?v=2TYLBusJKjc
        foreach (string file in files)
        {
            // Prevent button creation for the _New (Blank) board if the user wants to play a game b/c the blank board is unplayable.
            if (StateNameController.clickedButtonText.ToLower() == "play" && file.Contains("_New (Blank)")) {
                continue;
            }
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);
            button.GetComponentInChildren<TextMeshProUGUI>().text = file;
            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }
        
    }
}
