using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaterManager : MonoBehaviour
{
    private Collider2D col;
    private FluidSurface fluid;
    private void Start()
    {
        col = GetComponent<Collider2D>();
        fluid = GetComponent<FluidSurface>();
        fluid.Init(col.bounds.extents);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 point = col.ClosestPoint(collision.transform.position);
        Vector2 impact = collision.attachedRigidbody.velocity * 0.25f;
        float size = collision.bounds.extents.x;
        fluid.Impact(point, impact.y, size);
        Debug.DrawRay(point, Vector3.down * impact * size * 4, Color.red, 10);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 point = col.ClosestPoint(collision.transform.position);
        Vector2 impact = Vector3.down * collision.attachedRigidbody.velocity.magnitude * 0.025f;
        float size = collision.bounds.extents.x;

        Debug.DrawRay(point, Vector3.up, Color.green);
        fluid.Impact(point, impact.y, size);

    }

    private void Update()
    {
        fluid.UpdateFluid(Time.deltaTime);
    }
}
