using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public AudioSource source0;
    public AudioSource source1;

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float t0 = Mathf.Lerp(1, 0, rb.velocity.x);
        source0.volume = Mathf.MoveTowards(source0.volume, t0, 2 * Time.deltaTime);

        float t1 = Mathf.Lerp(0, 1, rb.velocity.x);
        source1.volume = Mathf.MoveTowards(source1.volume, t1, 2 * Time.deltaTime);
    }
}
