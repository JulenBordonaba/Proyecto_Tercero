using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using UnityEngine.EventSystems;

public class Connect : Photon.PunBehaviour
{
    
    // Start is called before the first frame update
    private static bool joinedLobby = false;
    public GameObject conectingScreen;
    public GameObject mainMenu;
    public GameObject firstButton;
    public GameObject loadingCanvas;

    private EventSystem evt;
    private string nextScene = "";

    RoomOptions roomOptions;

    private void Awake()
    {
        string nickname = LoadName();

        PhotonNetwork.playerName = nickname;
        
        PhotonNetwork.AuthValues = new AuthenticationValues(nickname);

        print("ID AuthValues: " + PhotonNetwork.AuthValues.UserId);
        
        
    }

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

    string LoadName()
    {
        string nickname = "";
        if(PlayerPrefs.HasKey("Nickname"))
        {
            nickname = PlayerPrefs.GetString("Nickname");
        }
        else
        {
            nickname = CreateRandomNickname();
            PlayerPrefs.SetString("Nickname", nickname);
        }
        return nickname;
    }

    public override void OnJoinedRoom()
    {
        //SceneManager.LoadScene("Blocking Nivel");
        loadingCanvas.SetActive(true);
        PhotonNetwork.LoadLevel(nextScene);
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

    public void JoinPrivateRoom(string _nextScene)
    {
        if (!joinedLobby) return;
        RoomOptions ops;

        nextScene = _nextScene;

        ops = new RoomOptions();

        Global.onePlayer = true;

        ops.MaxPlayers = (byte)4;
        ops.IsVisible = false;
        ops.IsOpen = false;

        List<string> teamIDs = new List<string>(SocialManager.friends);
        teamIDs.Add(PhotonNetwork.player.UserId);

        PhotonNetwork.CreateRoom(null, ops, TypedLobby.Default, teamIDs.ToArray());
    }

    public void JoinPublicRoom(string _nextScene)
    {
        if (!joinedLobby) return;

        nextScene = _nextScene;

        roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = (byte)20;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        if(!PhotonNetwork.JoinRandomRoom())
        {
            print("crea sala");
            
        }

        //PhotonNetwork.JoinOrCreateRoom("Pruebas", ops, TypedLobby.Default);
    }

    public void JoinPruebasRoom(string _nextScene)
    {
        if (!joinedLobby) return;

        nextScene = _nextScene;

        Global.onePlayer = true;

        roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = (byte)20;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        if (!PhotonNetwork.JoinRandomRoom())
        {
            print("crea sala");

        }

        //PhotonNetwork.JoinOrCreateRoom("Pruebas", ops, TypedLobby.Default);
    }




    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonCreateRoomFailed(codeAndMsg);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }

    public string CreateRandomNickname()
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
