using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 40f;
    [SerializeField] public float jumpForce = 600f;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Animator animator;

    private Vector2 movement;
    private bool jump = false;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        // movement.y = Input.GetAxisRaw("Vertical"); // y: look up/down

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        // move
        // rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        // rb.position += new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime, 0);
        rb.velocity = new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime * 10, rb.velocity.y);

        // jump
        if (jump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }
}
