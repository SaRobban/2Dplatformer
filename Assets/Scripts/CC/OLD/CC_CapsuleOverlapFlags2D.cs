using UnityEngine;
public class CC_CapsuleOverlapFlags2D
{
    [Header("Flags")]
    private bool grounded;
    private bool hittingWall;
    private bool hittingWallLeft;
    private bool hittingWallRight;
    private bool hittingRoof;

    public bool Grounded => grounded;
    public bool HittingWall => hittingWall;
    public bool HittingWallLeft => hittingWallLeft;
    public bool HittingWallRight => hittingWallRight;
    public bool HittingRoof => hittingRoof;

    private CapsuleCollider2D capsuleCollider;
    private float skinWitdh = 0.02f;
    private float slopeAngle;
   private float dotSlope;
   private ContactPoint2D[] cPoints = new ContactPoint2D[16];

    public CC_CapsuleOverlapFlags2D(CapsuleCollider2D capsuleCollider)
    {
        this.capsuleCollider = capsuleCollider;
        DotSlope();
    }

    private void DotSlope()
    {
        float hiOff = capsuleCollider.size.y - capsuleCollider.size.x;
        Vector2 hiPoint = Quaternion.Euler(0, 0, slopeAngle) * Vector3.up * capsuleCollider.size.x * 0.5f;
        hiPoint.y += hiOff * 0.5f;

        Debug.DrawRay(capsuleCollider.transform.position, hiPoint, Color.red, 5);

        dotSlope = Vector2.Dot(Vector2.up, hiPoint);
    }

    public void CheckColliderFlags()
    {
        grounded = false;
        hittingWall = false;
        hittingWallLeft = false;
        hittingWallRight = false;
        hittingRoof = false;

        int hits = capsuleCollider.GetContacts(cPoints);
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
            Debug.DrawRay((Vector3)cPoint.point + Vector3.forward * -1, cPoint.normal, Color.green);
            grounded = true;
            return;
        }

        if (checkGroundOnly)
        {
            return;
        }

        if (dot > -dotSlope)
        {
            Debug.DrawRay((Vector3)cPoint.point + Vector3.forward * -1, cPoint.normal, Color.yellow);
            hittingWall = true;
            if (Vector2.Dot(Vector2.right, cPoint.normal) < 0)
            {
                hittingWallRight = true;
            }
            else
            {
                hittingWallLeft = true;
            }

            return;
        }
        else
        {
            Debug.DrawRay((Vector3)cPoint.point + Vector3.forward * -1, cPoint.normal, Color.red);
            hittingRoof = true;
            return;
        }
    }

    public float GetSkinWitdh()
    {
        return skinWitdh;
    }
}
