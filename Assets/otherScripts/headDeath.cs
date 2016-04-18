using UnityEngine;
using System.Collections;

public class headDeath : MonoBehaviour {
    public Transform allMob;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag != "bullet" && coll.gameObject.tag != "Player" && coll.gameObject.tag != "trigger")
            Destroy(allMob.gameObject);


    }
}
