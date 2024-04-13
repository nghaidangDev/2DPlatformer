using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private Vector3 respawnPoint;
    public GameObject deadZone;

    public Text scoreTxt;

    public HealthBar healthBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
        scoreTxt.text = "Score: " + Scoring.totalScore;
    }

    private void Update()
    {
        PlayerMovement();
        Jump();
        MoveDeadZone();
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

    public void MoveDeadZone()
    {
        deadZone.transform.position = new Vector2(transform.position.x, deadZone.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            transform.position = respawnPoint;
        }
        else if (collision.CompareTag("CheckPoint"))
        {
            respawnPoint = transform.position;
        }
        else if (collision.CompareTag("NextLevel"))
        {
            SceneManager.LoadScene(1);
            respawnPoint = transform.position;
        }
        else if (collision.CompareTag("PreviosLevel"))
        {
            SceneManager.LoadScene(0);
            respawnPoint = transform.position;
        }
        else if (collision.CompareTag("Items"))
        {
            Scoring.totalScore += 1;
            scoreTxt.text = "Score: " + Scoring.totalScore;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Traps"))
        {
            healthBar.Damage(0.002f);
        }
    }
}
