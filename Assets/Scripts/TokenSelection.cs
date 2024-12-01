using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TokenSelection : MonoBehaviour
{
    public TokenSelectionData tokenData;
    public GameObject playerPreviewPrefab;
    public Transform previewPanelContainer;
    public int numberOfPlayers = 4; //can be changed based on how many players want to play. will need to be set up in other scene
    private int currentPlayerID = 1; // set as the starting point for the confirmSelection iteration 

    private GameObject previousToken = null;
    private bool pickedToken = false;
    private Dictionary<int, GameObject> playerPreviewPanel = new Dictionary<int, GameObject>();
    public Dictionary<int, Sprite> playerSelectedTokens = new Dictionary<int, Sprite>();

    private void Start()
    {
        numberOfPlayers = StateNameController.numberOfPlayers;
        CreatePreviewPanels();
    }

    // dynamically creates player preview panels in token select based on the number of players playing the game
    private void CreatePreviewPanels()
    {
        for (int i = 1; i <= numberOfPlayers; i++)
        {
            GameObject previewPanel = Instantiate(playerPreviewPrefab, previewPanelContainer);
            previewPanel.name = $"PlayerPreviewPanel_{i}";

            Debug.Log($"creating preview panel for player {i}");

            // sets theplayer id text in the player preview panel
            Text playerIDText = previewPanel.transform.Find("PlayerID")?.GetComponent<Text>();
            playerIDText.text = $"Player {i}";
            

            // disables confirm button for the players who's token selection turn is not up yet 
            Button confirmButton = previewPanel.transform.Find("ConfirmButton")?.GetComponent<Button>();
            confirmButton.interactable = false;

            // Save in the dictionary
            playerPreviewPanel[i] = previewPanel;
        }
        UpdateTurnUI();
    }
    // allows players to choose their playing token 
    public void TokenSelect(GameObject token)
    {
        
        if (playerPreviewPanel.ContainsKey(currentPlayerID))
        {
            GameObject previewPanel = playerPreviewPanel[currentPlayerID];
            Image previewImage = previewPanel.transform.Find("TokenImage").GetComponent<Image>();
            Button confirmButton = previewPanel.transform.Find("ConfirmButton").GetComponent<Button>();

            // set the selected token image and enable confirm button
            previewImage.sprite = token.GetComponent<Image>().sprite;
            confirmButton.interactable = true;
            //makes the token option unslectable for other players
            if (previousToken != null)
            {
                previousToken.GetComponent<Button>().interactable = true;
                previousToken = null;
            }

            previousToken = token;
            token.GetComponent<Button>().interactable = false;
            pickedToken = true;
            

            Debug.Log($"Player {currentPlayerID} selected Token {token.name}");
        }
    }
    //manages turn order and makes sure not more than the number of players in the game choose a token
    public void ConfirmSelection()
    {
        if (pickedToken == true){        
            GameObject previewPanel = playerPreviewPanel[currentPlayerID];
            Sprite selectedSprite = previewPanel.transform.Find("TokenImage").GetComponent<Image>().sprite;
            playerSelectedTokens[currentPlayerID] = selectedSprite;

            tokenData.playerSelectedTokens[currentPlayerID] = selectedSprite;

            previousToken = null;
            if (currentPlayerID < numberOfPlayers)
            {
                currentPlayerID++;
                UpdateTurnUI();
            }    
            else
            {
                SceneManager.LoadScene("Play");
            }
        }    
    }

    //updates ui so that only the player whos turn it is to select a token can select a token
    private void UpdateTurnUI()
    {   
        // finds the players confirm button in the dictionary 
        foreach (var kvp in playerPreviewPanel)
        {   
            // enables the confirm button for the player whos turn it is to select a token
            Button confirmButton = kvp.Value.transform.Find("ConfirmButton").GetComponent<Button>();
            confirmButton.interactable = kvp.Key == currentPlayerID;
        }
    }
}

