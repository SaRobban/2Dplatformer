using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprtiteEdit : MonoBehaviour
{
    Vector2[] springs;
    SpriteRenderer spriteSource;
    // Start is called before the first frame update
    void Start()
    {
        springs = new Vector2[2];

        spriteSource = GetComponent<SpriteRenderer>();
        float step = spriteSource.size.x / springs.Length-1;
        float thic = spriteSource.size.y;
        for (int i = 0; i < springs.Length; i++)
        {
            springs[i] = ToRectSpace(Vector2.right * i * step);
            Debug.Log("Surface " + springs[i]);
        }

        Create(thic);
    }
    void Create(float thickness)
    {
        Vector2[] verts = new Vector2[springs.Length * 2];
        ushort[] tris = new ushort[springs.Length * 3];

        int x = 0;
        for (int i = 0; i < springs.Length; i++)
        {
            Vector2 p1 = springs[i];
            Vector2 p2 = new Vector2(p1.x, p1.y - thickness);

            verts[x] = p1;
            verts[x + 1] = p2;
            x += 2;
        }
        //create tris
        int t = 0;
        for (int v = 0; v < verts.Length - 2; v += 2)
        {
            tris[t + 0] = (ushort)v;
            tris[t + 1] = (ushort)(v + 2);
            tris[t + 2] = (ushort)(v + 1);

            tris[t + 3] = (ushort)(v + 2);
            tris[t + 4] = (ushort)(v + 3);
            tris[t + 5] = (ushort)(v + 1);
            t += 6;
        }

        foreach (Vector2 vec in verts)
        {
            Debug.Log("Vert "+vec);
        }
            Debug.Log("vertLength : " + verts.Length);

        for (int g = 0; g < tris.Length; g += 3)
        {

            Debug.Log("tris : " + tris[g] + " " + tris[g + 1] + " " + tris[g + 2]);
        }
            Debug.Log("trisLength : " + tris.Length);

        spriteSource = GetComponent<SpriteRenderer>();
       // spriteSource.sprite.rect.Set(verts[1].x - 1, verts[1].y - 1, verts[verts.Length - 2].x + 1, verts[verts.Length - 2].y + 1);
        spriteSource.sprite.OverrideGeometry(verts, tris);
    }

    Vector2 ToRectSpace(Vector2 vertPos)
    {
        Vector2 rectLowLeft =  spriteSource.sprite.bounds.extents- spriteSource.sprite.bounds.center ;
        Vector2 rectSpace = vertPos + rectLowLeft;
        return rectSpace;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
