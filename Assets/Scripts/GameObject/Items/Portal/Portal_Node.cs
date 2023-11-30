using UnityEngine;

public class Portal_Node : MonoBehaviour
{
    public Portal_connector pc;
    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            pc.pl = collision.gameObject;
            pc.inPn = true;
            if (id == 0)
                pc.one = true;
            else
                pc.one = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            pc.inPn = false;
        }
    }
}
