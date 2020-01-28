using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersInRoom : Photon.PunBehaviour
{

    private Text playersText;

    // Start is called before the first frame update
    void Start()
    {
        playersText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        playersText.text = PhotonNetwork.room.PlayerCount + "/20";
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        playersText.text = PhotonNetwork.room.PlayerCount + "/20";
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        playersText.text = PhotonNetwork.room.PlayerCount + "/20";
    }

}
