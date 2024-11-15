using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatorMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private int direction = -1;
    private Animator anim;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float runSpeed = 1.0f;
    [SerializeField] private float initTime = 4.0f;

    private float totalTime;

    // Start is called before the first frame update.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        totalTime = initTime;
        
    }

    // Update is called once per frame.
    void Update()
    {
        // While total time has not expired, the Ant keep moving on the same direction.
        if (totalTime > 0)
        {
            //Subtract elapsed time every frame.
            totalTime -= Time.deltaTime;

            // Update Enemy position.
            transform.Translate(direction * runSpeed * Time.deltaTime, 0, 0);

        }
        // If total time has expired, restart timer and change direction.
        else
        {
            // Reset total time.
            totalTime = initTime;

            // Change direction and Flip the sprite accordingly.
            if (direction == 1)
            {
                direction = -1;
                spriteRenderer.flipX = false;
            }
            else
            {
                direction = 1;
                spriteRenderer.flipX = true;
            }
        }
    }
}