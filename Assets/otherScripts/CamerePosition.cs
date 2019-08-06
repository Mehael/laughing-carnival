using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerePosition : MonoBehaviour
{
    public Transform Player;
    public Transform Target;
    private float CameraZPosition;
    public Collider2D CameraBounds;
    private Vector3 CameraMinShift;
    private Vector3 CameraMaxShift;
    public Vector3 camShift = Vector3.zero;
    
    private void Awake()
    {
        CameraZPosition = transform.position.z;
        lastTarget = transform.position;
    }

    private Vector3 worldMinBound;
    private Vector3 worldMaxBound;
    
    private void Start()
    {
        var minShift = Camera.main.ScreenToWorldPoint(Vector3.zero) - transform.position;
        var maxShift = Camera.main.ScreenToWorldPoint(
                           new Vector3(Screen.width, Screen.height, 0)
                       ) - transform.position;
        

        worldMinBound = CameraBounds.bounds.min;
        worldMaxBound = CameraBounds.bounds.max;
        
        CameraMinShift =  worldMinBound - minShift;
        CameraMaxShift = worldMaxBound - maxShift;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(CameraMinShift,0.1f);
        Gizmos.DrawSphere(CameraMaxShift,0.1f);
        
        Gizmos.DrawSphere(worldMinBound,0.2f);
        Gizmos.DrawSphere(worldMaxBound,0.2f);
        Gizmos.DrawSphere(CameraBounds.transform.position,0.3f);
    }

    public float smoothTime = 1;
    private Vector3 velocity = Vector3.zero;
    private Vector3 lastTarget;
    public Vector3 target;
    void LateUpdate()
    {
        
        target = new Vector3(
            (Player.position.x + Target.position.x)/2,
            (Player.position.y + Target.position.y)/2, 
            CameraZPosition)
            + camShift;
        
        if (target.x < CameraMinShift.x ||target.x > CameraMaxShift.x)
            target.x = lastTarget.x;
        if (target.y < CameraMinShift.y ||target.y > CameraMaxShift.y)
            target.y = lastTarget.y;
        
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        lastTarget = target;
    }
}
