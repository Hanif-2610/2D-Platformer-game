using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public int fruits;

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    public float doubleJumpForce;
    public Vector2 wallJumpDirection;

    float defaultJumpForce;

    private bool canDoubleJump = true;
    private bool canMove;

    private float movingInput;

    [SerializeField] float bufferJumpTime;
    float bufferJumpCounter;

    [SerializeField] float cayoteJumpTime;
    float cayoteJumpCounter;
    bool canHaveCayoteJump;

    [Header("Knockback info")]
    [SerializeField] Vector2 knockbackDirection;
    [SerializeField] float knockbackTime;
    [SerializeField] float knockbackProtectionTime;
    bool isKnocked;
    bool canBeKnocked = true;

    [Header("Collision Info")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] Transform enemyCheck;
    [SerializeField] float enemyCheckRadius;
    private bool isGrounded;
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;


    private bool facingRight = true;
    private int facingDirection = 1;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        defaultJumpForce = jumpForce;
    }

    void Update()
    {
        AnimationControllers();

        if (isKnocked)
            return;

        FlipController();
        CollisionChecks();
        InputChecks();
        CheckForEnemy();

        bufferJumpCounter -= Time.deltaTime;
        cayoteJumpCounter -= Time.deltaTime;

        if (isGrounded)
        {
            canDoubleJump = true;
            canMove = true;

            if (bufferJumpCounter > 0)
            {
                bufferJumpCounter = -1;
                Jump();
            }
            canHaveCayoteJump = true;
        }
        else
        {
            if (canHaveCayoteJump)
            {
                canHaveCayoteJump = false;
                cayoteJumpCounter = cayoteJumpTime;
            }
        }

        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }

        Move();
    }

    private void CheckForEnemy()
    {
        Collider2D[] hitedColliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius);

        foreach (var enemy in hitedColliders)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {

                Enemy newEnemy = enemy.GetComponent<Enemy>();

                if(newEnemy.invincible)
                    return;
                
                if(rb.velocity.y < 0)
                {
                    newEnemy.Damage();
                    Jump();
                }
            }
        }
    }

    private void AnimationControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void InputChecks()
    {
        movingInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetAxis("Vertical") < 0)
            canWallSlide = false;
        
        if (Input.GetKeyDown(KeyCode.Space))
            JumpButton();
    }

    private void JumpButton()
    {
        if(!isGrounded)
            bufferJumpCounter = bufferJumpTime;

        if(isWallSliding)
        {
            WallJump();
            canDoubleJump = true;
        }
        else if (isGrounded || cayoteJumpCounter > 0)
        {
            Jump();
        }
        else if(canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            jumpForce = doubleJumpForce;
            Jump();
            jumpForce = defaultJumpForce;
        }

        canWallSlide = false;
    }

    public void Knockback(Transform damageTransform)
    {
        if(!canBeKnocked)
            return;

        fruits--;
        if(fruits < 0)
        {
            Destroy(gameObject);
        }

        GetComponent<CameraShakeFX>().ScreenShake(-facingDirection);
        isKnocked = true;
        canBeKnocked = false;

        #region Define horizontal direction for knockback
        int horizontalDirection = 0;
        if(transform.position.x > damageTransform.position.x)
            horizontalDirection = 1;
        else if(transform.position.x < damageTransform.position.x)
            horizontalDirection = -1;
        #endregion

        rb.velocity = new Vector2(knockbackDirection.x * horizontalDirection, knockbackDirection.y);
        Invoke("CancelKnockback", knockbackTime);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }

    private void CancelKnockback()
    {
        isKnocked = false;
    }

    private void AllowKnockback()
    {
        canBeKnocked = true;
    }

    private void Move()
    {
        if(canMove)
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
    }

    private void WallJump()
    {
        canMove = false;
        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Push(float pushForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, pushForce);
    }

    private void FlipController()
    {
        if(facingRight && rb.velocity.x < -.1f)
        {
            Flip();
        }
        else if(!facingRight && rb.velocity.x > .1f)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsWall);

        if(isWallDetected && rb.velocity.y < 0)
            canWallSlide = true;

        if(!isWallDetected)
        {
            isWallSliding = false;
            canWallSlide = false;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance, transform.position.z));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));    
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
    }
}
