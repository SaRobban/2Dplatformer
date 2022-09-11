using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public Transform target;
    public Vector3 moveVector;
    public Rect restrict;
    public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        float speedMul = dir.sqrMagnitude;

        moveVector = dir.normalized * speed * speedMul;
        moveVector.z = 0;
        ScreenRestictions();
        transform.position += moveVector;
    }

    void ScreenRestictions()
    {
        if (transform.position.x + moveVector.x < restrict.xMin)
        {
            moveVector.x = restrict.xMin - transform.position.x;
        }
        else if (transform.position.x + moveVector.x > restrict.xMax)
        {
            moveVector.x = restrict.xMax - transform.position.x;
        }
        if (transform.position.y + moveVector.y > restrict.yMax)
        {
            moveVector.y = restrict.yMax - transform.position.y;
        }
        else if (transform.position.y + moveVector.y < restrict.yMin)
        {
            moveVector.y = restrict.yMin - transform.position.y;
        }
    }
}
