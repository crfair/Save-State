using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpLimit;
    [SerializeField] float jumpVelocityClamp;

    Vector3 respawnPoint;
    int lives = 1;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    //Input Handling
    bool holdingD;
    bool holdingA;
    bool holdingJump;
    bool releaseJump;
    bool grounded;
    bool hasDoubleJump;

    float jumpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        respawnPoint = gameObject.transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Input Handling
        if (Input.GetKey("d"))
        {
            holdingD = true;
        }
        else
        {
            holdingD = false;
        }

        if (Input.GetKey("a"))
        {
            holdingA = true;
        }
        else
        {
            holdingA = false;
        }

        if (Input.GetKeyDown("space"))
        {
            holdingJump = true;
        }

        if (Input.GetKeyUp("space"))
        {
            holdingJump = false;
        }

        #endregion

        #region Animation Handling
        if (holdingA || holdingD) 
        {
            anim.SetBool("Walking", true);
        }
        else 
        {
            anim.SetBool("Walking", false);
        }

        if (rb.velocity.y > 0)
        {
            anim.SetInteger("Y Velocity", 1);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetInteger("Y Velocity", -1);
        }
        else 
        {
            anim.SetInteger("Y Velocity", 0);
        }
        #endregion
    }

    void FixedUpdate()
    {
        if (holdingD)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            sr.flipX = false;
        }

        if (holdingA)
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
            sr.flipX = true;
        }


        if (holdingJump && (grounded || hasDoubleJump))
        {
            rb.velocity = new Vector2(0, jumpSpeed * Time.deltaTime);
            jumpTimer += Time.deltaTime;
        }

        if (jumpTimer >= jumpLimit || (holdingJump == false && jumpTimer != 0))
        {
            rb.velocity = new Vector2(0, jumpVelocityClamp);
            jumpTimer = 0f;
            holdingJump = false;
            if (!grounded)
            {
                hasDoubleJump = false;
            }
            grounded = false;
        }
   
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform") && col.GetContact(0).normal.y > 0)
        {
            grounded = true;
            hasDoubleJump = true;
        }

        if(col.gameObject.CompareTag("Hazard"))
        {
            gameObject.transform.position = respawnPoint;
        }
    }
    public int GetLives()
    {
        return lives;
    }

    public void AddLives(int life)
    {
        lives += life;
    }

    public bool HasDoubleJump()
    {
        return hasDoubleJump;
    }

    public void SetDoubleJump(bool doubleJump)
    {
        hasDoubleJump = doubleJump;
    }
}
