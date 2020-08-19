using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 400f;
    [SerializeField] public float jumpForce = 600f;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] public Transform m_GroundCheckPoint; // 用点检半径检测地面的方法有个问题：斜坡上半径过小有可能检测不到
    [SerializeField] public LayerMask m_GroundLayer;
    [SerializeField] public Text m_CherryScore;
    private int m_CherryCount = 0;

    const float k_GroundCheckRadius = 0.1f;
    const float k_CeilingCheckRadius = 0.4f; // 天花板检查，检查能否从下蹲状态站起来
    private Vector2 movement;
    private bool jumpPressed = false;
    private bool facingRight = true;
    private bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        // movement.y = Input.GetAxisRaw("Vertical"); // y: look up/down

        if (animator.GetBool("Jumping") && rb.velocity.y <= 0)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
        if (animator.GetBool("Falling") && grounded)
        {
            animator.SetBool("Falling", false);
        }

        // test Face direction
        if (!facingRight && movement.x > 0 || facingRight && movement.x < 0)
        {
            FlipFacingDirection();
        }

        // Jump
        if (grounded && Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        // 测试是否在地上
        grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheckPoint.position, k_GroundCheckRadius, m_GroundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
        }

        // 移动
        // rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        // rb.position += new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime, 0);
        rb.velocity = new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        animator.SetFloat("Speed", Math.Abs(rb.velocity.x));

        // jump （可能有个小问题：连跳，在起跳后还是grounded，并按键）
        if (jumpPressed)
        {
            // 修复在斜坡起跳获得更快的速度
            jump();
            jumpPressed = false;
        }

        if (getCherry)
        {
            m_CherryCount++;
            m_CherryScore.text = m_CherryCount.ToString();
            getCherry = false;
        }
    }

    private void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0f, jumpForce));
        animator.SetBool("Jumping", true);
    }

    private void FlipFacingDirection()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        facingRight = !facingRight;
    }

    private bool getCherry = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            getCherry = true;
            Destroy(collision.gameObject);
        }
    }

    // 碰到敌人，消灭或受伤
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Enemy")
        {
            if (animator.GetBool("Falling"))
            {
                // 消灭敌人
                Destroy(collisionInfo.gameObject);
                // Jump
                jump();
            }
        }
    }
}
