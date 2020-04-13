using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using UnityEngine.EventSystems;

public class Connect : Photon.PunBehaviour
{
    
    // Start is called before the first frame update
    private bool joinedLobby = false;
    public GameObject conectingScreen;
    public GameObject mainMenu;
    public GameObject firstButton;

    private EventSystem evt;


    void Start()
    {
        evt = EventSystem.current;
        if(!PhotonNetwork.connected)
        {
            joinedLobby = false;
            mainMenu.SetActive(false);
            conectingScreen.SetActive(true);
        }
        else
        {
            EnableMenu();
        }
        RegisterSerializableTypes();
        PhotonNetwork.playerName = CreateRandomUsername();
        PhotonNetwork.ConnectUsingSettings("v1.0");

        
    }

    private void RegisterSerializableTypes()
    {
        PhotonPeer.RegisterType(typeof(Stats), (byte)1, Stats.Serialize, Stats.Deserialize);
        PhotonPeer.RegisterType(typeof(DamageManager), (byte)2, DamageManager.Serialize, DamageManager.Deserialize);
        PhotonPeer.RegisterType(typeof(Combustible), (byte)3, Combustible.Serialize, Combustible.Deserialize);
        PhotonPeer.RegisterType(typeof(Checkpoint), (byte)4, Checkpoint.Serialize, Checkpoint.Deserialize);
        PhotonPeer.RegisterType(typeof(SynchronizableGameObject), (byte)5, SynchronizableGameObject.Serialize, SynchronizableGameObject.Deserialize);
        PhotonPeer.RegisterType(typeof(EffectData), (byte)6, EffectData.Serialize, EffectData.Deserialize);
        PhotonPeer.RegisterType(typeof(TrapperHarpon), (byte)7, TrapperHarpon.Serialize, TrapperHarpon.Deserialize);
        



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
        EnableMenu();
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

    void EnableMenu()
    {
        joinedLobby = true;
        mainMenu.SetActive(true);
        conectingScreen.SetActive(false);
        evt.SetSelectedGameObject(firstButton);
    }

}
