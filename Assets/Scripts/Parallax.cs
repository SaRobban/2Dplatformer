using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Vector2 center;
    Vector2 cameraDir;
    [SerializeField] float admount;
    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        cameraDir = center - (Vector2)Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = (Vector2)Camera.main.transform.position + cameraDir - center;
        transform.position = center + dir * admount;
    }
}
