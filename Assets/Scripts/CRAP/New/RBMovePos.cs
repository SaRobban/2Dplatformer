using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovePos : MonoBehaviour
{

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
