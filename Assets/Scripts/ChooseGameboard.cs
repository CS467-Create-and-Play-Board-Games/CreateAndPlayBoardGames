using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine.UI;

public class ChooseGameboard : MonoBehaviour
{
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
                string lastModified = File.GetLastWriteTime(file).ToString("MM/dd/yy HH:mm");
                string gameInfo = name + " \t " + lastModified;
                result.Add(gameInfo);
                Debug.Log(gameInfo);
            }
        }
        return result;
    }

    public GameObject buttonContentHolder, buttonTemplate;
    // [SerializeField] private GameObject buttonTemplate;
    void GenerateButtons(GameObject buttonContentHolder, List<string> files)
    {
        // TODO:  https://www.youtube.com/watch?v=2TYLBusJKjc
        foreach (string file in files)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);
            button.GetComponentInChildren<TextMeshProUGUI>().text = file;
            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }
        
    }
    

    // Awake is called before Start
    void Awake()
    {
        List<string> fileList = ProcessFiles();
        GenerateButtons(buttonContentHolder, fileList);
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
