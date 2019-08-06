﻿using UnityEngine;
using System.Collections;
using ExtensionsMethods;

public abstract class aShot : MonoBehaviour {
    public int damage = 0;
    public float speed = 1;
    public float lifetime = 5f;

    protected Transform _target;
    protected Vector3 _targetPositionOnShoot;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public virtual void SetTarget(Transform player)
    {
        _target = player;
        _targetPositionOnShoot = _target.transform.position;

        transform.LookAt2D(Vector2.up, _target.transform);
    }

    public virtual void SetTarget(Vector3 target)
    {
        _targetPositionOnShoot = target;
        transform.LookAt2D(Vector2.up, target);
    }

	void FixedUpdate () {
        Move();
	}

    public virtual void Move() { }

}
