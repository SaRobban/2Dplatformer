using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAlwaysOnTop : MonoBehaviour
{
    void Start()
    {
        Transform parent = transform.parent;
        transform.SetParent(null);
        transform.SetParent(parent);
    }
}
