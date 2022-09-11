using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastIK : MonoBehaviour
   

{
    //NOTE last bone is a nub
    [Header("Stuff")]
    public int itterations = 10;
    public float delta = 0.01f;

    public Transform root;
    public Transform target;
    public Vector3 pole;



    [Header("Bones Root to Nub")]
    public Transform[] bones;
    
    private Vector3[] positions;
    private float[] boneMags;
    private float wholeMag;

    private void Start()
    {
        boneMags = new float[bones.Length];
        positions = new Vector3[bones.Length];

        wholeMag = 0;

        for (int i =0; i < bones.Length; i++)
        {
            boneMags[i] = transform.GetComponentInChildren<Renderer>().bounds.extents.y * 2;
            positions[i] = bones[i].position;
            wholeMag += boneMags[i];
        }
    }

    private void Update()
    {
        pole = target.up;
        Vector3 dir = target.position - root.position;
        float dist = dir.magnitude;
        dir = dir.normalized;

        bones[0].position = root.position;

        //if target is outside of bonesLength else IK
        if(dist > wholeMag)
        {
            //Set rootbone to position and direction
            bones[0].position = root.position;
            bones[0].up = dir;
            int y = 0;

            //Set other bones to position
            for (int i = 1; i < bones.Length; i++)
            {
                bones[i].position = bones[y].position + bones[y].up * boneMags[y];
                bones[i].up = dir;
                y = i;
            }
        }
        else
        {
           //IK
           for(int itt = 0; itt< itterations; itt++)
            {
                //Back IK
                for(int b = bones.Length -1; b > 0; b--)
                {
                    if (b == bones.Length - 1)
                    {
                        bones[b].position = target.position;
                    }
                    else
                    {
                        bones[b].position = bones[b + 1].position + (bones[b].position - bones[b + 1].position).normalized * boneMags[b];
                    }
                }

                //Forward
                for(int f = 1; f < bones.Length; f++)
                {
                    bones[f].position = bones[f - 1].position + (bones[f].position - bones[f - 1].position).normalized * boneMags[f - 1];
                }

                //Min delta
              
            }


            //Rotate bone
            //rotate (move) pos towards to Pole
            for (int i = 1; i < bones.Length - 1; i++)
            {
                Plane plane = new Plane(bones[i + 1].position - bones[i - 1].position, bones[i - 1].position);

                Vector3 projectedPole = plane.ClosestPointOnPlane(bones[i].position + pole * 10);
                Vector3 projectedBone = plane.ClosestPointOnPlane(bones[i].position);
                float angle = Vector3.SignedAngle((projectedBone - bones[i - 1].position).normalized, (projectedPole - bones[i - 1].position).normalized, plane.normal);

                bones[i].position = Quaternion.AngleAxis(angle, plane.normal) * (bones[i].position - bones[i - 1].position) + bones[i - 1].position;
            }
            for (int n = 0; n < bones.Length -1; n++)
            {
                bones[n].up = (bones[n + 1].position - bones[n].position).normalized;
             }
        }

    }

}