using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{

    public LayerMask groundLayer;
    public Transform legTarget;
    public float footMaxDist = 2;
    public Vector2 target;
    public Vector2 tUp;
    public float speed = 4;

    public bool walkToggle;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        legTarget.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        Vector2 mDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = mDir;

        Vector2 legPos = Vector2.MoveTowards(legTarget.position, target, speed * Time.deltaTime);
        legTarget.position = legPos;
        legTarget.up = tUp;

        if (walkToggle)
        {
            if(legPos == target)
            {
                setFootPos(mDir);
            }
        }
        else
        {
            float distToFoot = (transform.position - legTarget.position).sqrMagnitude;

            if (distToFoot > footMaxDist * footMaxDist)
            {
                target = transform.position;
                walkToggle = !walkToggle;
            }
        }
        Debug.DrawRay(target, Vector3.up, Color.blue);
    }

    void setFootPos(Vector2 mDir)
    {
        NewTargetPos(mDir);
        walkToggle = !walkToggle;
    }

    void NewTargetPos(Vector2 mDir)
    {
        //Create an array of rays, shoot to set new footTarget, if a target is valid break


        bool newTarget = false;
        //Get new target pos
        mDir = mDir.normalized;

        Vector2[] vs = new Vector2[12];

        vs[0] = mDir;
        vs[1] = Quaternion.AngleAxis(20, Vector3.forward) * mDir;
        vs[2] = Quaternion.AngleAxis(-20, Vector3.forward) * mDir;
        vs[3] = Quaternion.AngleAxis(40, Vector3.forward) * mDir;
        vs[4] = Quaternion.AngleAxis(-40, Vector3.forward) * mDir;
        vs[5] = Quaternion.AngleAxis(60, Vector3.forward) * mDir;
        vs[6] = Quaternion.AngleAxis(-60, Vector3.forward) * mDir;
        vs[7] = Quaternion.AngleAxis(80, Vector3.forward) * mDir;
        vs[8] = Quaternion.AngleAxis(-80, Vector3.forward) * mDir;
        vs[9] = Quaternion.AngleAxis(100, Vector3.forward) * mDir;
        vs[10] = Quaternion.AngleAxis(-100, Vector3.forward) * mDir;
        vs[11] = Quaternion.AngleAxis(120, Vector3.forward) * mDir;

        for (int i = 0; i < vs.Length; i++)
        {
            Debug.DrawRay(transform.position, vs[i] * footMaxDist, Color.red, 1);
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, vs[i], footMaxDist, groundLayer);
            if (hit.Length > 0)
            {
                for (int y = 0; y < hit.Length; y++)
                {
                    Vector2 p = hit[y].point + hit[y].normal * 0.01f;
                    if (!hit[y].collider.bounds.Contains(p))
                    {
                        target = hit[y].point;
                        //tUp = hit[y].normal;
                        tUp = mDir;
                        Debug.DrawRay(transform.position, vs[i] * footMaxDist, Color.green, 1);
                        newTarget = true;
                    }
                }
            }
            if (newTarget == true)
                break;
        }
    }
}
