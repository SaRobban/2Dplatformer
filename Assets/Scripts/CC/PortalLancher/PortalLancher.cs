using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLancher : MonoBehaviour
{
    [SerializeField] private MainCharacter mainCharacter;
    [SerializeField] private bool even;

    [SerializeField] private PortalProjectile projectile;

    public LayerMask hitMask;
    public float portalHeight = 2;


    [SerializeField] public Portal portalOne;
    [SerializeField] public Portal portalTwo;
    [SerializeField] private float kickOutStr = 10;

    private void Awake()
    {
        mainCharacter = GetComponentInParent<MainCharacter>();
        projectile = GetComponentInChildren<PortalProjectile>();

        Portal[] portals = GetComponentsInChildren<Portal>();
        portalOne = portals[0];
        portalTwo = portals[1];

        projectile.transform.parent = null;
        portalOne.transform.parent = null;
        portalTwo.transform.parent = null;


        projectile.gameObject.SetActive(false);
        portalOne.gameObject.SetActive(false);
        portalTwo.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Launch();
        }
    }

    public void Launch()
    {
        if (mainCharacter == null)
            mainCharacter = GetComponentInParent<MainCharacter>();

        if(mainCharacter.input.Axis.y !=0)
        {
            if (mainCharacter.input.Axis.y > 0)
            {
                projectile.Launch(transform.position, mainCharacter.transform.up, this);
                return;
            }
            if (mainCharacter.input.Axis.y < 0)
            {
                projectile.Launch(transform.position, -mainCharacter.transform.up, this);
                return;
            }
        }
        if (mainCharacter.sprite.flipX)
            projectile.Launch(transform.position, -mainCharacter.transform.right, this);
        else
            projectile.Launch(transform.position, mainCharacter.transform.right, this);
    }

    public void CreatePortalAt(Transform t, Collider2D collider)
    {
        Debug.Log("PlaceTeleporter");
        if (even)
        {
            portalTwo.Create(t, collider, portalOne, kickOutStr);
            even = !even;
        }
        else
        {
            portalOne.Create(t, collider, portalTwo, kickOutStr);
            even = !even;
        }
    }

    public void FailedToCreatePortalAt(Transform t)
    {
       
        Debug.Log("FailedToPlaceTeleporter");

    }
}
