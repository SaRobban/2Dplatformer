/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectArea : MonoBehaviour
{
    private float maxCastDist = 10;
    private float skinWidth = 0.6f;
    private float acceptableHeightDiff = 0.1f;


    private bool canJump = true;
    private float jumpDist = 3;
    private float jumpHeight = 3;

    public struct Bounds
    {
        public float minX;
        public float maxX;
        public float height;
        public bool jumpLeft;
        public bool jumpRight;
    }

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down);
        if (groundHit.collider == null)
            return;

        Vector2 leftestCorner = CornerStep(true, groundHit.collider);
        Vector2 rightestCorner = CornerStep(false, groundHit.collider);

        Debug.DrawRay(leftestCorner, Vector3.up, Color.green, 10);
        Debug.DrawRay(rightestCorner, Vector3.up, Color.red, 10);

        JumpCheck(true, 5, leftestCorner);
    }

    private Vector2 CornerStep(bool left, Collider2D start)
    {
        Vector2 startCorner = GetCorner(left, start);
        Vector2 result = startCorner;
        Collider2D startCollider = start;

        int itt = 0;
        while (itt < 100)
        {
            Collider2D newStart = GetXFromAdjustedNeighbours(left, startCollider, startCorner, out result);
            if (startCollider != newStart)
                startCollider = newStart;
            else
                break;

            print(left ? "LEFT" : "RIGHT" + itt);
            itt++;
        }
        return result;
    }

    private Collider2D GetXFromAdjustedNeighbours(bool left, Collider2D fromCollider, Vector2 startCorner, out Vector2 resultingCorner)
    {
        resultingCorner = startCorner;
        Collider2D resultCollider = fromCollider;

        Collider2D[] overlaps = Physics2D.OverlapBoxAll(
               fromCollider.transform.position,
               new Vector2(fromCollider.bounds.extents.x * 2 + skinWidth, fromCollider.bounds.extents.y * 2 + skinWidth),
               fromCollider.transform.rotation.eulerAngles.z
               );



        foreach (Collider2D overlapingCollider in overlaps)
        {
            Debug.DrawRay(overlapingCollider.transform.position, Vector3.up, Color.white * 0.5f, 10);

            Vector2 overlappingCorner = GetCorner(left, overlapingCollider);

            if (WithinAcceptableHeightDiff(overlappingCorner.y, startCorner.y))
            {
                if (left)
                {
                    if (overlappingCorner.x < resultingCorner.x)
                    {
                        resultingCorner = overlappingCorner;
                        resultCollider = overlapingCollider;
                    }
                }
                else
                {
                    if (overlappingCorner.x > resultingCorner.x)
                    {
                        resultingCorner = overlappingCorner;
                        resultCollider = overlapingCollider;
                    }
                }
            }
        }

        return resultCollider;
    }

    private Vector2 GetCorner(bool left, Collider2D collider)
    {
        int dir = 1;
        if (left)
            dir = -1;

        Vector2 corner = collider.ClosestPoint(
            collider.transform.position +
            Vector3.up * 100 +
            Vector3.right * dir * 100
            );

        return corner;
    }
    public bool WithinAcceptableHeightDiff(float height, float ground)
    {
        float heigthDiff = height - ground;
        if (Mathf.Abs(heigthDiff) < acceptableHeightDiff)
        {
            return true;
        }
        return false;
    }


    public void JumpCheck(bool left, float jumpRange, Vector2 start)
    {
        Vector2 dir = Vector2.right * jumpRange;
        if (left)
            dir *= -1;

        RaycastHit2D[] hits =
            Physics2D.CircleCastAll(
                start + dir,
                jumpRange * 0.5f,
                Vector2.down
        );

        foreach (RaycastHit2D hit in hits)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red, 10);
        }
    }
}
*/