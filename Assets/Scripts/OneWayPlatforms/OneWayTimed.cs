using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayTimed : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private GameObject front;
    [SerializeField] private Rigidbody2D[] fallFragmentsOrder;
    private Collider2D platformCollider;
    [Header("Settings")]
    [SerializeField] private float timeTillFall = 0.5f;
    [SerializeField] private float timeTillReset = 3f;
    [SerializeField] private float RandomRotStr = 360f;
    private bool isRunning = false;
    private Vector2 size;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If not activated and hit on top
        if (isRunning == false)
        {
            if (collision.GetContact(0).normal.y <= -0.7f)
            {
                StartCoroutine(Fall());
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        size = platformCollider.bounds.extents;
        size.x -= 0.1f;
        size.y -= 0.1f;
    }

    IEnumerator Fall()
    {
        isRunning = true;
        //Sets front to false then drops fragments one by one
        int i = 0;
        float timeDiv = fallFragmentsOrder.Length;
        front.SetActive(false);

        for (int o = 0; o < fallFragmentsOrder.Length; o++)
        {
            fallFragmentsOrder[o].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(timeTillFall / timeDiv);

        while (i <= fallFragmentsOrder.Length - 1)
        {
            yield return new WaitForSeconds(timeTillFall / timeDiv);
            fallFragmentsOrder[i].isKinematic = (false);
            fallFragmentsOrder[i].AddTorque(Random.Range(-RandomRotStr, RandomRotStr));
            i++;
        }

        platformCollider.enabled = false;

        StartCoroutine(ResetPlatform());
    }

    IEnumerator ResetPlatform()
    {
        yield return new WaitForSeconds(timeTillReset);

        while (Physics2D.OverlapBox(transform.position, size * 2, transform.rotation.z) != null)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForFixedUpdate();

        for (int i = 0; i < fallFragmentsOrder.Length; i++)
        {
            fallFragmentsOrder[i].isKinematic = true;
            fallFragmentsOrder[i].transform.position = transform.position;
            fallFragmentsOrder[i].transform.rotation = transform.rotation;
            fallFragmentsOrder[i].gameObject.SetActive(false);
        }
        front.SetActive(true);
        platformCollider.enabled = true;
        isRunning = false;
    }
}
