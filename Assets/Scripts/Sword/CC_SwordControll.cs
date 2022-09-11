using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_SwordControll : MonoBehaviour
{
    public bool hasSword = true;
    public bool keyToggle;
    public bool flipX;

    public float slashCoolDown = 0.5f;
    private float coolDownLeft;
    public float chargeTime = 1;
    public float chargeTimeLeft;

    [Header("SwordStages")]
    public GameObject slash;
    public GameObject flying;
    public GameObject stuck;
    public GameObject bounce;
    public GameObject flyBack;

    private GameObject activeSword;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coolDownLeft -= Time.deltaTime;
        chargeTimeLeft -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
           
            chargeTimeLeft = chargeTime;

            if (hasSword)
            {
                if (slashCoolDown < 0)
                {
                    Slash();
                    coolDownLeft = slashCoolDown;
                }
            }
            else
            {
                SwordReturn();
            }
        }


        if (Input.GetButton("Fire1") && hasSword)
        {
            //Special FX
            print("1");
            if (chargeTimeLeft < 0 && keyToggle == false)
            {
                keyToggle = true;
                print("2");
                Trow();
                hasSword = false;
            }
        }

        if (Input.GetButtonUp("Fire1") )
        {
            keyToggle = false;
            /*
            if(!hasSword)
                SwordReturn();
                */
        }

    }

    public void ChangeState(string comand)
    {

        if(comand == "Stuck")
        {
            SwordStuck();
        }
        if (comand == "Bounce")
        {
            Bounce();
        }
        if(comand == "FlyBack")
        {
            SwordReturn();
        }

        if(comand == "End")
        {
            SwordEnd();
        }
    }

    private void Slash()
    {
        print("slash");
        //activeSword = Instantiate(slash, transform.position, transform.rotation);
        //activeSword.GetComponent<SwordFlying>().swc = this;
    }

    private void Trow()
    {
        Vector3 dir = Vector3.forward;
        if (GetComponentInChildren<SpriteRenderer>().flipX == true)
        {
            dir = Vector3.back;
        }


        print("Trow");
        activeSword = Instantiate(flying, transform.position, Quaternion.LookRotation(dir, Vector3.up));
        activeSword.GetComponent<SwordFlying>().swc = this;
    }

    private void SwordStuck()
    {
        Vector3 pos = activeSword.transform.position;
        Quaternion rot = activeSword.transform.rotation;
        Destroy(activeSword);
        print("stuck");
        activeSword = Instantiate(stuck, pos, rot);
        activeSword.GetComponent<SwordPlatform>().swc = this;
    }

    private void Bounce()
    {
        Vector3 pos = activeSword.transform.position;
        Quaternion rot = activeSword.transform.rotation;
        print("bounce");
        Destroy(activeSword);
        activeSword = Instantiate(bounce, pos, rot);
        activeSword.GetComponent<SwordBend>().swc = this;
    }
    private void SwordReturn()
    {
        Vector3 pos = activeSword.transform.position;
        Quaternion rot = activeSword.transform.rotation;
        print("Return");
        Destroy(activeSword);
        activeSword = Instantiate(flyBack, pos, rot);
        activeSword.GetComponent<SwordFlyHome>().swc = this;
        activeSword.GetComponent<SwordFlyHome>().target = transform;
    }

    private void SwordEnd()
    {
        print("End");
        Destroy(activeSword);
        hasSword = true;
        chargeTimeLeft = chargeTime;
    }

    private void ChargeFX()
    {
        /*
        if (sfxObj == null)
        {
            sfxObj = Instantiate(sfxParticle, transform.position + Random.insideUnitSphere * 2, transform.rotation);
        }
        else
        {
            float pSpeed = Mathf.Clamp(1 - chargeTime, 0.1f, 1);
            sfxObj.transform.position = Vector3.MoveTowards(sfxObj.transform.position, transform.position, pSpeed * 10 * Time.deltaTime);
            if (sfxObj.transform.position == transform.position)
            {
                Destroy(sfxObj);
            }
        }

        chargeTime -= Time.deltaTime;
        */
    }
}
