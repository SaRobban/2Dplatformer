using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDoor : MonoBehaviour
{
    [SerializeField]
    private PortalDoor otherPortal;
    private Collider2D attatchedToCollider;

    List<TeleportCloneController> teleportebles = new List<TeleportCloneController>();


    float fadeMultiplyer = 0.2f;
    float fade = 0;
    System.Action<float> update;

    [SerializeField] private SpriteRenderer[] spriteRenderer;
    [SerializeField] private Color[] orgCol;

    private List<Collider2D> ignoringCollision = new List<Collider2D>();

    bool fadeIn = false;
    bool fadeOut = false;


    private void Awake()
    {
        orgCol = new Color[spriteRenderer.Length];
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            orgCol[i] = spriteRenderer[i].color;
            spriteRenderer[i].color = Color.black *0;
        }
    }

    public void Connect(PortalDoor otherPortal)
    {
        this.otherPortal = otherPortal;
    }

    public void InitializeWarp(Vector2 point, Vector2 normal, Collider2D attatchedToCollider)
    {
        fade = 0;

        gameObject.SetActive(true);
        transform.position = point;
        transform.up = normal;
        transform.parent = attatchedToCollider.transform;
        this.attatchedToCollider = attatchedToCollider;

        if (fadeOut)
            update -= FadeOut;

        if (fadeIn)
            return;

        fadeIn = true;
        update += FadeIn;
    }

    public void KillWarp()
    {
        Disengagewarp();

        if (fadeIn)
            update -= FadeIn;

        if (fadeOut)
            return;

        fadeOut = true;
        update += FadeOut;
    }

    private void FadeIn(float deltaTime)
    {
        fade += deltaTime * fadeMultiplyer;
        for(int i =0; i< spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = orgCol[i] * fade;
        }

        if (fade < 1)
            return;

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = orgCol[i];
        }

        fade = 1;
        update -= FadeIn;
        fadeIn = false;
    }


    private void FadeOut(float deltaTime )
    {
        fade -= deltaTime * fadeMultiplyer;
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = orgCol[i] * fade;
        }

        if (fade > 0)
            return;

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = Color.black * 0;
        }

        gameObject.SetActive(false);

        fade = 1;
        update -= FadeOut;
        fadeOut = false;
    }

    private void Disengagewarp()
    {
        foreach(Collider2D col in ignoringCollision)
        {
            Physics2D.IgnoreCollision(attatchedToCollider, col, false);
        }
        ignoringCollision.Clear();
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TeleportCloneController teleporteble))
        {
            teleporteble.Show(transform, otherPortal.transform);
            teleportebles.Add(teleporteble);
        }
        Physics2D.IgnoreCollision(attatchedToCollider, collision, true);
        ignoringCollision.Add(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TeleportCloneController teleporteble))
        {
            teleporteble.Move();

            Vector3 dir = teleporteble.transform.position - transform.position;

            if (Vector2.Dot(transform.up, dir) < 0)
            {
                teleporteble.transform.position = otherPortal.transform.position + dir;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TeleportCloneController teleporteble))
        {
            teleporteble.Hide();
            teleportebles.Remove(teleporteble);
        }
        Physics2D.IgnoreCollision(attatchedToCollider, collision, false);
        ignoringCollision.Remove(collision);
    }

    private void Update()
    {
        update?.Invoke(Time.deltaTime);
    }

    public bool TryGetSpriteFromTree(Collider2D collider, out Sprite sprite)
    {
        if (collider.TryGetComponent(out Sprite s))
        {
            sprite = s;
            return true;
        }

        if (collider.transform.childCount > 0)
        {
            foreach (Transform child in collider.transform)
            {
                if (child.TryGetComponent(out Sprite cs))
                {
                    sprite = cs;
                    return true;
                }
            }
        }
        sprite = null;
        return false;
    }
}
