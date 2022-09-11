using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float hurtTime = 2;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character_Move cM = collision.collider.GetComponent<Character_Move>();
        if (cM != null)
        {
            StartCoroutine(HurtPlayer(cM, collision.relativeVelocity));
        }
        Debug.Log("spikes");
    }

    IEnumerator HurtPlayer(Character_Move cc, Vector2 dir)
    {
        dir *= -1;
        dir.y = 0;
        dir = dir.normalized;
        dir += Vector2.up;

        float time = hurtTime;

        cc.overrideMovement = dir * 5;
        while (time > 0)
        {
            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
