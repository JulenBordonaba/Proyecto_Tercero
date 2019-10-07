using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connect : Photon.PunBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v1.0");
    }

    public override void OnJoinedRoom()
    {
        //SceneManager.LoadScene("Blocking Nivel");
        PhotonNetwork.LoadLevel("Blocking Nivel");
    }

    public override void OnConnectedToPhoton()
    {
        Debug.Log(">>>>>>>>> Conectado a Photon");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(">>>>>>>>> Conectado al servidor");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public override void OnJoinedLobby()
    {
        RoomOptions ops;
        
        ops = new RoomOptions();
        
        ops.MaxPlayers = (byte)30;
        ops.IsVisible = true;
        ops.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Pruebas",ops,TypedLobby.Default);
    }
}
