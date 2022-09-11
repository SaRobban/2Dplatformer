using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlatform : MonoBehaviour
{
    private Collider2D col;
    Vector2 lastColPos;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        col = collision.collider;
        lastColPos = col.transform.position;
        Debug.Log(col.name);
    }

    private void FixedUpdate()
    {
        if (col != null)
        {
            Vector2 addVelocity = ((Vector2)col.transform.position - lastColPos);
            Debug.DrawRay(transform.position, addVelocity);
            lastColPos = col.transform.position;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.MovePosition(rb.position + addVelocity);
            rb.velocity += Vector2.right * 0.1f;
        }
    }
}
