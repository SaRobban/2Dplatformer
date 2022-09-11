using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    public Camera cam;
    public Vector3 offsett;
    public float para = 0.5f;

    private Vector3 orgPos;
    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        orgPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = cam.transform.position- orgPos;
        
        newPos *= para;
        transform.position = orgPos + newPos;
    }
}
