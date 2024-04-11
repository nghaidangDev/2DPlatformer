using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadderMovement : MonoBehaviour
{
    private float vertical;
    public float speed;
    private Rigidbody2D rb;
    private bool isLadder;
    private bool isCliming;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        vertical = Input.GetAxis("Vertical");
        if (isLadder && Mathf.Abs(vertical) > 0)
        {
            isCliming = true;
        }
    }

    private void FixedUpdate()
    {
        if (isCliming)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isCliming = false;
        }
    }
}
