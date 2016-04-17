using UnityEngine;
using System.Collections;

public class LampTrigger : MonoBehaviour {
    public Transform message;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            message.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            message.gameObject.SetActive(false);
        }
    }

}
