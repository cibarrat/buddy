using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public GameObject EnemyDeath;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float maxJumpForce = 7f;
    [SerializeField] private float minJumpForce = 1f;
    [SerializeField] private float minDoubleJumpForce = 1f;
    [SerializeField] private float maxDoubleJumpForce = 4f;
    [SerializeField] private float jumpLag = 0.2f;
    [SerializeField] private float runStartThreshold = 0.5f;
    [SerializeField] private float hitStun = 0.5f;
    [SerializeField] private float hitInvulnerabilityTime = 1f;
    [SerializeField] private float hitKnockback = 1.5f;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip stompSound;
    [SerializeField] private AudioClip hitSound;
    private GameManager gameManager;
    public bool CanMove { get; set; }
    private Animator animator;
    private Rigidbody2D rigidBody;
    private Collider2D groundChecker;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private bool isWalking;
    private bool isRunning;
    private bool canRun;
    private bool canJump;
    private bool canDoubleJump;
    private float jumpCount;
    private float jumpForce;
    private float runCount;
    private bool isLanded;
    private bool isFalling;
    private bool isInvincible;
    private float airborneHorizVelocity;

    private HashSet<Collider2D> ignoredColliders = new HashSet<Collider2D>();

    public const string IS_LANDED_NAME = "isLanded";

    void Start()
    {
        groundChecker = GetComponentInChildren<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameManager.Instance;
        isWalking = false;
        isRunning = false;
        canJump = true;
        canDoubleJump = true;
        isLanded = true;
        jumpCount = -1;
        runCount = -1;
        jumpForce = minJumpForce;
        airborneHorizVelocity = 0;
        CanMove = true;
    }

    void Update()
    {
        if (CanMove)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool(IS_LANDED_NAME, false);
            float vertical = Input.GetAxisRaw("Vertical");
            walk();
            checkForJump();
            if (rigidBody.velocity.y < -0.0001)
            {
                isFalling = true;
            }
            else if (rigidBody.velocity.y > -0.0001)
            {
                isFalling = false;
            }
            if (isLanded)
            {
                audioSource.pitch = 1;
            }
            animator.SetBool("isLanded", isLanded);
            animator.SetBool("isFalling", isFalling);
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isRunning", isRunning);
        }
        if (isInvincible)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; 
        } else
        {
            spriteRenderer.enabled = true;
        }
    }

    private void walk()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (runCount > -1 && runCount < runStartThreshold)
        {
            runCount += Time.deltaTime;
        }
        if (horizontal != 0f)
        {
            if (isLanded && !isRunning && runCount < 0)
            {
                runCount = 0;
            }
            isWalking = true;
            if (isLanded && !isRunning && canRun)
            {
                isRunning = true;
                canRun = false;
                runCount = -1;
            };
            transform.Translate(horizontal * (isRunning ? runSpeed : walkSpeed) * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(horizontal, 1);
        }
        else
        {
            if (isRunning)
            {
                runCount = 0;
                canRun = true;
            }
            canRun = runCount != -1 && runCount < runStartThreshold;
            if (!canRun)
            {
                runCount = -1;
            }
            isRunning = false;
            isWalking = false;
        }
        
    }

    private void checkForJump()
    {
        bool isDoubleJump = false;
        if (jumpCount > -1)
        {
            jumpCount += Time.deltaTime;
        }
        if ((canJump || canDoubleJump) && Input.GetButtonDown("Jump"))
        {
            if (!canJump)
            {
                canDoubleJump = false;
                isDoubleJump = true;
                audioSource.pitch = 0.7f;
                jumpCount = jumpLag + 1;
            }
            else
            {
                canJump = false;
            }
            animator.SetTrigger("jump");
            jumpCount = 0;
        }
        if (Input.GetButton("Jump"))
        {
            jumpForce = isDoubleJump? maxDoubleJumpForce : maxJumpForce;
            
        }
        if (Input.GetButtonUp("Jump"))
        {
            jumpForce = isDoubleJump? minDoubleJumpForce : minJumpForce;
        }
        if (jumpCount > jumpLag)
        {
            audioSource.PlayOneShot(jumpSound);
            jump(jumpForce);
        }
    }

    public void jump(float jumpForce)
    {
        airborneHorizVelocity = rigidBody.velocity.x;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        jumpCount = -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) {
            case("Tilemap"):
        
                canJump = true;
                canDoubleJump = true;
                isLanded = true;
                    break;
            case ("Enemy"):
                if (isFalling)
                {
                    audioSource.PlayOneShot(stompSound);
                    GameObject.Destroy(collision.gameObject);

                    // Instantiate the EnemyDeath animation and destroy it after one second.
                    GameObject clone = Instantiate(EnemyDeath,collision.transform.position , collision.transform.rotation);
                    Destroy(clone, 1.0f);
                    
                    animator.SetTrigger("jump");
                    jump(minDoubleJumpForce);
                }
                break;
            case ("Collectible"):
                collision.GetComponent<Collectible>().Effect();
                GameObject.Destroy(collision.gameObject);
                break;
        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            canJump = false;
            isLanded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isInvincible)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                ignoredColliders.Add(collision.collider);
            }
            else
            {
                audioSource.PlayOneShot(hitSound);
                StartCoroutine(PlayerHit(1));
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isInvincible)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            ignoredColliders.Add(collision.collider);
            
        }
    }
    public IEnumerator PlayerHit(int dmg)
    {
        animator.SetTrigger("Hit");
        gameManager.DamagePlayer(dmg);
        if (gameManager.PlayerHealth <= 0)
        {
            CanMove = false;
        }
        rigidBody.velocity = new Vector2(transform.localScale.x * -hitKnockback, hitKnockback);
        StartCoroutine(BecomeInvulnerable(hitInvulnerabilityTime));
        TogglePlayerMovement();
        yield return new WaitForSeconds(hitStun);
        TogglePlayerMovement();
    }

    public void TogglePlayerMovement()
    {
        CanMove = !CanMove;
    }

    IEnumerator BecomeInvulnerable(float time)
    {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
        foreach (Collider2D collider in ignoredColliders)
        {
            if (collider)
            {
                Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
            }
        }
    }
}
