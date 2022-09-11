using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//WARNING : This creates garbage
public class OneWayPlatforms : MonoBehaviour
{
    [SerializeField] private float errorMargin = 0.1f;
    [SerializeField] private Rigidbody2D character;
    private EdgeCollider2D[] oneWayPlatformsColliders;
    private MainCharacter cc;
    private Collider2D ccCollider;
    // Start is called before the first frame update
    void Start()
    {
        //TODO : K Tree on y pos?
        if (!character)
        {
            cc = FindObjectOfType<MainCharacter>() as MainCharacter;
            print(cc.name);
            character = cc.rb;
        }
        cc = character.GetComponent<MainCharacter>();
        ccCollider = cc.GetComponent<Collider2D>();
        oneWayPlatformsColliders = FindObjectsOfType<EdgeCollider2D>();
    }

    private void FixedUpdate()
    {
        if (cc.GetDrop())
            DisableByInput();
        else
            DisableByHight();
    }
    private void DisableByInput()
    {
        foreach (EdgeCollider2D edge in oneWayPlatformsColliders)
        {
            Physics2D.IgnoreCollision(ccCollider, edge, true);
        }
    }
    private void DisableByHight()
    {
        float ccY = character.position.y + errorMargin;

        for (int i = 0; i < oneWayPlatformsColliders.Length; i++)
        {
            if (ccY < oneWayPlatformsColliders[i].transform.position.y + oneWayPlatformsColliders[i].offset.y + oneWayPlatformsColliders[i].points[0].y)
            {
                Physics2D.IgnoreCollision(ccCollider, oneWayPlatformsColliders[i], true);
            }
            else
            {
                Physics2D.IgnoreCollision(ccCollider, oneWayPlatformsColliders[i], false);
            }
        }
        DebugLine();
    }

    private void DebugLine()
    {
        Color lineColor = Color.green;
        foreach (EdgeCollider2D edge in oneWayPlatformsColliders)
        {
            if (edge.enabled)
                lineColor = Color.green;
            else
                lineColor = Color.red;

            int x = 0;
            for (int y = 1; y < edge.pointCount; y++)
            {
                Debug.DrawLine(
                    (Vector2)edge.transform.position + edge.offset + edge.points[x],
                    (Vector2)edge.transform.position + edge.offset + edge.points[y],
                    lineColor,
                    Time.fixedDeltaTime);
                x = y;
            }
        }
    }
}
