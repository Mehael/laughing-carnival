using UnityEngine;
using System.Collections;

public class linemonsrer : MonoBehaviour
{
    public Transform bullet;
    public Transform target;
    public bool isJumpable = false;
    public float jumpPause = 1f;
    public float jumpForce = 100f;
    public bool isDANGERIOUS = false;

    public float speed = 1f;

    void Start()
    {
        if (isJumpable)
            InvokeRepeating("Jump", Random.Range(0f,3f), jumpPause);
    }

    void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            transform.Translate((target.position - transform.position).normalized * Time.deltaTime * speed);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!isDANGERIOUS)
            return;

        if (coll.gameObject.tag == "Player")
            LevelManager.RestartLevel();
    }
}
