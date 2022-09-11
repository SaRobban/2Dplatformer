using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCustom : MonoBehaviour
{
    [Header("Leg")]
    public Transform root;
    private float rootMag;
    public Transform foot;
    public float footMag;
    public Transform legTarget;

    [Header("IK")]
    public int iterations = 10;
    public float minDelta = 0.01f;
    public Vector3 pole = Vector3.up;
    public Transform IKTarget;

    public Transform[] bones;
    private Vector3[] bonesPos;
    private float[] bonesMag;

    private float bonesMagnetude;
    private float wholeMagnetude;

    //Made with tutorial from DitzelGames "C# Inverse Kinematics in Unity"
    void Start()
    {
        rootMag = (bones[0].position - root.position).magnitude;
        //footMag = (bones[bones.Length-1].position - foot.position).magnitude;

        SetupIK();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlaceLeg();
        SolveIK();
    }

    public void PlaceLeg()
    {
        //placefoot
        Vector3 dir = legTarget.position - root.position;
        float dist = dir.sqrMagnitude;

        if(dist < wholeMagnetude * wholeMagnetude)
        {
            foot.up = legTarget.up;
            foot.position = legTarget.position;
            print(footMag);
        }
        else
        {
            foot.position = root.position + dir.normalized * Mathf.Sqrt(dist);
        }

        root.LookAt(legTarget.position);
        bones[0].position = root.position + root.forward * rootMag;
        bonesPos[0] = root.position + root.forward * rootMag;
        IKTarget.position = foot.position + foot.up * footMag;

        wholeMagnetude = bonesMagnetude + rootMag;
    }

    public void SetupIK()
    {
        bonesPos = new Vector3[bones.Length];
        bonesMag = new float[bones.Length];

        bonesMagnetude = 0;


        for (int x = 0; x < bones.Length - 1; x++)
        {
            bonesPos[x] = bones[x].position;
            bonesMag[x] = (bones[x + 1].position - bones[x].position).magnitude;

            bonesMagnetude += bonesMag[x];
        }

        
        //Last pos
        bonesPos[bones.Length - 1] = bones[bones.Length - 1].position;
        bonesMag[bones.Length - 1] = 0.5f;
        /*
        //Debug
        for (int i = 0; i < bones.Length; i++)
        {
            //Debug.DrawRay(bonesPos[i], Vector3.up, Color.red, 10);
        }
        */
    }

    public void SolveIK()
    {
        Vector3 dirToIKT = IKTarget.position - bones[0].position;
        float distToIKT = dirToIKT.sqrMagnitude;

        //If IKT out of reach : set bone in line to IKTarget else : IK
        if(distToIKT > bonesMagnetude * bonesMagnetude)
        {
            Vector3 root = bones[0].position;
            dirToIKT = dirToIKT.normalized;
            for(int i = 0; i < bones.Length; i++)
            {
                bonesPos[i] = root;
                root += dirToIKT * bonesMag[i];
            }
        }
        else
        {
            bonesPos[bones.Length - 1] = IKTarget.position; //set position of last bone to target

            //prevent IKlock
            if (dirToIKT.normalized == bones[bones.Length - 2].forward)
                bonesPos[bones.Length - 2] += pole * 0.1f;

            //iterate to accsepteble position
            for (int itt = 0; itt < iterations; itt++)
            {
                //Inverse K
                for (int y = bonesPos.Length - 2; y > 0; y--)
                {
                    //If dir = iktdir
                    bonesPos[y] = bonesPos[y + 1] + (bonesPos[y] - bonesPos[y + 1]).normalized * bonesMag[y];
                }

                //Forward K
                for (int y = 1; y < bones.Length; y++)
                {
                    bonesPos[y] = bonesPos[y - 1] + (bonesPos[y] - bonesPos[y - 1]).normalized * bonesMag[y - 1];
                }

                //In delta??
                if ((bonesPos[bonesPos.Length - 1] - IKTarget.position).sqrMagnitude < minDelta * minDelta)
                    break;
            }

        }

        //rotate (move) pos towards to Pole
        //if (pole != null)
        //{
            for (int i = 1; i < bonesPos.Length - 1; i++)
            {
                Plane plane = new Plane(bonesPos[i + 1] - bonesPos[i - 1], bonesPos[i - 1]);
                Vector3 projectedPole = plane.ClosestPointOnPlane(bonesPos[i] + pole * 10);
                Vector3 projectedBone = plane.ClosestPointOnPlane(bonesPos[i]);
                float angle = Vector3.SignedAngle((projectedBone - bonesPos[i - 1]).normalized, (projectedPole - bonesPos[i - 1]).normalized, plane.normal);

                bonesPos[i] = Quaternion.AngleAxis(angle, plane.normal) * (bonesPos[i] - bonesPos[i - 1]) + bonesPos[i - 1];
            }
        //}

        for (int x = 0; x < bones.Length - 1; x++)
        {
            bones[x].position = bonesPos[x];
            //bones[x].LookAt(bonesPos[x + 1]);

            bones[x].rotation = Quaternion.LookRotation(bonesPos[x + 1] - bonesPos[x], root.position - foot.position);

            Debug.DrawLine(bonesPos[x], bonesPos[x + 1],Color.blue);
        }
    }
}
