/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerReciver : MonoBehaviour, ICanReciveSignals
{
    void Start()
    {
        TriggerSignalManager.TriggerRadio.AddListener(this.gameObject);
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public void RecieveSignal(TriggerSignals signal)
    {
        switch (signal)
        {
            case TriggerSignals.Earth:
                Debug.Log("Earth");
                break;
            case TriggerSignals.Fire:
                Debug.Log("Fire");
                break;
            case TriggerSignals.Wind:
                Debug.Log("Wind");
                break;
            default:
                Debug.Log("Not identifyed");
                break;
        }
    }
}
*/