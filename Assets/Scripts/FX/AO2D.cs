using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO2D : MonoBehaviour
{

    Texture aTexture;
    RenderTexture rTex;

    void Start()
    {
        if (!aTexture || !rTex)
        {
            Debug.LogError("A texture or a render texture are missing, assign them.");
        }
    }

    void Update()
    {
        Graphics.Blit(aTexture, rTex);
    }
}