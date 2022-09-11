using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePespektiv : MonoBehaviour
{
    public float moveAmount = 0.5f;
    public Transform cam;
    private Vector3 orgOffset;
    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        orgOffset = cam.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newpos = orgOffset + cam.position * moveAmount;
        transform.position = newpos;

    }
}
