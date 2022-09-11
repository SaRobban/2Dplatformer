using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSpawner : MonoBehaviour
{
    public GameObject smoke;
    public string clip;
    void Umpfh(Vector2 pos)
    {
        smoke.SetActive(true);
        smoke.transform.position = pos;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
