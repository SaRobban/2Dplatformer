using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moves a rigidbody in order of direction trogh the wayPoints.
[RequireComponent(typeof(Rigidbody2D))]
public class MoveingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private int t;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float speed = 2;
    [SerializeField] private bool pingPong = false;
    [SerializeField] private int direction = 1;
    [SerializeField] private bool topOnly = true;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            if (topOnly)
            {
                float yOne = collision.collider.transform.position.y;
                float yTwo = collision.otherCollider.transform.position.y + collision.otherCollider.bounds.extents.y;
                if (yOne > yTwo)
                {
                    Debug.Log(collision.transform.name + "'s : parent :" + transform.name);
                    collision.transform.parent = transform;
                }
            }
            else
            {
                collision.transform.parent = transform;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.transform.parent == transform)
        {
            collision.transform.parent = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = wayPoints[t].position;
    }

    // Update is called once per frame
    void Update()
    {
        WaypointsSelectionCheck();
        Vector2 targetVector = Vector2.MoveTowards(transform.position, wayPoints[t].position, speed * Time.deltaTime);
        transform.position = targetVector;
    }
    void WaypointsSelectionCheck()
    {
        if (transform.position != wayPoints[t].position)
        {
            return;
        }
        else
        {
            t += direction;

            if (t >= wayPoints.Length)
            {
                if (pingPong)
                {
                    direction *= -1;
                    t -= wayPoints.Length - 1 + direction;
                }
                else
                {
                    t = 0;
                }
            }
            if (t < 0)
            {
                if (pingPong)
                {
                    direction *= -1;
                    t = direction;
                }
                else
                {
                    t = wayPoints.Length - 1;
                }
            }
        }
    }
}
