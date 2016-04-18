using UnityEngine;
using System.Collections;

public class ConstantAnimation : MonoBehaviour {
    public Vector3 moveVector;
    public float Torgue;
    public float random = 0;

    void Start()
    {
        moveVector += new Vector3(Random.Range(0, random), Random.Range(0, random), Random.Range(0, random));
    }

	// Update is called once per frame
	void Update () {

        var delta = Time.deltaTime;
        transform.Translate(moveVector * delta);
        transform.Rotate(Vector3.forward, Torgue * delta, Space.World);
	}
}
