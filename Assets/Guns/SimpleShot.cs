using UnityEngine;
using System.Collections;

public class SimpleShot : aShot{
    public bool isMystic = false;

    public override void Move()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
            return;

        if (other.gameObject.tag == "Player")
            LevelManager.RestartLevel();
        else
            if (!isMystic)
                Destroy(gameObject);


    }
}
