/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public float wordPointY;
    public bool hasEnterdAbove;
    public bool startCheck;
    private float bodyhight = 0.96875f;
    MainCharacter playerBody;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO : REname stuff
        MainCharacter playerBody;
        if (collision.gameObject.TryGetComponent<MainCharacter>(out playerBody))
        {
            this.playerBody = playerBody;
            startCheck = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (startCheck)
        {
            Debug.DrawRay(new Vector2(transform.position.x, wordPointY), Vector3.left, Color.red);
            Debug.DrawRay(collision.collider.transform.position + Vector3.up * bodyhight, Vector3.left, Color.yellow);
            if (collision.collider.transform.position.y + bodyhight > wordPointY)
            {
                hasEnterdAbove = true;
            }
            if (hasEnterdAbove && collision.collider.transform.position.y + bodyhight <= wordPointY)
            {
                //TODO : GetbodyHight from main.stats
                //TODO : RightLeft mount
                Debug.Log("ChangeStateTo ledge");
                Vector2 pos = playerBody.rb.position;
                pos.y = wordPointY - bodyhight;
                playerBody.TeleportTo(pos);
                playerBody.ChangeStateTo<CC_LedgeHang>();
                ResetLedge();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        MainCharacter main;
        if (collision.gameObject.TryGetComponent<MainCharacter>(out main))
        {
            ResetLedge();
        }
    }

    public void ResetLedge()
    {
        hasEnterdAbove = false;
        playerBody = null;
        startCheck = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        wordPointY = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(new Vector2(transform.position.x, wordPointY), Vector3.left, Color.red);
    }
}
*/