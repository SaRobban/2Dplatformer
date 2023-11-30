using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPlatform : MonoBehaviour
{
    public Vector2 staticForce;
    public CC_SwordControll swc;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<CC_Move>())
        {
            if (Vector2.Dot(Vector2.up, collision.transform.position - transform.position) > 0.5f)
            {
                collision.transform.GetComponent<CC_Move>().addForce = staticForce;
                swc.ChangeState("Bounce");
            }
        }
    }
}
