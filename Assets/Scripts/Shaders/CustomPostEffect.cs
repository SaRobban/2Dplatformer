using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class CustomPostEffect : MonoBehaviour
{
    public float intensity;
[SerializeField]    private Material material;
    // Creates a private material used to the effect
    void Awake()
    {
      //  material = new Material(Shader.Find("Custom/blur"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }
        //material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }
}