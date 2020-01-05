using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connect : Photon.PunBehaviour
{
    
    // Start is called before the first frame update
    private bool joinedLobby = false;
    void Start()
    {
        PhotonNetwork.playerName = CreateRandomUsername();
        PhotonNetwork.ConnectUsingSettings("v1.0");
    }

    public override void OnJoinedRoom()
    {
        //SceneManager.LoadScene("Blocking Nivel");
        PhotonNetwork.LoadLevel("Blocking Nivel Pruebas Terrenos");
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
        joinedLobby = true;
    }

    public void JoinPrivateRoom()
    {
        if (!joinedLobby) return;
        RoomOptions ops;

        ops = new RoomOptions();

        ops.MaxPlayers = (byte)4;
        ops.IsVisible = false;
        ops.IsOpen = false;

        PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.playerName, ops, TypedLobby.Default);
    }

    public void JoinPublicRoom()
    {
        if (!joinedLobby) return;

        RoomOptions ops;

        ops = new RoomOptions();

        ops.MaxPlayers = (byte)20;
        ops.IsVisible = true;
        ops.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Pruebas", ops, TypedLobby.Default);
    }

    public string CreateRandomUsername()
    {
        string characters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
        string returnValue = "";
        for (int i = 0; i < 8; i++)
        {
            returnValue += characters[Random.Range((int)0, characters.Length)];
        }
        return returnValue;
    }

}
