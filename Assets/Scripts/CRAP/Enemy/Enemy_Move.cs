using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    public bool turnOnCasm;
    private bool invertDir;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    public float speed;

    public SpriteRenderer eSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        eSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
        Vector2 dir = (Vector2.down + Vector2.right) *0.5f;

        if (!invertDir)
        {
            dir.x *= -1;
            eSprite.flipX = false;
        }
        else
        {
            eSprite.flipX = true;
        }

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, 1, groundMask);
        Debug.DrawRay(transform.position, dir, Color.green);

        if (hit.Length != 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if(Mathf.Abs( hit[i].normal.x) > 0.25f)
                {
                    //Hit wall, slope
                    invertDir = !invertDir;
                }
                moveDir.y = 0;
            }
        }
        else if(turnOnCasm)
        {
            //hit casm
            invertDir = !invertDir;
        }

        if (invertDir)
            moveDir.x = 1;
        else
            moveDir.x = -1;

        moveDir.x *= speed;
        moveDir.y -= 10 * Time.fixedDeltaTime;

        Vector2 pos = transform.position;

        //Move
        rb.MovePosition(pos + moveDir * Time.fixedDeltaTime);
    }
}
