using UnityEngine;
using System.Collections;

public class footer : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
            LevelManager.RestartLevel();

        var node = coll.gameObject.GetComponent<Node>();
        if (node != null)
            node.selfDestroy();
        else
            Destroy(coll.gameObject);


    }
}
