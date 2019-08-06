using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rBody;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>(); 
        rBody = GetComponent<Rigidbody2D>();
    }

    public float movSpeed = 1;
    public float jmpSpeed = 1;
    public float maxVelocity = 0.5f;
    public Transform groundCheck;
    void FixedUpdate()
    {
        var movement = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            movement += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            movement += Vector3.right;

        var velY = rBody.velocity.y;
        var velX = rBody.velocity.x;
        rBody.AddForce(Time.deltaTime * movSpeed * movement);

        if (Mathf.Abs(rBody.velocity.x) > maxVelocity)
            rBody.velocity = new Vector2(Mathf.Sign(velX)*maxVelocity,
                rBody.velocity.y);
        
        animator.SetBool("isMoving", Mathf.Abs(velX) > 0.1f);
        animator.SetBool("isGrounded", Mathf.Abs(velY) < 0.1f);
        
        if (Input.GetKeyDown(KeyCode.W) && velY < 0.1f &&
            Physics2D.OverlapPoint(groundCheck.position) != null)
            rBody.AddForce(Time.deltaTime * jmpSpeed * Vector2.up, ForceMode2D.Impulse);
    }
}
