using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_LightnignMManager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] GameObject target;
    Vector3 lastTargetPos;

    [Header("Prefabs")]
    [SerializeField] private GameObject nubPreFab;
    [SerializeField] private GameObject linePreFab;


    FX_Lightning[] ligntnings;
  public  bool active;
    // Start is called before the first frame update
    void Start()
    {
        ligntnings = GetComponentsInChildren<FX_Lightning>();

        foreach (FX_Lightning flash in ligntnings)
        {
            flash.Init(nubPreFab, linePreFab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        lastTargetPos = transform.position;

        transform.position = target.transform.position;
        if (active != target.activeSelf)
        {
            if (target.activeSelf)
                StartTrail();

            active = target.activeSelf;
        }
        UpdateTrail();
    }

    void StartTrail()
    {
        foreach (FX_Lightning flash in ligntnings)
        {
            flash.gameObject.SetActive(true);
            flash.StartTrail();
        }
    }

    void UpdateTrail()
    {
      
        foreach (FX_Lightning flash in ligntnings)
        {
            if (flash.gameObject.activeSelf)
                flash.UpdateTrail(active);
        }
    }

    private void ResetTrailPositions()
    {
        foreach (FX_Lightning flash in ligntnings)
        {
            if (flash.gameObject.activeSelf)
                flash.ResetPosition();
        }
    }
}
