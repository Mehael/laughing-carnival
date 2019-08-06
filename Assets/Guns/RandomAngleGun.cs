using UnityEngine;
using System.Collections;
using ExtensionsMethods;
public class RandomAngleGun : aGun {
    float _counter = 0;
    float _counterSpeed = 2;
    float _counterLimit = 2000;

    void Update()
    {
        _counter += _counterSpeed * Time.deltaTime;
        if (_counter > _counterLimit)
            _counter = 0;
    }

    public override void Shoot()
    {
        if (target!= null)
            (Instantiate(shot, transform.position, Quaternion.identity) as GameObject)
                .transform.LookAt2D(new Vector2(Mathf.Cos(_counter),Mathf.Sin(_counter))*30, target.position);
    }
}
