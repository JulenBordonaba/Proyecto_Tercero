using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image checkpointImage;
    public float checkpointDamping = 5;
    private Camera myCamera;


    // Start is called before the first frame update
    void Start()
    {
        myCamera = transform.parent.gameObject.GetComponentInChildren<CameraController>().gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowNewestCheckpoint();
    }

    private void ShowNewestCheckpoint()
    {
        print(CheckpointScreenPosition);
        Vector3 auxPos;
        auxPos = new Vector3(Mathf.Clamp(CheckpointScreenPosition.x - (Screen.width * 0.5f), -Screen.width * 0.5f, Screen.width * 0.5f), Mathf.Clamp(CheckpointScreenPosition.y - (Screen.height * ((myCamera.rect.height * 0.5f) + myCamera.rect.y)), -(Screen.height * myCamera.rect.height * 0.5f), (Screen.height * myCamera.rect.height * 0.5f)), 0);
        if (CheckpointScreenPosition.z < 0)
        {
            Vector3 pos = auxPos;



            if (pos.x > -(Screen.width * 0.5f) || pos.x < (Screen.width * 0.5f))
            {
                auxPos = new Vector3(pos.x, (Screen.height * myCamera.rect.height * 0.5f), 0);
            }
            else
            {
                auxPos = new Vector3(pos.x, -pos.y, 0);
            }
        }
        checkpointImage.GetComponent<RectTransform>().localPosition = Vector3.Lerp(checkpointImage.GetComponent<RectTransform>().localPosition, auxPos, Time.deltaTime*checkpointDamping);
    }

    private Vector3 CheckpointScreenPosition
    {
        get { return myCamera.WorldToScreenPoint(CheckpointManager.newest.transform.position); }
    }

}
