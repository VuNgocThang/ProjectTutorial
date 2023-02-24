using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog2 : Enemy
{
    [SerializeField] private float leftCap = 40;
    [SerializeField] private float rightCap = 60;
    [SerializeField] private float heightJump = 4f;
    [SerializeField] private float lengthJump = 3f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private bool facingLeft = true;
    public int availableJump;
    public int totalJumps = 2;
    [SerializeField] private bool isGrounded;
    private Collider2D coll;   

    public enum State
    {
        jump,
        fall,
        idle
    }
    public State state = State.idle;

    private void Awake()
    {
        availableJump = totalJumps;
    }

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        /*if (availableJump > 0)
        {
            MoveAndFlip();
        }*/
        setAnim();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }



    private void MoveAndFlip()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (state == State.idle && availableJump > 0)
                {
                    
                    jump(-1);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);

                }
                if (state == State.idle && availableJump > 0)
                {
                    
                    jump(1);
                }
            } 
            else
            {
                facingLeft = true;
            }
        }
    }

    void jump(int hor)
    {       
        rb.velocity = new Vector2(hor * lengthJump, heightJump);
        anim.Play("frog_jump");
        state = State.jump;
        availableJump--;
    }
    private void setAnim()
    {
        if (state == State.jump)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.fall;
                anim.Play("frog_fall");
            }
        }
        else if (state == State.fall)
        {
            if (availableJump == totalJumps)
            {
                state = State.idle;
                anim.Play("frog_idle");
            }
        }  
       
    }
    void CheckGround()
    {
        if (coll.IsTouchingLayers(ground))
        {
            isGrounded = true;
            StartCoroutine(ExampleCoroutine());
            availableJump = totalJumps;

        }
        else
        {
            isGrounded = false;
        }
    }
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(1f);
        MoveAndFlip();
    }

}
