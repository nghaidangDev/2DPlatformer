using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool isFacingRight;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMovement();
        Jump();
    }

    public void PlayerMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        anim.SetBool("run", horizontal != 0);
        Flip();
    }

    public void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            anim.SetTrigger("jump");
        }
    }

    public void Flip()
    {
        if ((isFacingRight && horizontal > 0) || (!isFacingRight && horizontal < 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }
}
