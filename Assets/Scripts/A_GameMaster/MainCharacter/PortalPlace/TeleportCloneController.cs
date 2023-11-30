using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class mean the object can move trough portals 
/// </summary>
public class TeleportCloneController : MonoBehaviour
{
    [SerializeField()] private GameObject clonePreFab;
    internal GameObject cloneInstance;

    Transform from;
    Transform to;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        if (clonePreFab.TryGetComponent( out TeleportCloneController t))
        {
            Debug.LogError("Cloned object should not be cloneble");
            return;
        }

        cloneInstance = Instantiate(clonePreFab);
        Hide();
    }

    public virtual void Show(Transform from, Transform to)
    {
        cloneInstance.gameObject.SetActive(true);
        this.from = from;
        this.to = to;
    }

    public virtual void Move()
    {
        Vector3 pos = from.InverseTransformPoint(transform.position);
        cloneInstance.transform.position = to.TransformPoint(-pos);
    }

    public virtual void Hide()
    {
        cloneInstance.gameObject.SetActive(false);
    }
}
