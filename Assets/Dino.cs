using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : Enemy
{
    [SerializeField] private float maxHeightJump = 6;
    [SerializeField] private float forceJump = 4f;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    float degreesPerSecond = 30;
    [SerializeField] private bool isGrounded = true;
    float z = 180;
    

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (coll.IsTouchingLayers(ground))
        {
            Jump();
        }

        if (transform.position.y > maxHeightJump)
        {
            Rotate();

        }
        setAnim();


    }

    void setAnim()
    {

        if (rb.velocity.y > 0.1f)
        {
            anim.Play("dino_jump");
        }
        if (transform.position.y > maxHeightJump)
        {
            anim.Play("rotate");
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * forceJump, ForceMode2D.Impulse);
    }
    void Rotate()
    {
        rb.velocity = Vector3.zero;
        /*Vector3 rotationVector = new Vector3(0, 0, z);
        transform.rotation = Quaternion.Euler(rotationVector);*/
    }



}
