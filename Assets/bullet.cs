using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
    public float lifetime = 5f;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        Destroy(gameObject);
        //if (coll.gameObject.tag == "Enemy")
        //    coll.gameObject.SendMessage("ApplyDamage", 10);

    }
}
