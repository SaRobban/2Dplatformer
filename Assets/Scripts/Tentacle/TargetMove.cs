using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.up * 5 + new Vector3(Mathf.Sin(Time.time*0.25f)*3, Mathf.Cos(Time.time*0.25f), 0) * 3;
    }
}
