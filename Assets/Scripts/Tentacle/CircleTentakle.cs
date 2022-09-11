using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTentakle : MonoBehaviour
{


   private LineRenderer lr;
   [SerializeField] public Transform target;
    [SerializeField] private float maxLength = 5;
    private float sqrMaxLength;
    private float step = 0.25f;
    private float randomDetactionScale;
    private float randomOffset;
    private Vector3[] bones = new Vector3[4];


    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        step = 1 / (float)(lr.positionCount - 1);
        sqrMaxLength = maxLength * maxLength;
        randomOffset = Random.Range(0, Mathf.PI * 2);
        randomDetactionScale = Random.Range(0.01f, 0.09f);
    }


    void LepiNize(Vector2 pOne, Vector2 pTwo, Vector2 pTre, Vector2 pFor)
    {
        Vector2 lastP = Vector2.zero;

        for (int i = 0; i < lr.positionCount; i++)
        {
            float s = step * i;

            Vector2 a = Vector2.Lerp(pOne, pTwo, s);
            Vector2 b = Vector2.Lerp(pTwo, pTre, s);
            Vector2 c = Vector2.Lerp(pTre, pFor, s);
            Vector2 d = Vector2.Lerp(a, b, s);
            Vector2 e = Vector2.Lerp(b, c, s);
            Vector2 p = Vector2.Lerp(d, e, s);
            Debug.DrawLine(lastP, p, Color.red * i);
            lastP = p;

            lr.SetPosition(i, p);
        }
    }

    void DebugDrawPoint(Vector3 point, float size, Color col)
    {
        Debug.DrawRay(point, Vector3.up * size, col);
        Debug.DrawRay(point, Vector3.right * size, col);
        Debug.DrawRay(point, Vector3.down * size, col);
        Debug.DrawRay(point, Vector3.left * size, col);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 circle = new Vector3(Mathf.Sin(Time.time), Mathf.Cos(Time.time), 0);
        // TwoToFourPointsNormalized(transform.position, target.position);
        IK(transform.position, GetTargetByDist());
        LepiNize(bones[0], bones[1], bones[2], bones[3]);
    }

    Vector3 GetTargetByDist()
    {
        //return transform.position+ transform.up * (maxLength);


        Vector3 dir = target.position - transform.position;
        Debug.DrawRay(transform.position, dir, Color.blue);
        float dist = dir.sqrMagnitude;
        dist -= sqrMaxLength;

        if (dist < 0)
            return target.position;

        return Vector3.Lerp(
            transform.position + dir.normalized * maxLength, 
            transform.position+ transform.up * (maxLength - 1) + CircleForward() * 0.5f,
            dist * randomDetactionScale);
    }

    Vector3 CircleForward()
    {
        return new Vector3(Mathf.Sin(Time.time + randomOffset), Mathf.Cos(Time.time + randomOffset), 0);
    }



    private void IK(Vector3 root, Vector3 target)
    {
        Vector3 dir = target - root;

        float step = maxLength / (bones.Length - 1);
        dir = dir.normalized;//TODO : remove


        int itt = 10;
        float delta = 0.01f;

        bones[0] = root;

        //IK
        for (int i = 0; i < itt; i++)
        {
            //Back
            bones[bones.Length - 1] = target;

            for (int b = bones.Length - 2; b > 0; b--)
            {
                bones[b] = bones[b + 1] + (bones[b] - bones[b + 1]).normalized * step;
            }
            /*
            //smooth
            Vector3 move = root - bones[0];
            for (int b = 0; b < bones.Length; b++)
            {
                bones[b] += move;
            }
            */
            //Frw
            for (int b = 1; b < bones.Length; b++)
            {
                bones[b] = bones[b - 1] + (bones[b] - bones[b - 1]).normalized * step;
            }

            if ((bones[bones.Length - 1] - target).sqrMagnitude < delta)
                break;
        }

        
        Vector3 poleTarget;
        poleTarget.x = dir.y;
        poleTarget.y = -dir.x;
        poleTarget.z = 0;
        poleTarget *= 3;
        poleTarget += dir;
        poleTarget *= maxLength * 0.5f;

        poleTarget = Quaternion.AngleAxis(Time.time * 30 + randomOffset*360, dir)* poleTarget;
        poleTarget += root;
        //Pole
        for (int b = 1; b < bones.Length - 1; b++)
        {
            Plane plane = new Plane(bones[b + 1] - bones[b - 1], bones[b - 1]);
            Vector3 projectedPole = plane.ClosestPointOnPlane(poleTarget);
            Vector3 projectedBone = plane.ClosestPointOnPlane(bones[b]);
            float angle = Vector3.SignedAngle(projectedBone - bones[b - 1], projectedPole - bones[b - 1], plane.normal);
            bones[b] = Quaternion.AngleAxis(angle, plane.normal) * (bones[b] - bones[b - 1]) + bones[b - 1];
        }
        
        //DrawLoop
         DebugDrawPoint(poleTarget, 0.25f, Color.blue);
        Vector2 lastB = bones[0];
        for (int b = 1; b < bones.Length; b++)
        {
            DebugDrawPoint(bones[b], 0.25f, Color.white * 0.25f * b * 0.5f);
            Debug.DrawLine(bones[b], lastB, Color.white);
            lastB = bones[b];
        }
    }
    /*
    [SerializeField] private float sliterWidthScale = 0.1f;
    [SerializeField] private float detectionScale = 0.03f;
     [SerializeField] private float HIGHTCHANGE = 1f;
    [SerializeField] private float tipStarrtSlitterScale = 0.01f;
   void CircleMotionTarget()
   {
       ///NOTE circle offset the IK point instead
       Vector2 circleTarget;

       Vector2 dir = target.position - transform.position;
       float sqrDist = dir.sqrMagnitude;
       sqrDist -= maxLength;
       dir = dir.normalized;

       if (sqrDist > 0)
       {
           Vector2 t = Vector3.Slerp(dir, Vector2.up, sqrDist);
           Vector2 tCross;
           tCross.x = t.y;
           tCross.y = -t.x;
           t += tCross * Mathf.Sin(Time.time + randomOffset);

       }
   }


   void TwoToFourPointsNormalized(Vector2 root, Vector2 target)
   {
       Vector2 dir = target - root;
       float sqrDist = dir.sqrMagnitude;
       dir = dir.normalized;

       float overMaxDist = (sqrDist - sqrMaxLength) / maxLength;

       float tipSlitter = Mathf.Lerp(0, 1, overMaxDist * tipStarrtSlitterScale);
       float hightRange = Mathf.Lerp(0, HIGHTCHANGE, overMaxDist * tipStarrtSlitterScale);
       dir = Vector3.Slerp(dir, Vector2.up, overMaxDist * detectionScale);

       if (overMaxDist > 0)
           sqrDist = maxLength;
       else
           sqrDist = Mathf.Sqrt(sqrDist);


       Vector2 crossDir;
       crossDir.x = dir.y;
       crossDir.y = -dir.x;

       float time = randomOffset + Time.time;
       float sinTime = Mathf.Sin(time);
       float posCosTime = Mathf.Cos(time) + 1;

       Vector2 dirRoot = Vector2.up * 0.75f + dir * 0.25f;
       Vector2 dirNeck = Vector2.up * 0.25f + dir * 0.75f;
       Vector2 cDirRoot = new Vector2(dirRoot.y, -dirRoot.x);
       Vector2 cDirNeck = new Vector2(dirNeck.y, -dirNeck.x);


       Vector2 pOne = root;
       Vector2 pTwo = root + dirRoot * sqrDist * 0.25f +
           cDirRoot * sliterWidthScale * Mathf.Sin(time * 2);
       Vector2 pTre = root + dirNeck * sqrDist * 0.75f * (posCosTime * hightRange + 1 - hightRange) +
           cDirNeck * 0.5f * sliterWidthScale * 0.5f * sinTime;
       Vector2 pFor = root + dir * sqrDist * (posCosTime * hightRange + 1 - hightRange) +
           crossDir * tipSlitter * sliterWidthScale * sinTime;

       DebugDrawPoint(pOne, 0.2f, Color.blue);
       DebugDrawPoint(pTwo, 0.2f, Color.blue);
       DebugDrawPoint(pTre, 0.2f, Color.blue);
       DebugDrawPoint(pFor, 0.2f, Color.blue);

       LepiNize(pOne, pTwo, pTre, pFor);
   }
   void TwoToFourPoints(Vector2 root, Vector2 target)
   {
       Vector2 dir = target - root;
       float sqrDist = dir.sqrMagnitude;
       sqrDist -= sqrMaxLength;

       float tipSlitter = Mathf.Lerp(0, 1, sqrDist * tipStarrtSlitterScale);
       if (sqrDist > 0)
       {
           dir = Vector3.Slerp(dir, Vector2.up, sqrDist * detectionScale);
           dir = dir.normalized * maxLength; //:(
       }
       else
       {
           tipSlitter = 0;
       }

       Vector2 crossDir;
       crossDir.x = dir.y;
       crossDir.y = -dir.x;

       float time = randomOffset + Time.time;

       Vector2 pOne = root;
       Vector2 pTwo = root + dir * 0.25f +
           crossDir * sliterWidthScale * Mathf.Sin(time * 2);
       Vector2 pTre = root + dir * 0.75f +
           crossDir * sliterWidthScale * 0.5f * Mathf.Sin(time);
       Vector2 pFor = root + dir +
           crossDir * tipSlitter * sliterWidthScale * Mathf.Sin(time);

       DebugDrawPoint(pOne, 0.2f, Color.blue);
       DebugDrawPoint(pTwo, 0.2f, Color.blue);
       DebugDrawPoint(pTre, 0.2f, Color.blue);
       DebugDrawPoint(pFor, 0.2f, Color.blue);

       LepiNize(pOne, pTwo, pTre, pFor);
   }
*/
}
