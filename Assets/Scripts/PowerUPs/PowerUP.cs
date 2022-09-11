using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform t = collision.transform.root;
        if (t.TryGetComponent(out MainCharacter sm))
        {
            sm.stats.SetDubbleJumps(1);
            sm.ResetDubbleJumps();
            Destroy(gameObject);
        }
    }
}
