using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextPopUp : Photon.PunBehaviour
{

    private Text text;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage(string message, Color color)
    {
        Vector3 _color = new Vector3(color.r, color.g, color.b);

        photonView.RPC("ShowMessageRPC", PhotonTargets.All, message, _color);


    }
    public void ShowMessage(string message)
    {
        text.text = message;

        Vector3 _color = new Vector3(text.color.r, text.color.g, text.color.b);

        photonView.RPC("ShowMessageRPC", PhotonTargets.All, message, _color);

    }

    [PunRPC]
    public void ShowMessageRPC(string message, Vector3 color )
    {
        text.text = message;

        text.color = new Color(color.x, color.y, color.z);

        anim.SetTrigger("Message");
    }

}
