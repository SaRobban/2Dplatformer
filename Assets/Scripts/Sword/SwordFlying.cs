using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFlying : MonoBehaviour
{
    public float speed = 10;
    public LayerMask wallMask;
    public CC_SwordControll swc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //RayCast forward to see if sword hits something-...
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.5f, wallMask);
        if (hit.collider != null)
        {
            //If sword is not inside hit Collider go to nextstep, otherwise it can attach to "oneway" platforms
            if (!hit.collider.bounds.Contains(transform.position))
            {
                swc.ChangeState("Stuck");
            }
        }
    }
}
