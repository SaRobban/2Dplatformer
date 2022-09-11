using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_FlickerMove : MonoBehaviour
{
    Vector2 orgPos;
    Vector3 target;

    [SerializeField] private float moveDist = 0.25f;
    [SerializeField] private float flickerRate = 1f;
    // Start is called before the first frame update
    void Start()
    {
        orgPos = transform.position;
        target = orgPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == target)
        {
            target= GetNewTarget();
        }

        transform.position = Vector3.MoveTowards(transform.position, target, flickerRate* Time.deltaTime);
    }

    Vector3 GetNewTarget()
    {
        return orgPos + Random.insideUnitCircle * moveDist;
    }
}
