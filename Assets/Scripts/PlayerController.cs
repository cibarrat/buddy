using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float maxJumpForce = 7f;
    [SerializeField] private float minJumpForce = 1f;
    [SerializeField] private float minDoubleJumpForce = 1f;
    [SerializeField] private float maxDoubleJumpForce = 4f;
    [SerializeField] private float jumpLag = 0.2f;
    [SerializeField] private float runStartThreshold = 0.5f;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip stompSound;
    [SerializeField] private AudioClip hitSound;
    public GameManager gameManager;
    public bool CanMove { get; set; }
    private Animator animator;
    private Rigidbody2D rigidBody;
    private Collider2D groundChecker;
    private AudioSource audioSource;
    private bool isWalking;
    private bool isJumping;
    private bool isRunning;
    private bool canRun;
    private bool canJump;
    private bool canDoubleJump;
    private float jumpCount;
    private float jumpForce;
    private float runCount;
    private float lastDirection;
    private bool isLanded;
    private bool isFalling;
    private float airborneHorizVelocity;

    public const string IS_LANDED_NAME = "isLanded";

    void Start()
    {
        groundChecker = GetComponentInChildren<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();  
        isWalking = false;
        isRunning = false;
        isJumping = false;
        canJump = true;
        canDoubleJump = true;
        isLanded = true;
        jumpCount = -1;
        runCount = -1;
        jumpForce = minJumpForce;
        airborneHorizVelocity = 0;
        CanMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool(IS_LANDED_NAME, false);
            float vertical = Input.GetAxisRaw("Vertical");
            rigidBody.velocity += Vector2.right * airborneHorizVelocity;
            walk();
            jump();
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
        text.text = $"isFalling: {isFalling}\nisLanded: {isLanded}\nCan Jump: {canJump}" +
            $"\nCan Double Jump: {canDoubleJump}\nisWalking: {isWalking}\nisRunning: {isRunning}\nCan run: {canRun}\nrunCount {runCount}\nAirborne Velocity X: {airborneHorizVelocity}" +
            $"\nVelocity X: {rigidBody.velocity.x}";

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

    private void jump()
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
            airborneHorizVelocity = rigidBody.velocity.x;
            audioSource.PlayOneShot(jumpSound);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            jumpCount = -1;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            canJump = true;
            canDoubleJump = true;
            isLanded = true;
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
}
