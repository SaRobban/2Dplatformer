using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsettTextrue : MonoBehaviour
{
    Material mat;
    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
 
    }
    private void Update()
    {

        mat.SetTextureOffset("Tex",  mat.GetTextureOffset("Tex") + Vector2.one * Time.deltaTime); 
    }
}
