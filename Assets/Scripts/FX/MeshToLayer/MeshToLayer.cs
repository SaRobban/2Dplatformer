using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshToLayer : MonoBehaviour
{

    [SerializeField] private string _layerName = "Default";
    [SerializeField, Range(-10000, 10000)] private int _orderInLayer = 0;
    public bool recursive = false;

    // wait for renderer to be created
    public bool waitForRenderer = false;
    private void Start()
    {
        Renderer renderer = transform.GetComponent<Renderer>();
        if (renderer != null)
        {
            _layerName = renderer.sortingLayerName;
        }
        _SetLayer(_layerName, _orderInLayer);
    }

    private void _SetLayer(string layerName, int orderInLayer)
    {
        Renderer[] renderers;
        if (recursive)
        {
            renderers = transform.GetComponentsInChildren<Renderer>(true);
        }
        else
        {
            renderers = transform.GetComponents<Renderer>();
        }
        foreach (Renderer renderer in renderers)
        {
           // renderer.sortingLayerName = layerName;
            renderer.sortingOrder = orderInLayer;
        }
    }
}