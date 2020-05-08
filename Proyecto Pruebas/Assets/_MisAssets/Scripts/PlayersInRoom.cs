using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayersInRoom : Photon.PunBehaviour
{

    private TextMeshProUGUI playersText;

    // Start is called before the first frame update
    void Start()
    {
        playersText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playersText.text = GameManager.navesList.Count + "/20";
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        playersText.text = GameManager.navesList.Count + "/20";
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        playersText.text = GameManager.navesList.Count + "/20";
    }

}
