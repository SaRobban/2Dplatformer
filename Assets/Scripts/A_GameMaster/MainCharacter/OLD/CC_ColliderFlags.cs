using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_ColliderFlags
{
    //[Header("Flags")]
    public bool Hit { get; private set; }
    public bool Grounded { get; private set; }
    public bool HittingWall { get; private set; }
    public bool HittingWallLeft { get; private set; }
    public bool HittingWallRight { get; private set; }
    public bool HittingRoof { get; private set; }

    //Test
    private Transform ownerT;
    public bool HitLeftWallHi { get; private set; }
    public bool HitRightWallHi { get; private set; }

    // public bool HitMoveble { get; private set; }

    private PolygonCollider2D collider;
    private float skinWitdh = 0.02f;
    private float dotSlope = 0.707f;
    private Vector3 hiWallPoint = new Vector3(0, 0.9f, 0);
    private ContactPoint2D[] cPoints = new ContactPoint2D[16];

    private Transform owner;

    public CC_ColliderFlags(PolygonCollider2D collider, Transform owner)
    {
        this.collider = collider;
        this.owner = owner;
    }

    public void CheckColliderFlags()
    {
        Hit = false;

        Grounded = false;
        HittingWall = false;
        HittingWallLeft = false;
        HittingWallRight = false;
        HittingRoof = false;

        HitLeftWallHi = false;
        HitRightWallHi = false;

        //  HitMoveble = false;

        int hits = collider.GetContacts(cPoints);
        for (int i = 0; i < hits; i++)
        {
            FlagByCollider(cPoints[i], cPoints[i].collider.usedByEffector);
        }
    }

    private void FlagByCollider(ContactPoint2D cPoint, bool checkGroundOnly)
    {


        float dot = Vector2.Dot(Vector2.up, cPoint.normal);
        if (dot > dotSlope)
        {
            //Debug.DrawRay((Vector3)cPoint.point + Vector3.forward * -1, cPoint.normal, Color.green);
            Hit = true;
            Grounded = true;
            return;
        }

        if (checkGroundOnly)
        {
            return;
        }

        if (dot > -dotSlope)
        {
            Hit = true;
            HittingWall = true;
            Vector2 checkPoint = owner.position + hiWallPoint;
            Vector2 closestPoint = cPoint.collider.ClosestPoint(checkPoint);
            checkPoint.y -= skinWitdh;

            if (Vector2.Dot(Vector2.right, cPoint.normal) < 0)
            {
                Hit = true;
                HittingWallRight = true;
                if (closestPoint.y > checkPoint.y)
                {
                    HitRightWallHi = true;
                }
            }
            else
            {
                Hit = true;
                HittingWallLeft = true;
                if (closestPoint.y > checkPoint.y)
                {
                    HitLeftWallHi = true;
                }
            }

            return;
        }
        else
        {
            Hit = true;
            Debug.DrawRay((Vector3)cPoint.point + Vector3.forward * -1, cPoint.normal, Color.red);
            HittingRoof = true;
            return;
        }
    }

    public float GetSkinWitdh()
    {
        return skinWitdh;
    }

    public PolygonCollider2D GetCollider()
    {
        return collider;
    }

    public Collider2D[] GetContactsAllocated(LayerMask mask)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(mask);

        List<Collider2D> hits = new List<Collider2D>();
        collider.OverlapCollider(contactFilter, hits);
        return hits.ToArray();
    }
}
