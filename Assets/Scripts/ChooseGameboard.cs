using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;


public class ChooseGameboard : MonoBehaviour
{
    public GameObject buttonContentHolder, buttonTemplate;
    public TextMeshProUGUI numberOfPlayersText, players;
    public GameObject plusButton, minusButton, startFromScratchButton;

    // Awake is called before Start
    void Awake()
    {
        // Display either the "Start from Scratch" button or the player selection dialog.
        Debug.Log("StateNameController.clickedButtonText = " + StateNameController.clickedButtonText);
        List<string> fileList = ProcessFiles();
        GenerateButtons(buttonContentHolder, fileList);
        if (StateNameController.clickedButtonText.ToLower().Contains("play")){
            StateNameController.numberOfPlayers = 2;
            numberOfPlayersText.gameObject.SetActive(true);
            players.gameObject.SetActive(true);
            plusButton.SetActive(true);
            minusButton.SetActive(true);
            startFromScratchButton.SetActive(false);
        }
    }
    
    List<string> ProcessFiles()
    {
        // string sourceDir = Application.dataPath + "/Boards/";
        string sourceDir = Application.persistentDataPath + "/Boards/";
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
                Debug.Log(gameInfo.ToString());
            }
        }
        return result;
    }

    
    void GenerateButtons(GameObject buttonContentHolder, List<string> files)
    {
        // Button generation based on https://www.youtube.com/watch?v=2TYLBusJKjc
        foreach (string file in files)
        {
            Debug.Log("GenerateButtons file is " + file);
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

    public void IncreasePlayerCount(){
        if (StateNameController.numberOfPlayers < 8) {
            StateNameController.numberOfPlayers += 1;
        }
        players.text = StateNameController.numberOfPlayers.ToString();
    }

    public void DecreasePlayerCount(){
        if (StateNameController.numberOfPlayers > 2) {
            StateNameController.numberOfPlayers -= 1;
        }
        players.text = StateNameController.numberOfPlayers.ToString();
    }
}
