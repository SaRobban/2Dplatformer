using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class postProssesingCustom : MonoBehaviour
{
    public Material postProsessingMAt;

    public bool toggle;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (toggle)
        {
            Graphics.Blit(source, destination, postProsessingMAt);
        }
        else
        {
            Graphics.Blit(source, destination);
            return;
        }

    }

    public void Toggle()
    {
        toggle = !toggle;
    }
}
