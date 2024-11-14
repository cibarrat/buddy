using System.Collections;
using System.Collections.Generic;
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
    private Animator animator;
    private Rigidbody2D rigidBody;
    private Collider2D groundChecker;
    private bool isWalking;
    private bool isJumping;
    private bool isRunning;
    private bool canJump;
    private bool canDoubleJump;
    private float jumpCount;
    private float jumpForce;

    public const string IS_LANDED_NAME = "isLanded";

    void Start()
    {
        groundChecker = GetComponentInChildren<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator.SetBool(IS_LANDED_NAME, true);
        isWalking = false;
        isRunning = false;
        isJumping = false;
        canJump = true;
        canDoubleJump = true;
        jumpCount = -1;
        jumpForce = minJumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(jumpCount);
        animator.SetBool("isFalling", false);
        animator.SetBool("isWalking", false);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0f)
        {
            walk(horizontal);

        }
        jump();
        if (rigidBody.velocity.y < -0.0001)
        {
            animator.SetBool("isFalling", true);
        } else
        {
            animator.SetBool("isFalling", false);
        }
    }

    private void walk(float horizontal)
    {
        transform.Translate(horizontal * walkSpeed * Time.deltaTime, 0, 0);
        transform.localScale = new Vector3(horizontal, 1);
        animator.SetBool("isWalking", true);
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
                animator.SetTrigger("doubleJump");
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
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            jumpCount = -1;
            animator.SetBool("isLanded", false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            animator.SetBool(IS_LANDED_NAME, true);
            canJump = true;
            canDoubleJump = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            animator.SetBool(IS_LANDED_NAME, false);
        }
    }
}
