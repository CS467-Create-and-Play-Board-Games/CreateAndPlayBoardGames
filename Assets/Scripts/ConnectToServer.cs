// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // https://www.youtube.com/watch?v=93SkbMpWCGo
    // Start is called before the first frame update
    void Start()
    {
        // if (!PhotonNetwork.IsConnected) {
        PhotonNetwork.ConnectUsingSettings();
        // } else {
        //     OnConnectedToMaster();
        // }
    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
