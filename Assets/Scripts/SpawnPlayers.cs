using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Tilemaps;
using UnityEditor;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Tilemap tilemap;

    private List<Sprite> sprites;
    public TokenSelectionData tokenData;
    private UnityEngine.Vector3[] playerOffsets = new UnityEngine.Vector3[8];


    private void Start()
    {
        sprites = new List<Sprite>(tokenData.playerSelectedTokens.Values);
        Debug.Log("Start for SpawnPlayers is running.");
        Vector3 startPosition = GetPosition("start");
        playerOffsets[0] = new UnityEngine.Vector3(0.05f, 0.20f, 0.0f);
        playerOffsets[1] = new UnityEngine.Vector3(0.20f, 0.20f, 0.0f);
        playerOffsets[2] = new UnityEngine.Vector3(0.05f, 0.05f, 0.0f);
        playerOffsets[3] = new UnityEngine.Vector3(0.20f, 0.05f, 0.0f);
        playerOffsets[4] = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(0.05f, 0.20f, 0.0f), new UnityEngine.Vector3(0.7f, 0.7f, 0));
        playerOffsets[5] = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(0.20f, 0.20f, 0.0f), new UnityEngine.Vector3(0.7f, 0.7f, 0));
        playerOffsets[6] = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(0.05f, 0.05f, 0.0f), new UnityEngine.Vector3(0.7f, 0.7f, 0));
        playerOffsets[7] = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(0.20f, 0.05f, 0.0f), new UnityEngine.Vector3(0.7f, 0.7f, 0));
           
        if (!StateNameController.OnlineMultiplayerTrue) {
            for (int i = 0; i < StateNameController.numberOfPlayers; i++) 
            {
                GameObject player = Instantiate(playerPrefab, startPosition, Quaternion.identity);
                player.transform.parent = GameObject.Find("Players").transform;
                player.GetComponent<SpriteRenderer>().sprite = sprites[i];
                player.GetComponent<FollowThePath>().playerNumber = i + 1;
                player.GetComponent<FollowThePath>().myOffset = playerOffsets[i];
            }
        }
        if (StateNameController.OnlineMultiplayerTrue) {
            Debug.Log("Instantiating player for online multiplayer game.");
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, startPosition, Quaternion.identity);
            player.transform.parent = GameObject.Find("Players").transform;
            player.GetComponent<FollowThePath>().playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            player.GetComponent<SpriteRenderer>().sprite = sprites[player.GetComponent<FollowThePath>().playerNumber - 1];
            player.GetComponent<FollowThePath>().myOffset = playerOffsets[player.GetComponent<FollowThePath>().playerNumber - 1];
            
        }
        
    }

    Vector3 GetPosition(string tileName)
    {
    BoundsInt bounds = tilemap.cellBounds;
    foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null){
                if (tile.sprite.name.ToLower().Contains(tileName)){
                    return tilemap.CellToWorld(pos);
                }
            } 
        }
        return new Vector3();
    }

}
