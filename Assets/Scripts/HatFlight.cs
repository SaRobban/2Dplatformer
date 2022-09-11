using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatFlight : MonoBehaviour
{
    public float speed = 0.2f;
    public float damping = 0.2f;


    private int hits;
    private RaycastHit2D[] hitData;
    public Rigidbody2D rb;

    Vector2 dir = Vector2.zero;

    [SerializeField] private float minDistToG = 1;

    private Collider2D thisCollider;
    // Start is called before the first frame update
    void Start()
    {
        thisCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        hitData = new RaycastHit2D[8];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        dir = rb.velocity;
        //Gravity
        dir.y -= 10f * Time.fixedDeltaTime;
        
        float distToG = minDistToG;
        hits = Physics2D.CircleCastNonAlloc((Vector2)transform.position, 0.1f, Vector2.down, hitData, minDistToG);
        if (hits > 0)
        {
            float closest = minDistToG;
            for (int i = 0; i < hits; i++)
            {
                if (hitData[i].distance < closest && hitData[i].collider != thisCollider)
                    closest = hitData[i].distance;
            }
            distToG = closest;
        }

        dir.y += (minDistToG - distToG * distToG) * 100 * Time.fixedDeltaTime * speed;
        dir.y *= 1 - damping * Time.fixedDeltaTime;
        rb.AddForce(Vector2.up * (minDistToG - distToG * distToG) * 40, ForceMode2D.Force);

       // rb.velocity = dir;

    }
}
