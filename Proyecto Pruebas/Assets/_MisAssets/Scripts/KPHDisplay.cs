using UnityEngine;
using UnityEngine.UI;

public class KPHDisplay : MonoBehaviour
{
    public Rigidbody rb;
    public Text display_Text;
    public Transform modelTransform;

    public void Update()
    {
        return;
        Vector3 locVel = modelTransform.InverseTransformDirection(rb.velocity);
        display_Text.text = (int)new Vector3(locVel.x, 0, locVel.z).magnitude + " KPH";
    }
}