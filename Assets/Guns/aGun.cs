using UnityEngine;
using System.Collections;

public class aGun : MonoBehaviour {
    public Transform target;
    public float reloadTime = 1.0f;
    public GameObject shot;

    private float _pastTime;

    void Start()
    {
        _pastTime = reloadTime;
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate () {
        if (isReloaded())
            Shoot();
	}

    public virtual bool isReloaded()
    {
        _pastTime += Time.deltaTime;
        if (_pastTime<reloadTime)
            return false;

        _pastTime = 0;
        return true;
    }

    public virtual void Shoot()
    {
        (Instantiate(shot, transform.position, Quaternion.identity) as GameObject)
            .SendMessage("SetTarget", target);
    }
}
