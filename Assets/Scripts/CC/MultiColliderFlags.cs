using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiColliderFlags
{

    //    [Header("Flags")]
    public bool Grounded{ get; private set; }
    public bool HittingWall { get; private set; }
    public bool HittingWallLeft { get; private set; }
    public bool HittingWallRight { get; private set; }
    public bool HittingRoof { get; private set; }

    //Test
    private MainCharacter owner;
    public bool HitLeftWallHi { get; private set; }
    public bool HitRightWallHi { get; private set; }


    public Collider2D[] colliders;
    private float dotSlope = 0.707f;
    private ContactPoint2D[] cPoints = new ContactPoint2D[16];
    private Vector2 bounds;


    public MultiColliderFlags(Collider2D[] colliders, MainCharacter owner)
    {
        this.owner = owner;
        this.colliders = colliders;
        this.bounds = CalcBounds();
    }

    public Vector2 CalcBounds()
    {
        Vector2 newBounds = Vector2.zero;
        Vector2 b = Vector2.zero;
        foreach (Collider2D col in colliders)
        {
            b = col.offset + (Vector2)col.bounds.extents;
            if (newBounds.x < b.x)
                newBounds.x = b.x;

            if (newBounds.y < b.y)
                newBounds.y = b.y;
        }
        return newBounds;
    }

    public void CheckColliderFlags()
    {
        Grounded = false;
        HittingWall = false;
        HittingWallLeft = false;
        HittingWallRight = false;
        HittingRoof = false;

        HitLeftWallHi = false;
        HitRightWallHi = false;

        foreach (Collider2D col in colliders)
        {
            GetColliderFlags(col);
        }
    }

    private void GetColliderFlags(Collider2D col)
    {
        int hits = col.GetContacts(cPoints);
        for (int i = 0; i < hits; i++)
        {
            FlagByCollider(cPoints[i], cPoints[i].collider.usedByEffector);
        }
    }

    private void FlagByCollider(ContactPoint2D cPoint, bool checkGroundOnly)
    {
        Debug.DrawRay((Vector3)cPoint.point + Vector3.back, cPoint.normal, Color.white, 0.1f);
        float dot = Vector2.Dot(Vector2.up, cPoint.normal);
        if (dot > dotSlope)
        {
            Debug.DrawRay((Vector3)cPoint.point + Vector3.forward * -1, cPoint.normal, Color.green);
            Grounded = true;
            return;
        }

        if (checkGroundOnly)
        {
            return;
        }

        if (dot > -dotSlope)
        {
            Debug.DrawRay((Vector3)cPoint.point + Vector3.forward * -1, cPoint.normal, Color.yellow);
            HittingWall = true;
            if (Vector2.Dot(Vector2.right, cPoint.normal) < 0)
            {
                HittingWallRight = true;
                if (cPoint.point.y > owner.GetPosition().y + owner.stats.HangHeight)
                {
                    HitRightWallHi = true;
                }
            }
            else
            {
                HittingWallLeft = true;
                if (cPoint.point.y > owner.GetPosition().y + owner.stats.HangHeight)
                {
                    HitLeftWallHi = true;
                }

            }

            return;
        }
        else
        {
            Debug.DrawRay((Vector3)cPoint.point + Vector3.forward * -1, cPoint.normal, Color.red);
            HittingRoof = true;
            return;
        }
    }
}
