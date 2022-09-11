using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : MonoBehaviour
{

    [SerializeField] float pushSpeed = 3;
    [SerializeField] float gravity = 10;
    [SerializeField] Rigidbody2D rb;
    float currentG = 0;
    Coroutine running;

    bool wasKinematic;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
        if (collision.collider.TryGetComponent(out MainCharacter mainC))
        {
            Debug.Log("MainC hit");
        }
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (HitSide(collision.relativeVelocity))
        {
            rb.isKinematic = false;
            wasKinematic = false;
            rb.velocity = PushForce(collision.relativeVelocity) * pushSpeed;
          //  if (running == null)
          //      running = StartCoroutine(WaitForRest());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(wasKinematic)

        if (HitSide(collision.relativeVelocity))
        {
            wasKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (wasKinematic)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
    }

    bool HitSide(Vector2 impactVelocity)
    {
        float dot = Vector2.Dot(impactVelocity, Vector2.up);
        if (dot < 0.707f && dot > -0.707)
        {
            return true;
        }
        return false;
    }

    IEnumerator WaitForRest()
    {
        yield return null;
        while (rb.velocity.sqrMagnitude > 0.1f)
        {
            yield return null;
        }
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        running = null;
    }

    Vector2 PushForce(Vector2 impactVelocity)
    {
        float dot = Vector2.Dot(impactVelocity, Vector2.up);
        if (dot < 0.707f && dot > -0.707)
        {
            //HitSide
            if (impactVelocity.x < 0)
                return Vector2.left;
            return Vector2.right;
        }
        return Vector2.zero;


    }
}
