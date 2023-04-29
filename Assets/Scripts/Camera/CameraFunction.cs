using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFunction : MonoBehaviour
{
    public static CameraFunction instance;
    private Transform targetOne;
    private Transform targetTwo;
    public enum CameraMode { TargetOne, TargetTwo }
    public CameraMode mode;

    public float moveSpeed = 6;
    public float behind = 4;
    public float maxWidth = 6;
    public float maxHeigth = 6;

    public bool follow = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (follow)
            CameraUpdateSwitch();
    }

    public void TargetOne(Transform t)
    {
        this.targetOne = t;
        mode = CameraMode.TargetOne;
    }

    public void TargetBetweenTwoTargets(Transform tOne, Transform tTwo)
    {
        this.targetOne = tOne;
        this.targetTwo = tTwo;
        mode = CameraMode.TargetTwo;
    }

    void CameraUpdateSwitch()
    {
        switch (mode)
        {
            case CameraMode.TargetOne:
                TargetUpdateOne();
                break;
            case CameraMode.TargetTwo:
                TargetUpdateTwo();
                break;
            default:
                TargetUpdateOne();
                break;
        }
    }

    void TargetUpdateOne()
    {
        float dist = ((Vector2)transform.position - (Vector2)targetOne.position).sqrMagnitude;
        if (dist < 0.12f)
            return;
        float cammeraSpeed = dist * 0.25f;
        cammeraSpeed *= moveSpeed;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetOne.position - Vector3.forward * behind,
            cammeraSpeed * Time.deltaTime
            );
    }
    void TargetUpdateTwo()
    {
        Vector3 pos = (targetTwo.position - targetOne.position) * 0.5f;
        pos.x = Mathf.Clamp(pos.x, -maxWidth, maxWidth);
        pos.y = Mathf.Clamp(pos.y, -maxHeigth, maxHeigth);
        pos += targetOne.position;
        transform.position = Vector3.MoveTowards(
            transform.position,
            pos - Vector3.forward * behind,
            moveSpeed * Time.deltaTime
            );
    }
}
