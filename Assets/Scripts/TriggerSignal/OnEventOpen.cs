using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEventOpen : MonoBehaviour
{

    public bool busy;
    public bool open;

    public Vector3 openPos;
    public Vector3 closedPos;

    // Start is called before the first frame update
    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.up * 2;

        GameEvents._instance.onEvent += ToggleDoor;
    }

    void ToggleDoor(Vector3 pos)
    {
        if (!busy && (transform.position - pos).sqrMagnitude < 10)
        {
            if (open)
                StartCoroutine(Close());
            else
                StartCoroutine(Open());
        }
    }

    private IEnumerator Open()
    {
        busy = true;
        while (transform.position != openPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPos, Time.deltaTime);
            yield return null;
        }
        open = true;
        busy = false;
    }

    private IEnumerator Close()
    {
        busy = true;
        while (transform.position != closedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPos, Time.deltaTime);
            yield return null;
        }
        open = false;
        busy = false;
    }
}
