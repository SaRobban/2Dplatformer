using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBend : MonoBehaviour
{
    public float timeOut = 0.6f;
    public CC_SwordControll swc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        timeOut -= Time.deltaTime;
        if(timeOut < 0)
            swc.ChangeState("FlyBack");
    }
}
