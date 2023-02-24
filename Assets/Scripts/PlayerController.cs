using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour


{
    
    //public Animator anim;
    public enum State
    {
        Running,
        JumpUp,
        JumpFall,
        Idle,
        Hurt,
        Climb
    }
    public State state = State.Idle;
    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool bottomLadders = false;
    [HideInInspector] public bool topLadders = false;
    public ladders ladders;
    private float naturalGravityScale;
    [SerializeField] float climbSpeed = 3f;
    public Rigidbody2D rb;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    public Animator anim;
    [SerializeField] private float jump = 15f;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private string name;

    public int availableJump;
    public int totalJumps = 3;
    [SerializeField] private bool isGrounded = false;

    private void Awake()
    {
        availableJump = totalJumps;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        naturalGravityScale = rb.gravityScale;
        // PermanentUI.perm.heartsText.text = PermanentUI.perm.hearts.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (state == State.Climb)
        {
            Climb();
        }
        else if (state != State.Hurt)
        {
            Movement();
        }
        //  SetAnimation();
        VelocityState();
        //anim.SetInteger("state", (int)state);
    }
    private void FixedUpdate()
    {
        CheckGround();  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries += 1;
            PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
        }
        if (collision.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
            jump = 15f;
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(ResetPower());

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.JumpFall)
            {
                enemy.JumpedOn();
                JumpUp();
            }
            else
            {
                anim.Play("hurt");
                state = State.Hurt;

                HeartsHandle();
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }


        }
    }

    public void HeartsHandle()
    {
        PermanentUI.perm.hearts -= 1;
        PermanentUI.perm.heartsText.text = PermanentUI.perm.hearts.ToString();
        if (PermanentUI.perm.hearts <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void Movement()
    {
        if (canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            state = State.Climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladders.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }

        if (Input.GetKeyDown(KeyCode.Space) && availableJump > 0)
        {
            availableJump--;
            JumpUp();
            anim.Play("jump_up");
        }
    }
      /*  if (Input.GetKeyDown(KeyCode.Space) )
        {
            rb.velocity = new Vector2(rb.velocity.x, forceJump);
            state = State.JumpUp;
            anim.Play("jump_up");
        }*/
    /* void SetAnimation()
     {

         if (rb.velocity.magnitude < 0.1f)
         {
             anim.Play("idle");
             state = State.Idle;
         }
         else if (rb.velocity.y > 0.1f)
         {
             anim.Play("jump_up");
             state = State.JumpUp;
         }
         else if (rb.velocity.y < -0.1f)
         {
             anim.Play("jump_fall");
             state = State.JumpFall;
         }
         else if (rb.velocity.x != 0 && rb.velocity.y <= 0.1f)
         {
             anim.Play("run");
             state = State.Running;

         }
         *//* if (Mathf.Abs(rb.velocity.x) < 0.1f)
          {
              if (state == State.Hurt)
              {
                  state = State.Idle;
              }
          }*//*

     }*/

    private void JumpUp()
    {
        
        rb.velocity = new Vector2(rb.velocity.x, jump);
        state = State.JumpUp;
    }
    private void VelocityState()
    {
        if (state == State.Climb)
        {
            Climb();
            anim.Play("climb");
        }
        else if (state == State.JumpUp)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.JumpFall;
                anim.Play("jump_fall");
            }
        }
        else if (state == State.JumpFall)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.Idle;
                anim.Play("idle");
                availableJump = totalJumps;

            }
        }
        else if (state == State.Hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.Idle;
            }
        }
        else if (rb.velocity.x != 0 && rb.velocity.y <= 0.1f)
        {
            state = State.Running;
            anim.Play("run");
        }
        else
        {
            state = State.Idle;
            anim.Play("idle");
        }
    }
    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jump = 10f;
        GetComponent<SpriteRenderer>().color = Color.white;

    }

    private void Climb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = naturalGravityScale;
            anim.speed = 1f;
            rb.velocity = new Vector2(rb.velocity.x, jump);
            state = State.JumpUp;
            
        }
        float vDirection = Input.GetAxis("Vertical");
        if (vDirection > .1f && !topLadders)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        else if (vDirection < -.1f && !bottomLadders)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;

        }
        else { 
            anim.speed = 0f;
            rb.velocity = Vector2.zero;
        }

    }
    void CheckGround()
    {
        if (coll.IsTouchingLayers(ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }


}
