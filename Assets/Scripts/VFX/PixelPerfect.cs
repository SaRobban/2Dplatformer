using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfect : MonoBehaviour
{
    [Header("Build")]
    [Tooltip("Snap position to game resulution")]
    public int resPerUnit;
    public Transform[] snapThis;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        for(int i = 0; i < snapThis.Length; i++)
        {
            Vector2 pos = snapThis[i].position;
            pos *= resPerUnit;
            pos.x = Mathf.RoundToInt(pos.x);
            pos.y = Mathf.RoundToInt(pos.y);
            snapThis[i].position = pos/ resPerUnit;
        }
    }
}
