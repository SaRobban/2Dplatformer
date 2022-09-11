using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField] private Transform[] tailParts;
    [SerializeField] private float partDist;

    [SerializeField] private float tailG = 2;
    [SerializeField] private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check dist to prev

        Vector3 prevPos = transform.position + offset;

        for(int i = 0; i < tailParts.Length; i++)
        {
            //Apply gravity
            tailParts[i].position += Vector3.down * tailG * Time.deltaTime;
            
            Vector3 dir = tailParts[i].position - prevPos;
            float dist = dir.sqrMagnitude;

            if(dist > (partDist * (i+1))*(partDist * (i + 1)))
            {
                tailParts[i].position = prevPos + dir.normalized * partDist * (i+1);
            }

            prevPos = tailParts[i].position;
        }
    }
}
