using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charmovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private float movementInputDirection;
    public Rigidbody2D rb;
    public bool grounded;
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform wallCheck2;
    public Transform wallCheck3;
    public float jumpPower = 100f;
    public LayerMask whatIsGround;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public GameObject projectile;
    public GameObject blade;
    public Transform shotPoint;
    public Transform shotPointUp;
    public Transform shotPointAngle;
    public float startShotCooldown;
    public float startCutCooldown;
    public float acceleration;
    public GameObject text;
    public int hp = 100;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;

    private bool lookingUp;
    private bool lookingAngle;
    private float lastImageXpos;
    private bool isFacingRight = true;
    private bool canJump;
    private bool isTouchingWall;
    private bool walking;
    private float shotCooldown = 0.25f;
    private float cutCooldown = 0.25f;
    private float curSpeed;
    private Animator anim;
    private bool dead = false;
    private bool isDashing;
    private float dashTimeLeft;
    private float lastDash = -100f;


    private void Start()
    {
        anim = GetComponent<Animator>();
        blade.SetActive(false);
    }

    private void Update()
    {
        if (!text.activeSelf && !isDashing)
        {
            CheckInput();
            CheckMovementDirection();
            CheckIfCanJump();
            UpdateAnimations();
            Shoot();
            Cut();
        }
        CheckDash();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            ApplyMovement();
        }
        CheckSurroundings();
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if(Input.GetButtonDown("Dash"))
        {
            if(Time.time >= (lastDash + dashCooldown))
            {
                AttemptToDash();
            }
        }

        if (Input.GetAxisRaw("Vertical") == 1 && Input.GetAxisRaw("Horizontal") != 0)
        {
            lookingAngle = true;
            lookingUp = false;
        }
        else if (Input.GetAxisRaw("Vertical") == 1 && Input.GetAxisRaw("Horizontal") == 0)
        {
            lookingUp = true;
            lookingAngle = false;
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            lookingUp = false;
            lookingAngle = false;
        }
    }

    private void ApplyMovement()
    {
        if (curSpeed >= speed)
        {
            rb.velocity = new Vector2(speed * movementInputDirection, rb.velocity.y);
        }
        else if (curSpeed < speed)
        {
            rb.velocity = new Vector2(curSpeed * movementInputDirection, rb.velocity.y);
            if (movementInputDirection != 0)
            {
                curSpeed += acceleration;
            }
        }
        if (movementInputDirection == 0)
        {
            if (isFacingRight == true && curSpeed > 0)
            {
                rb.velocity = new Vector2(curSpeed, rb.velocity.y);
                curSpeed -= acceleration;
            }
            else if (curSpeed > 0)
            {
                rb.velocity = new Vector2(-curSpeed, rb.velocity.y);
                curSpeed -= acceleration;
            }
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (rb.velocity.x != 0)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    private void CheckSurroundings()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround) == true || Physics2D.Raycast(wallCheck2.position, transform.right, wallCheckDistance, whatIsGround) == true || Physics2D.Raycast(wallCheck3.position, transform.right, wallCheckDistance, whatIsGround) == true)
        {
            isTouchingWall = true;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            isTouchingWall = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", walking);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    private void CheckIfCanJump()
    {
        if (grounded == true)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if(isDashing)
        {
            if (dashTimeLeft > 0)
            {
                if(isFacingRight)
                    rb.velocity = new Vector2(dashSpeed, rb.velocity.y);
                else
                    rb.velocity = new Vector2(-dashSpeed, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if(dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
            }
        }
    }

    private void Shoot()
    {
        if (Input.GetButton("Fire1"))
        {
            if (shotCooldown <= 0)
            {
                if (lookingUp == false && lookingAngle == false)
                {
                    Instantiate(projectile, shotPoint.position, shotPoint.rotation);
                    shotCooldown = startShotCooldown;
                }
                else if(lookingUp == true)
                {
                    Instantiate(projectile, shotPointUp.position, shotPointUp.rotation);
                    shotCooldown = startShotCooldown;
                }
                else if (lookingAngle == true)
                {
                    Instantiate(projectile, shotPointAngle.position, shotPointAngle.rotation);
                    shotCooldown = startShotCooldown;
                }
            }
            if (shotCooldown > 0)
            {
                shotCooldown -= Time.deltaTime;
            }
        }
        else if (shotCooldown > 0)
        {
            shotCooldown -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void Cut()
    {
        if(cutCooldown <= 0.7)
        {
            blade.SetActive(false);
        }
        if (cutCooldown <= 0)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("kek");
                blade.SetActive(true);
                cutCooldown = startCutCooldown;
            }
        }
        else if (cutCooldown > 0)
        {
            cutCooldown -= Time.deltaTime;
        }
    }
}