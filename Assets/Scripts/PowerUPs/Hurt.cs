using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform t = collision.transform.root;
        if (t.TryGetComponent(out MainCharacter sm))
        {
            sm.ChangeStateTo<CC_Hurt>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform t = collision.transform.root;
        if (t.TryGetComponent(out MainCharacter sm))
        {
            sm.ChangeStateTo<CC_Hurt>();
        }
    }
}
