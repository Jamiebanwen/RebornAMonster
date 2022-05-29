using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpforce = 1f;
    [SerializeField] float climbspeed = 5f;
    [SerializeField] float dashSpeed = 3f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] int jumpsAvailableCount = 2;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip runSFX;
    Animator myAnimator;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D playerFeetCollider;
    SpriteRenderer mySpriteRenderer;
    float playerGravityAtStart;
    bool isAlive = true;
    public bool canDash = false;
    public bool canDoubleJump = false;
    bool isDashing = false;


    // Start is called before the first frame update
    void Start()
    {
      myRigidbody = GetComponent<Rigidbody2D>();
      myAnimator = GetComponent<Animator>();  
      myCapsuleCollider = GetComponent<CapsuleCollider2D>();
      playerFeetCollider = GetComponent<BoxCollider2D>();  
      playerGravityAtStart = myRigidbody.gravityScale;
      mySpriteRenderer = GetComponent<SpriteRenderer>();
      AbilityCheck();
    }

    public void AbilityCheck()
    {
        int currentlevel = SceneManager.GetActiveScene().buildIndex;
        if(currentlevel == 1)
        {
            canDash = false;
            canDoubleJump = false;
        }
        else if(currentlevel == 2)
        {
            canDoubleJump = true;
        }
        else if(currentlevel > 2)
        {
            canDash = true;
            canDoubleJump = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive){ return; }
        
        Run();
        FlipSprite();
        ClimbLadder();
        hitEnemy();
    
    }
    public void ResetJumpCount()
    {
        // if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        // {
        //     //Debug.Log("Check");
        if(canDoubleJump)
        {
            jumpsAvailableCount = 2;
        }
        else
        {
            jumpsAvailableCount = 1;
        }
        
        // }
        
        // else
        // {
        //     jumpsAvailableCount = 0;
        // }
    }
    
    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x*moveSpeed,myRigidbody.velocity.y);
        myRigidbody.velocity=playerVelocity;

        bool PlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) >Mathf.Epsilon;
        myAnimator.SetBool("IsRunning",PlayerHasHorizontalSpeed);
        // if(PlayerHasHorizontalSpeed)
        // {
        //  InvokeRepeating("PlayWalking", 00.1f, 0.3f);
        // }
    }

    // void PlayWalking()
    // {
    //     AudioSource.PlayClipAtPoint(runSFX,Camera.main.transform.position);
    // }

    void OnMove(InputValue value)
    {
        if(!isAlive){ return; }
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {   
        if(!isAlive){ return; }
 
        // myAnimator.SetBool("IsJumping",true);
        

        // if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        // {
        //     // Debug.Log(isInAir);
            
        //     return;
        // }
        if(value.isPressed && jumpsAvailableCount > 0)
        {
            myRigidbody.velocity += new Vector2(0f,jumpforce);
            AudioSource.PlayClipAtPoint(jumpSFX,Camera.main.transform.position);
            jumpsAvailableCount --;
            bool PlayerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) >Mathf.Epsilon;
            //Debug.Log(PlayerHasVerticalSpeed);
            myAnimator.SetBool("IsJumping",PlayerHasVerticalSpeed);
            // myAnimator.SetTrigger("DoubleJump");
        }
        // if(value.isPressed && jumpsAvailableCount ==0 && canDoubleJump && isJumping)
        // {
        //     myAnimator.SetBool("DubJump",true);
        // }
    }


    private void OnCollisionEnter2D(Collision2D other) 
    {
        //Debug.Log("test jump off");
        myAnimator.SetBool("IsJumping",false);
        myAnimator.SetBool("IsDashing",false);
        myAnimator.SetBool("DubJump",false);
        ResetJumpCount();
        // bool PlayerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) >Mathf.Epsilon;
        // Debug.Log(PlayerHasVerticalSpeed);
        // myAnimator.SetBool("IsJumping",PlayerHasVerticalSpeed);
    }

    void ClimbLadder()
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
            {
            //myAnimator.SetBool("IsCliming",false);
            myRigidbody.gravityScale = playerGravityAtStart;    
            return;
            }
        
            Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x,moveInput.y*climbspeed);
            myRigidbody.velocity=climbVelocity;       
            // myRigidbody.gravityScale = 0f;

            bool PlayerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) >Mathf.Epsilon;
            //myAnimator.SetBool("IsCliming",PlayerHasVerticalSpeed);
    }

    void FlipSprite()
    {
        bool PlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) >Mathf.Epsilon;
        if(PlayerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x),1f);
        }
    }

    void OnFire(InputValue value)
    {
        if(!isAlive){ return; }
        Instantiate(bullet, bulletSpawn.position, transform.rotation);
    }

    void hitEnemy()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            myRigidbody.velocity += new Vector2(2f,10f);
            //myAnimator.SetTrigger("Damage");
            mySpriteRenderer.color = new Color(255f,122f,122f,255f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            isAlive = false;
            myRigidbody.velocity += new Vector2(2f,20f);
            myAnimator.SetTrigger("Damage");
            mySpriteRenderer.color = new Color(255f,122f,122f,255f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
 
    void OnDash(InputValue value)
    {
        if(!isDashing && canDash)
        {
            StartCoroutine(Dash());
        }

    }
    IEnumerator Dash()
    {
        float baseSpeed = moveSpeed;
        isDashing=true;
        myAnimator.SetBool("IsDashing",isDashing);
        //audiosource.PlayOneShot(DASHSFX)
        moveSpeed *= dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = baseSpeed;
        isDashing = false;
        myAnimator.SetBool("IsDashing",isDashing);
    }
}
