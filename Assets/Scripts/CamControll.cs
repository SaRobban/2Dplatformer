// Smooth towards the target

using UnityEngine;
using System.Collections;

public class CamControll : MonoBehaviour
{
    [Header("Stuff")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    public Vector2 screenUnitBound = new Vector3(4,6);
    private Vector2 tPos;
    private Vector2 endPos;


    [Header("Restrict")]
    public bool lockX;
    public bool lockY;
    public bool forcePosX;
    public bool forcePosY;
    public Vector2 forcePos;

    [Header("MoveTowards")]
    public bool moveTowardsXY = false;
    public bool moveTowardsX = false;
    public bool moveTowardsY = false;
    public float moveSpeed = 0.3f;
    private Vector2 smoothV;


    [Header("PixelSnap")]
    public bool pixelSnap = true;
    public int pixelPerMeter = 32;

    [Header("Other")]
    public float impForce;

    private void Start()
    {
        tPos = target.position;
        endPos = target.position;
    }
    private void Update()
    {
        tPos = target.position;

        //Restrictions
        if (lockX)
            tPos.x = transform.position.x;

        if (lockY)
            tPos.y = transform.position.y;

        if (forcePosX)
            tPos.x = forcePos.x;

        if (forcePosY)
            tPos.y = forcePos.y;


        //offSide
        Vector2 maxOffside = endPos + screenUnitBound;
        Vector2 minOffside = endPos - screenUnitBound;

        if (tPos.x < minOffside.x)
            endPos.x = tPos.x + screenUnitBound.x;
        else if (tPos.x > maxOffside.x)
            endPos.x = tPos.x - screenUnitBound.x;

        if (tPos.y < minOffside.y)
            endPos.y = tPos.y + screenUnitBound.y;
        else if (tPos.y > maxOffside.y)
            endPos.y = tPos.y - screenUnitBound.y;

        //smooth
        if (moveTowardsXY)
        {
            endPos = Vector2.MoveTowards(endPos, tPos, moveSpeed * Time.deltaTime);
        }
        else if (moveTowardsX)
        {
            endPos.x = Mathf.MoveTowards(endPos.x, tPos.x, moveSpeed * Time.deltaTime);
            endPos.y = tPos.y;
        }
        else if (moveTowardsY)
        {
            endPos.x = tPos.x;
            endPos.y = Mathf.MoveTowards(endPos.y, tPos.y, moveSpeed * Time.deltaTime);
        }
        else
        {
            endPos = tPos;
        }


        if (impForce > 0)
        {
            endPos = tPos;
            endPos += Random.insideUnitCircle * impForce;
            impForce -= 4* Time.deltaTime;
            
        }
            
        //Snap        
        if (pixelSnap)
            endPos = PixelSnap(endPos);

        

        transform.position = endPos;
        transform.position += offset;

        ///Destrorwsgfhgjösj
        transform.position = new Vector3(target.position.x , target.position.y , offset.z);
        
    }

    Vector2 PixelSnap(Vector2 pos)
    {
        Vector3 pixSnap = pos * pixelPerMeter;
        pixSnap.x = Mathf.RoundToInt(pixSnap.x);
        pixSnap.y = Mathf.RoundToInt(pixSnap.y);

        pixSnap /= pixelPerMeter;

        return pixSnap;
    }

    void Impact(float force)
    {
        
    }
}
