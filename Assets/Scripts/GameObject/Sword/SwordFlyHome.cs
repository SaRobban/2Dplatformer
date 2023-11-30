using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFlyHome : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 360f;
    public float speed = 10f;

    public CC_SwordControll swc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        if(transform.position == target.position)
        {
            swc.ChangeState("End");
        }
    }
}
