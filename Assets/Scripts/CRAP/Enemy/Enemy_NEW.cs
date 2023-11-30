using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_NEW : MonoBehaviour, IEnemyStateMachine
{
    IEnemyState currentState;

    public SpriteRenderer sprite { get; set; }
    public Rigidbody2D rb { get; set; }

    [SerializeField]
    public LayerMask groundLayer;
    class CollisionFlags
    {
        IEnemyStateMachine owner;

        bool hit;
        bool grounded;
        bool right;
        bool left;
        bool top;

        private float skinWitdh = 0.02f;
        private float dotSlope = 0.707f;
        private Vector3 hiWallPoint = new Vector3(0, 0.9f, 0);
        private ContactPoint2D[] cPoints = new ContactPoint2D[16];
        void UpdateFlags(Collider2D collider)
        {
            ResetFlags();
            int hits = collider.GetContacts(cPoints);
            for (int i = 0; i < hits; i++)
            {
                FlagByCollider(cPoints[i]);
            }
        }

        private void ResetFlags()
        {
            hit = false;
            grounded = false;
            right = false;
            left = false;
            top = false;
        }

        private void FlagByCollider(ContactPoint2D cPoint)
        {
            hit = true;
            float dot = Vector2.Dot(Vector2.up, cPoint.normal);
            if (dot > dotSlope)
            {
                grounded = true;
                return;
            }


            if (dot > -dotSlope)
            {
                Vector2 checkPoint = owner.sprite.transform.position;
                Vector2 closestPoint = cPoint.collider.ClosestPoint(checkPoint);
                if (Vector2.Dot(Vector2.right, cPoint.normal) < 0)
                {
                    right = true;
                }
                else
                {
                    left = true;
                }
                return;
            }
            else
            {
                top = true;
                return;
            }


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sprite == null)
        {
            if (transform.GetChild(0).TryGetComponent(out SpriteRenderer sr))
            {
                sprite = sr;
            }
        }
        if (rb == null)
            if (TryGetComponent(out Rigidbody2D rb))
                this.rb = rb;

        ChangeState(new EnemyMove(this));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateState(Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Player")
        {
            Vector2 dir = PlayerMain.mainCharacter.transform.position - transform.position;
            Debug.DrawRay(transform.position, dir * 10, Color.magenta, 2);

            if (Vector2.Dot(dir.normalized, Vector2.up) > 0.707f)
            {
                Debug.Log("Kill Enemy");
                collision.transform.parent = transform;
            }
            else
                PlayerMain.mainCharacter.AddDamage(1);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.transform.parent == transform)
        {
            collision.transform.parent = null;
        }
    }
    
    public void UpdateState(float time)
    {
        {
            if (currentState == null)
                return;
            currentState.OnUpdate(Time.deltaTime);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.OnExit();

        currentState = newState;
        currentState.OnEnter(this);
    }
}

public interface IEnemyStateMachine
{
    public SpriteRenderer sprite { get; set; }
    public Rigidbody2D rb { get; set; }

    public void UpdateState(float time);
    public void ChangeState(IEnemyState newState);
}

public interface IEnemyState
{
    public Enemy_NEW owner { get; set; }
    public void OnEnter(Enemy_NEW owner);
    public void OnUpdate(float deltaT);
    public void OnExit();
}
class EnemyMove : IEnemyState
{
    public Enemy_NEW owner { get; set; }

    public Vector3 dir;

    public EnemyMove(Enemy_NEW owner)
    {
        this.owner = owner;
    }
    public void OnEnter(Enemy_NEW owner)
    {
        this.owner = owner;

        dir = Vector2.left;
        if (owner.sprite.flipX)
            dir = Vector2.right;
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate(float deltaT)
    {
        RaycastHit2D hit = Physics2D.Raycast(owner.transform.position + dir * 0.5f, Vector3.down, 0.2f, owner.groundLayer);

        if (hit.collider == null)
        {
            owner.sprite.flipX = !owner.sprite.flipX;
            dir *= -1;
        }
        //WARNING : I move by transform to make platforming work
        owner.transform.position = ((Vector2)owner.transform.position + (Vector2)dir * 2 * deltaT);
        Debug.DrawRay(owner.transform.position, dir, Color.red);
        Debug.DrawRay(owner.transform.position + dir * 0.5f, Vector3.down, Color.red);
    }
}