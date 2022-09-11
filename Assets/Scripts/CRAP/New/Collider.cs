using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{
    CapsuleCollider2D CCcollider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ColliderHit();
    }

    Collider2D[] colliders = new Collider2D[16];
    private void ColliderHit()
    {
        int length = Physics2D.OverlapBoxNonAlloc((Vector2)transform.position, CCcollider.size, 0, colliders);

        if (length > 0)
        {
            for (int i = 0; i < length; i++)
            {
                CCcollider.ClosestPoint(colliders[i].transform.position);
            }
        }
    }
}
