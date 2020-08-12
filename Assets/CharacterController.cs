using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 40f;
    [SerializeField] public float jumpForce = 600f;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Animator animator;
    [SerializeField] public Transform m_GroundCheckPoint; // 用点检半径检测地面的方法有个问题：斜坡上半径过小有可能检测不到
    [SerializeField] public LayerMask m_GroundLayer;

    const float k_GroundCheckRadius = 0.1f;
    const float k_CeilingCheckRadius = 0.4f; // 天花板检查，检查能否从下蹲状态站起来
    private Vector2 movement;
    private bool jump = false;
    private bool facingRight = true;
    private bool grounded;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        // movement.y = Input.GetAxisRaw("Vertical"); // y: look up/down

        // test Face direction
        if (!facingRight && movement.x > 0 || facingRight && movement.x < 0)
        {
            FlipFacingDirection();
        }

        // Jump
        if (grounded && Input.GetButtonDown("Jump"))
        {
            jump = true;
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

        // move
        // rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        // rb.position += new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime, 0);
        rb.velocity = new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime * 10, rb.velocity.y);

        // jump （可能有个小问题：连跳，在起跳后还是grounded，并按键）
        if (jump)
        {
            // 修复在斜坡起跳获得更快的速度
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }

    private void FlipFacingDirection()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        facingRight = !facingRight;
    }
}
