using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Vector2 pushBack = Vector2.one;
    public int hPPenalty = 0;
    public bool playerControl = false;

    public float coolDown = 0.25f;
    private float coolLeft;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Player enterd " + transform.name);
        if (collision.transform.tag == "Player" && coolLeft < 0)
        {
            coolLeft = coolDown;
            Hurt(collision.transform);
        }
    }
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coolLeft -= Time.deltaTime;
    }

    private void Hurt(Transform player)
    {
        Vector2 pback = pushBack;
        if (player.transform.position.x < transform.position.x)
            pback.x *= -1;
        

        Character_Move cM = player.GetComponent<Character_Move>();
        cM.airControl = playerControl;

        if(cM.climbing == true)
        {
            cM.climbing = false;
            pback.y = 0;
        }

        
        cM.overrideMovement = pback;
        Debug.DrawRay(transform.position, pback, Color.yellow, 1);
        print("pow");


    }
}
