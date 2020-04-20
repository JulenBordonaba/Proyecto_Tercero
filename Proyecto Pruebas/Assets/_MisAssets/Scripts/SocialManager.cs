using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SocialManager : Photon.PunBehaviour
{
    public GameObject socialCanvas;

    public GameObject socialFirstUIElement;
    public GameObject mainMenuFirstUIElement;

    public TMP_InputField friendInput;

    public static List<string> friends = new List<string>();



    private EventSystem evt;

    // Start is called before the first frame update
    void Start()
    {
        evt = EventSystem.current;

    }

    // Update is called once per frame
    void Update()
    {
        ChangeMenu();
    }

    void ChangeMenu()
    {
        if(Input.GetKeyDown(KeyCode.Joystick1Button13) || Input.GetKeyDown(KeyCode.Tab))
        {
            if(socialCanvas.activeInHierarchy)
            {
                socialCanvas.SetActive(false);
                evt.SetSelectedGameObject(mainMenuFirstUIElement);
            }
            else
            {
                socialCanvas.SetActive(true);
                evt.SetSelectedGameObject(socialFirstUIElement);
            }
        }
    }

    public void InviteFriend()
    {
        PhotonNetwork.FindFriends(new string[1] { friendInput.text });
    }


    public override void OnUpdatedFriendList()
    {
        friends.Clear();
        for (int i = 0; i < PhotonNetwork.Friends.Count; i++)
        {
            FriendInfo friend = PhotonNetwork.Friends[i];

            friends.Add(friend.UserId);

            Debug.LogFormat("Id: {0} / is online: {1} / is in room: {2}", friend.UserId, friend.IsOnline, friend.IsInRoom);
        }
    }
}
