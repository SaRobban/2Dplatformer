using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelsnapFollow : MonoBehaviour
{
    [SerializeField] private int pixelPerUnit = 32;
    private float step;


    // Start is called before the first frame update
    void Start()
    {
        step = 1f / (float)pixelPerUnit;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x = Mathf.CeilToInt(transform.position.x * (float)pixelPerUnit);
        x *= step;
        float y = Mathf.CeilToInt(transform.position.y * (float)pixelPerUnit);
        y *= step;
        transform.position = Vector3.right * x + Vector3.up * y + Vector3.forward * transform.position.z;
    }
}
