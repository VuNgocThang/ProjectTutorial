using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float heightJump = 3f;
    [SerializeField] private float lengthJump = 3f;
    private Rigidbody2D rb;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    [SerializeField] private bool facingLeft = true;
    public int availableJumps;
    public int totalJumps;
    private bool isGrounded;
    private bool multiJump;


    private void Awake()
    {
        availableJumps = totalJumps;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        /* if (anim.GetBool("Jumping"))
         {
             if (rb.velocity.y < 0.1)
             {
                 anim.SetBool("Falling", true);
                 anim.SetBool("Jumping", false);
             }
         }
         if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
         {

             anim.SetBool("Falling", false);
             anim.SetBool("Jumping", false);
         }*/
        MoveFog();
        if (rb.velocity.y > 0.1f)
        {
            anim.Play("frog_jump");
            
        }
        else if (rb.velocity.y < -0.1f)
        {
            anim.Play("frog_fall");

        }
        else
        {
            anim.Play("frog_idle");
        }
    }
    private void FixedUpdate()
    {
        CheckGround();
    }

    private void MoveFog()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (availableJumps > 0)
                {
                    multiJump = true;
                    availableJumps--;
                    rb.velocity = new Vector2(-lengthJump, heightJump);
                    // anim.SetBool("Jumping", true);
                }               
                /*else
                {
                    if (multiJump && availableJumps > 0)
                    {
                        availableJumps--;
                        rb.velocity = new Vector2(-lengthJump, heightJump);
                        anim.SetBool("Jumping", true);

                    }
                }*/
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
                if (availableJumps > 0)
                {
                    multiJump = true;
                    availableJumps--;
                    rb.velocity = new Vector2(lengthJump, heightJump);
                    // anim.SetBool("Jumping", true);

                }                
                /* else
                 {
                     if (multiJump && availableJumps > 0)
                     {
                         availableJumps--;
                         rb.velocity = new Vector2(lengthJump, heightJump);
                         anim.SetBool("Jumping", true);

                     }
                 }*/
            }
            else
            {
                facingLeft = true;
            }
        }
    }
    void CheckGround()
    {
        if (coll.IsTouchingLayers(ground))
        {
            isGrounded = true;
            availableJumps = totalJumps;
        }
        else
        {
            isGrounded = false;
        }
    }



    /* void Jump(int dir)
     {
         if (availableJumps > 0)
         {
             multiJump = true;
             availableJumps--;
             rb.velocity = new Vector2(lengthJump * dir, heightJump);
             anim.SetBool("Jumping", true);
         }
     }*/
}
