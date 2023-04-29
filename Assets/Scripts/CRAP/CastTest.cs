using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastTest : MonoBehaviour
{
    float time = -10;
    float trigger = 0.5f;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (time < 0)
        {
            anim.Play(0);
            SPECIALFX.Command.Fire("FX_Magic", transform.position+ Vector3.one * 0.1f, Vector3.up);
            time = 0;
        }

      
        if(time > 3)
        {
            time = -10;
        }

        time += Time.deltaTime;
    }

}
