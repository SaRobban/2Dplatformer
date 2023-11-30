using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float life = 5;
    public bool onFire;
    public bool ice;
    public bool earth;

    private Rigidbody2D rb;
    [SerializeField] private GameObject explotion;
    [SerializeField] private PS_ChangeGradient psScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fire")
        {
            onFire = true;
            psScript.ChangeToFire();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onFire)
        {
            foreach (ContactPoint2D col in collision.contacts)
            {
                //If impactangle < 30 deg
                if (Vector2.Dot(col.normal, collision.relativeVelocity.normalized) > 0.866f)
                {
                    print("Boom");
                    Instantiate(explotion, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }

        if (collision.collider.tag == "Fire")
        {
            onFire = true;
            psScript.ChangeToFire();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (psScript == null)
            psScript = GetComponentInChildren<PS_ChangeGradient>();
    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life < 0)
            Destroy(gameObject);
    }

}
