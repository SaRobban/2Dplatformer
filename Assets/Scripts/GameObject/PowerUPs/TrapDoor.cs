using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : SensorReciverBase
{
    [Header("Open/Closed positions")]
    [Tooltip("Set to NULL to DISABLE OPEN")]
    [SerializeField] private Transform targetOpen;
    [Tooltip("Set to NULL to DISABLE CLOSE")]
    [SerializeField] private Transform targetClosed;
    private Vector3 openTarget;
    private Vector3 closedTarget;

    [SerializeField] private float openSpeed = 4;
    [SerializeField] private float closeSpeed = 2;


    private IEnumerator coroutine;

    private void Awake()
    {
        if (targetOpen != null)
            openTarget = targetOpen.position;

        if (targetClosed != null)
            closedTarget = targetClosed.position;
    }

    public override void Active()
    {
        if (targetOpen == null)
            return;

        if (coroutine != null)
            StopCoroutine(coroutine);

        if (openTarget == transform.position)
            return;

        coroutine = Open();
        StartCoroutine(coroutine);
    }

    public override void InActive()
    {
        if (targetClosed == null)
            return;

        if (coroutine != null)
            StopCoroutine(coroutine);

        if (closedTarget == transform.position)
            return;

        coroutine = Close();
        StartCoroutine(coroutine);
    }

    private IEnumerator Open()
    {
        while (transform.position != openTarget)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, openTarget, openSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Close()
    {
        while (transform.position != closedTarget)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, closedTarget, closeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
