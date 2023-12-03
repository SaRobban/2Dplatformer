using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrachTest : MonoBehaviour
{
    float intervall = 1f;
    int size = 256;
    SpriteRenderer sprite;
    Color[] pixelColor;
    internal class TestPixel
    {
        Color col;
        public TestPixel(Color col)
        {
            this.col = col;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        pixelColor = new Color[size * size];

        for (int i = 0; i < pixelColor.Length; i++)
        {

            int a = (int)Mathf.Clamp01(Random.Range(-2, 1.1f));
            Color col = Color.white;


            col.a = a;// (float)i * (1f/(float)pixelColor.Length);
            pixelColor[i] = col;
        }


        Texture2D blankTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        blankTexture.filterMode = FilterMode.Point;
        Sprite blankSprite = Sprite.Create(blankTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = blankSprite;

        sprite.sprite.texture.SetPixels(pixelColor);
        sprite.sprite.texture.Apply();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = size; i < pixelColor.Length; i++)
        {
            pixelCheck(i);
        }
        sprite.sprite.texture.SetPixels(pixelColor);
        sprite.sprite.texture.Apply();
        // intervall = Time.time + 0.1f;
        //}
    }
    int row = 0;
    void EvenPixel(int i)
    {
        for (int x = row; x < i; x++)
        {
            if (pixelColor[x].a == 0)
            {
                pixelColor[i].a = 0;
                pixelColor[x].a = 1;
                return;
            }
            row++;
        }
    }
    void pixelCheck(int i)
    {
        int under = i - size;

        if (pixelColor[i].a == 1)
        {
            if (pixelColor[under].a == 0)
            {
                pixelColor[under].a = 1;
                pixelColor[i].a = 0;
                return;
            }
            EvenPixel(i);
        }

    }
}
