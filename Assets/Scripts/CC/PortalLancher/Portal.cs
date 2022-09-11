using UnityEngine;

public class Portal : MonoBehaviour
{
    Collider2D triggerBox;
    Portal sisterPortal;
    public Collider2D disableCollider;
    float kickOutStr;

    public Bounds GetBoxBounds()
    {
        if (triggerBox == null)
            triggerBox = GetComponent<Collider2D>();

        return triggerBox.bounds;
    }
    public void Create(Transform place, Collider2D onCollider, Portal sister, float kickOutStr)
    {
        transform.position = place.position;
        transform.rotation = place.rotation;
        disableCollider = onCollider;
        sisterPortal = sister;
        this.kickOutStr = kickOutStr;

        transform.parent = null;
        transform.localScale = Vector3.one;
        transform.parent = onCollider.transform;
        gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D colliderEnter)
    {
        if (!sisterPortal.gameObject.activeSelf)
            return;

        if (!colliderEnter.TryGetComponent(out Rigidbody2D otherRB))
            return;

        if (!otherRB.simulated)
            return;


        DisableCollisionToParent(colliderEnter);

        if (!ShouldTeleport(colliderEnter, otherRB))
        {
            return;
        }

        Physics2D.IgnoreCollision(colliderEnter, sisterPortal.disableCollider, true);

        otherRB.velocity = sisterPortal.transform.up * kickOutStr;

        colliderEnter.transform.position = (Vector2)sisterPortal.transform.position + CompansateOffCenter(colliderEnter); //ReflectPos(dir);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetCollision(collision);
    }
    private bool ShouldTeleport(Collider2D enterCollider, Rigidbody2D otherRB)
    {
        Vector2 dir = enterCollider.transform.position - transform.position;
        float dotPos = Vector2.Dot(transform.up, dir);
        float dotVelo = Vector2.Dot(transform.up, otherRB.velocity);
        if (dotPos < 0 && dotVelo < 0)
        {
            return true;
        }
        return false;
    }
    private Vector2 CompansateOffCenter(Collider2D col)
    {
        Vector2 dir = col.transform.position - col.bounds.center;
        return dir;
    }
    private void DisableCollisionToParent(Collider2D colliderEnter)
    {
        if ((colliderEnter.bounds.center - transform.position).sqrMagnitude < 1)
            Physics2D.IgnoreCollision(colliderEnter, disableCollider, true);
        else
            Physics2D.IgnoreCollision(colliderEnter, disableCollider, false);
    }

    private void ResetCollision(Collider2D colliderEnter)
    {
        Physics2D.IgnoreCollision(colliderEnter, disableCollider, false);
    }
}
