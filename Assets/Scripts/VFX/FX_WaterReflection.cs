using UnityEngine;

public class FX_WaterReflection : MonoBehaviour
{
    //2D waterReflection made by Robert Sandh
    //Add to Quadmesh component in Scene

    public int pxPerMeter = 32;
    void Start()
    {
        //Get size from sprite
        Vector2 bounds = gameObject.GetComponent<Renderer>().bounds.extents;

        //Create a render texture
        RenderTexture rtex = new RenderTexture((int)(bounds.x * 2 * pxPerMeter), (int)(bounds.y * 2 * pxPerMeter),100);
        rtex.filterMode = FilterMode.Point;

        //Create a camera to render to the render texture and position it
        Camera reflectionCam = new GameObject().AddComponent<Camera>();
        reflectionCam.orthographic = true;
        reflectionCam.targetTexture = rtex;
        reflectionCam.orthographicSize = bounds.y;
        reflectionCam.transform.position = transform.position + Vector3.forward *-10 + Vector3.up * bounds.y *2;

        //Apply renderTexture to material
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", rtex);
    }
}
