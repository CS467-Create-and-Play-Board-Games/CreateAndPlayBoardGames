// using System.Diagnostics;
using UnityEngine;
using Photon.Pun;
using TMPro;


// https://www.youtube.com/watch?v=93SkbMpWCGo
public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI createInput;
    public TextMeshProUGUI joinInput;
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        Debug.Log("JoinRoom clicked and joinInput is " + joinInput.text);
        if (!PhotonNetwork.IsConnected) {
            Debug.Log("How is photon not connected already?");
            PhotonNetwork.ConnectUsingSettings();
        }
        
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Play");
    }

}
