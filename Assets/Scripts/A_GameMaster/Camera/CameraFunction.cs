using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFunction : MonoBehaviour
{
    private Vector3 realPos;
    [SerializeField] private Transform targetOne;
    [SerializeField] private Transform targetTwo;
    public enum CameraMode { TargetOne, TargetTwo, MoveTowards, Static }
    public CameraMode mode;

    public float moveSpeed = 6;
    public float behind = 4;
    public float maxWidth = 6;
    public float maxHeigth = 6;

    public System.Action aTargetReached;

    private void Start()
    {
        realPos = CameraManager.cameraTruck.position;
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        CameraUpdateSwitch(dt);
    }
    public void SetStatic()
    {
        mode = CameraMode.Static;
    }

    public void MoveFromTo(Vector3 tOne, Transform tTwo, float speed)
    {
        realPos = tOne;
        targetOne = tTwo;
        moveSpeed = speed;
        mode = CameraMode.MoveTowards;
    }

    public void TargetOne(Transform t, float speed = 6)
    {
        this.moveSpeed = speed;
        this.targetOne = t;
        mode = CameraMode.TargetOne;
    }

    public void TargetBetweenTwoTargets(Transform tOne, Transform tTwo, float speed = 6)
    {
        this.moveSpeed = speed;
        this.targetOne = tOne;
        this.targetTwo = tTwo;
        mode = CameraMode.TargetTwo;
    }

    void CameraUpdateSwitch(float dt)
    {
        switch (mode)
        {
            case CameraMode.TargetOne:
                TargetUpdateOne(dt);
                break;
            case CameraMode.TargetTwo:
                TargetUpdateTwo(dt);
                break;
            case CameraMode.MoveTowards:
                MoveTowardsTarget(dt);
                break;
            case CameraMode.Static:
                break;
            default:
                TargetUpdateOne(dt);
                break;
        }
    }

    void TargetUpdateOne(float dt)
    {
        Vector2 pos = CameraManager.cameraTruck.position;
        float dist = (pos - (Vector2)targetOne.position).sqrMagnitude;
        if (dist < 0.12f)
            return;
        float cammeraSpeed = dist * 0.25f;
        cammeraSpeed *= moveSpeed;

        CameraManager.cameraTruck.position = Vector3.MoveTowards(
            pos,
            targetOne.position - Vector3.forward * behind,
            cammeraSpeed * Time.deltaTime
            );
    }
    void TargetUpdateTwo(float dt)
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

    void MoveTowardsTarget(float dt)
    {
        realPos = Vector3.MoveTowards(realPos, targetOne.transform.position, moveSpeed * dt);
        transform.position = realPos;
        if ((realPos - targetOne.transform.position).sqrMagnitude < Mathf.Epsilon*2)
            aTargetReached?.Invoke();
    }
}
