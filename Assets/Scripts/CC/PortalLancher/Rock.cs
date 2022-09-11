using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    Rigidbody2D rb;

    public void SetActiveAndTrow(Vector2 velocity, Transform manager)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        gameObject.SetActive(true);
        transform.position = manager.position;
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
