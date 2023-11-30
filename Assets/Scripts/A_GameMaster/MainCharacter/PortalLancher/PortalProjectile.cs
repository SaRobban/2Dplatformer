using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalProjectile : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] PortalLancher launcher;
    [SerializeField] float fitOffsett = 0.03125f;

    public void Launch(Vector2 pos, Vector2 dir, PortalLancher launcher)
    {
        transform.position = pos + dir;
        transform.up = dir;
        this.launcher = launcher;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, step, launcher.hitMask);
        Physics2D.queriesHitTriggers = true;
        if (hit.collider)
        {
            if (hit.collider.name == "PortalOne")
            {
                Debug.Log("HitPortal");
                transform.position = launcher.portalTwo.transform.position + launcher.portalTwo.transform.up;
                transform.up = launcher.portalTwo.transform.up;
                return;
            }
            if (hit.collider.name == "PortalTwo")
            {
                Debug.Log("HitPortal2");
                transform.position = launcher.portalOne.transform.position + launcher.portalOne.transform.up;
                transform.up = launcher.portalOne.transform.up;
                return;
            }

            OnHit(hit);
            return;
        }
        transform.position += transform.up * step;
    }

    public void OnHit(RaycastHit2D hit)
    {
        transform.position = hit.point + hit.normal * fitOffsett;
        transform.up = hit.normal;

        if (DoesFit())
            launcher.CreatePortalAt(transform, hit.collider);
        else
            launcher.FailedToCreatePortalAt(transform);

        gameObject.SetActive(false);
    }

    bool DoesFit()
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position, -transform.right, launcher.portalHeight, launcher.hitMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position, transform.right, launcher.portalHeight, launcher.hitMask);

        if (hitL.collider && hitR.collider)
        {
            float space = (hitL.point - hitR.point).sqrMagnitude;
            if (space < launcher.portalHeight * launcher.portalHeight)
                return false;
        }

        if (hitL.collider)
        {
            float distL = hitL.distance - (launcher.portalHeight * 0.5f);
            if (distL < 0)
                transform.position -= transform.right * distL;
        }

        if (hitR.collider)
        {
            float distR = hitR.distance - (launcher.portalHeight * 0.5f);
            if (distR < 0)
                transform.position += transform.right * distR;
        }
        return true;
    }
}
