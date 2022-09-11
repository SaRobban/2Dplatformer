using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacle : MonoBehaviour
{
    public Transform target;
    public Vector2 targetPos;
    public float segLength = 0.5f;
    public float maxLength = 10;
    int itt;
    float random;
    // Start is called before the first frame update
    void Start()
    {
        itt = Mathf.RoundToInt(maxLength / segLength);
        random = Random.value * 100;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, target.position, Color.red);
        Target();
        //Slitter();
        Circle();
    }

    void Target()
    {
        float seek = (transform.position - target.position).magnitude - (maxLength);
        seek /= maxLength;
        targetPos = Vector3.Lerp(target.position, transform.position + Vector3.up * 5, seek);
    }

    void Circle()
    {
        Vector2[] points = new Vector2[itt];


        Vector2 point = transform.position;

        Vector2 dir = targetPos - point;
        float dist = dir.magnitude/ maxLength;
        float left = Mathf.Clamp(maxLength - dist, 0, 1);
        dist *= segLength;
        float time = Time.time + random;
        Vector2 lastStep = point;

        for (int i = 0; i < itt; i++)
        {
            float ittStep = (float)i / (float)itt;
            ittStep *= Mathf.PI;
            Vector2 step =
                new Vector2(
                    Mathf.Sin
                        (ittStep * left + time) * (itt-i)*0.02f,
                    Mathf.Cos
                        (ittStep * left + time) * (itt - i)*0.02f
                    );
            step = Quaternion.Euler(0, 0, 90) * step;
            step += dir.normalized * i * dist;
            Debug.DrawLine(point + lastStep, point + step);
            points[i] = point + step;
            lastStep = step;
        }
/*
        for(int i = 1; i < points.Length; i++)
        {
            Vector2 dirr = points[i - 1] - points[i];
            dirr = new Vector2(dirr.y, -dirr.x);
            points[i] += dirr * Mathf.Sin((float)i*0.25f + Time.time) * 0.75f;
            Debug.DrawLine(points[i - 1] , points[i], Color.red);
        }
*/
    }

    void Slitter()
    {
        Vector2 point = transform.position;
        Vector2 dir = targetPos - point;
        float dist = dir.magnitude;
        dir = dir.normalized;
        dir *= segLength;

        for (int i = 0; i < itt; i++)
        {
            Vector2 cross = new Vector2(dir.y, -dir.x);
            float left = Mathf.Clamp(dist - i * segLength, 0, maxLength) / maxLength;
            cross = cross * Mathf.Sin(i * segLength + random + Time.time) * (left + 1) * 0.05f;

            Debug.DrawRay(point, cross + dir, Color.green);
            dir += cross;
            point += dir;
        }

    }
}
