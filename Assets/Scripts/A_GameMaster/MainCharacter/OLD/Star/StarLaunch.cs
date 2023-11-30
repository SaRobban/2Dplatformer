using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLaunch : MonoBehaviour
{
   public Rigidbody2D ccRB;
   public GameObject starPreFab;
    MainCharacter cc;
    public float speed = 5;

    private void Start()
    {
        cc = GetComponentInParent<MainCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO : Velocity y
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject star = Instantiate(starPreFab,ccRB.transform.position + Vector3.up, Quaternion.identity);
            star.GetComponent<Rigidbody2D>().velocity = cc.aim.GetAim() * speed;
        }
    }
}
