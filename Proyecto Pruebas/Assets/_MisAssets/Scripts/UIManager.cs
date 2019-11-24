using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform checkpointImage;
    public RectTransform checkpointArrows;
    public float checkpointDamping = 5;

    [Header("Checkpoint limits")]
    public float leftLimit = 50f;
    public float rightLimit = 50f;
    public float upLimit = 50f;
    public float downLimit = 50f;

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
        OrientateCheckpointArrows();
    }

    private void ShowNewestCheckpoint()
    {
        //print(CheckpointScreenPosition);
        Vector3 auxPos;
        auxPos = new Vector3(Mathf.Clamp(CheckpointScreenPosition.x - (Screen.width * 0.5f), (-Screen.width * 0.5f) + leftLimit, (Screen.width * 0.5f) - rightLimit), Mathf.Clamp(CheckpointScreenPosition.y - (Screen.height * ((myCamera.rect.height * 0.5f) + myCamera.rect.y)), -(Screen.height * myCamera.rect.height * 0.5f) + downLimit, (Screen.height * myCamera.rect.height * 0.5f) - upLimit), 0);
        if (CheckpointScreenPosition.z < 0)
        {
            Vector3 pos = auxPos;



            if (pos.x > -(Screen.width * 0.5f) || pos.x < (Screen.width * 0.5f))
            {
                auxPos = new Vector3(-pos.x, (Screen.height * myCamera.rect.height * 0.5f) - upLimit, 0);
            }
            else
            {
                auxPos = new Vector3(pos.x, -pos.y, 0);
            }


        }
        checkpointImage.localPosition = Vector3.Lerp(checkpointImage.localPosition, auxPos, Time.deltaTime * checkpointDamping);

    }

    private void OrientateCheckpointArrows()
    {
        //comprobar si el checkpoint esta delante de la cámara
        if (CheckpointScreenPosition.z > 0)
        {
            Vector2 auxPos = new Vector2(CheckpointScreenPosition.x - (Screen.width * 0.5f), CheckpointScreenPosition.y - (Screen.height * ((myCamera.rect.height * 0.5f) + myCamera.rect.y)));
            //comprobar si esta dentro del intervalo horizontal
            if (auxPos.x > -(Screen.width * 0.5f) && auxPos.x < (Screen.width * 0.5f))
            {
                //comprobar si esta dentro del intervalo vertical
                if (auxPos.y < (Screen.height * myCamera.rect.height * 0.5f) && auxPos.y > -(Screen.height * myCamera.rect.height * 0.5f))
                {
                    checkpointArrows.gameObject.SetActive(false);
                }
                else
                {
                    checkpointArrows.gameObject.SetActive(true);
                }
            }
            else
            {
                checkpointArrows.gameObject.SetActive(true);
            }
        }
        else
        {
            checkpointArrows.gameObject.SetActive(true);
        }
        checkpointArrows.localRotation = Quaternion.Euler(0, 0, GetAngle(Vector2.zero, new Vector2(checkpointImage.localPosition.x, checkpointImage.localPosition.y)));
    }

    private Vector3 CheckpointScreenPosition
    {
        get { return myCamera.WorldToScreenPoint(CheckpointManager.newest.transform.position); }
    }

    public Vector2 ScreenMiddle
    {
        get { return new Vector2(Screen.width * 0.5f, Screen.height * ((myCamera.rect.height * 0.5f) + myCamera.rect.y)); }
    }

    public float GetAngle(Vector2 start, Vector2 end)
    {
        return Mathf.Atan2(end.y - start.y, end.x - start.x) * 180 / Mathf.PI;
    }

}
