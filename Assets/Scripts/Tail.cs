using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] Transform testAnimationTarget;
    [SerializeField] private float liftStr = 4;
    [SerializeField] private float momentumMultiplyer = 0.01f;
    [SerializeField] private float momentumMemory = 2;
    [SerializeField] private float swayStr = 1;

    [SerializeField] private float partDist = 0.1f;

    private Transform[] tailSegments;
    private Vector2[] storedMomentum;
    private float tailSegmentDiv;
    // Start is called before the first frame update
    void Awake()
    {
        sprite = testAnimationTarget.GetComponentInParent<SpriteRenderer>();

        tailSegments = new Transform[transform.childCount];
        Debug.Log("l : " + tailSegments.Length);
        storedMomentum = new Vector2[tailSegments.Length];
        Vector2 lastTailSegmentPos = testAnimationTarget.position;
        Vector2 targetPos = testAnimationTarget.position;
        for (int i = 0; i < tailSegments.Length; i++)
        {
            Transform child = transform.GetChild(i).gameObject.transform;
            tailSegments[i] = child;
            tailSegments[i].position = targetPos + Vector2.right * partDist * i;
            tailSegments[i].up = lastTailSegmentPos - (Vector2)tailSegments[i].position;
            storedMomentum[i] = Vector2.zero;
            lastTailSegmentPos = tailSegments[i].position;
        }

        tailSegmentDiv = 1f / (float)tailSegments.Length;
    }

    // Update is called once per frame
    public void UpdateTail(float deltaTime)
    {


        //TODO : See if momentum array is nessesery, try a target momental force!?
        //WARNING!!! : HOTFIX!!! Since deltatime can be 0 in first frame.
        float deltaDiv = 0.1f;
        if (deltaTime > 0)
            deltaDiv = 1 / deltaTime;

        Vector2 targetPos = testAnimationTarget.position;

        Vector2 targetDir = testAnimationTarget.up;
        if (sprite.flipX)
        {
            targetPos = testAnimationTarget.localPosition;
            targetPos.x *= -1;
            targetPos += (Vector2)testAnimationTarget.parent.transform.position;
            targetDir.x *= -1;
        }

        Vector2 tailLiftForce = Vector2.up * liftStr * tailSegmentDiv;
        Vector2 swayForceDirection = Vector2.right * Mathf.Sin(Time.time * 2) + Vector2.up * Mathf.Cos(Time.time * 4);
        swayForceDirection *= tailSegmentDiv;
        swayForceDirection *= swayStr;

        Vector2 prevPos = targetPos;
        for (int i = 0; i < tailSegments.Length; i++)
        {
            Vector2 currentPos = (Vector2)tailSegments[i].position;
            Vector2 tailDirectionForce = targetDir;
            tailDirectionForce += tailLiftForce * i;
            tailDirectionForce += storedMomentum[i] * momentumMultiplyer * tailSegmentDiv * i;
            tailDirectionForce += swayForceDirection * i;

            Vector2 newPos = currentPos;
            newPos += tailDirectionForce * deltaTime;

            //Constrain position
            Vector2 dir = newPos - prevPos;
            float dist = dir.sqrMagnitude;
            float compDist = partDist * partDist;
            if (dist > compDist)
            {
                newPos = prevPos + dir.normalized * partDist;
            }

            //Add to momentum to array
            Vector2 move = (newPos - currentPos) * deltaDiv;
            storedMomentum[i] = Vector2.Lerp(storedMomentum[i], move, momentumMemory);

            //Set position
            tailSegments[i].transform.up = newPos - prevPos;
            tailSegments[i].position = newPos;
            prevPos = tailSegments[i].position;
        }
    }
}
