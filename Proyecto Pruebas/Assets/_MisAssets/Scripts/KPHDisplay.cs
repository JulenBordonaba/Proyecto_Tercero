using UnityEngine;
using UnityEngine.UI;

public class KPHDisplay : MonoBehaviour
{
    public Rigidbody rb;
    public Text display_Text;

    public void Update()
    {
        display_Text.text = (int)rb.velocity.magnitude + " KPH";
    }
}