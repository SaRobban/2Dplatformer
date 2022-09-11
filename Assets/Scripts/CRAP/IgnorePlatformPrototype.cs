using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WARNING : Prototype
//TODO : Eventbased
//Script to make it posseble to jumpdown from platforms
//Idea : Make a state rb move trogh;
[RequireComponent(typeof(MainCharacter))]
public class IgnorePlatformPrototype : MonoBehaviour
{
    public int IgnorePaltform;
    private int defaultLayer;
    MainCharacter mainC;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        mainC = gameObject.GetComponent<MainCharacter>();
        rb = gameObject.GetComponent<Rigidbody2D>();
            print(mainC.gameObject.layer);
        defaultLayer = mainC.gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainC.input.Axis.y < -0.5f)
        {
            print("Telefrag");
            transform.position = (rb.position + Vector2.down * 2);
        }
        
    }
}
