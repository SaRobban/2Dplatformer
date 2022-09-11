/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TriggerSignalManager : MonoBehaviour
{
    //Singleton 
    private static TriggerSignalManager _triggerRadio;
    public static TriggerSignalManager TriggerRadio { get { return _triggerRadio; } }

    private void Awake()
    {
        if (_triggerRadio != null && _triggerRadio != this)
        {
            Destroy(this);
        }
        else
        {
            _triggerRadio = this;
        }
    }

    //private ICanReciveSignals[] recievers;
    private List<ICanReciveSignals> recievers;
    private List<Transform> recieversTransform;
    private void Start()
    {
        
        recievers = new List<ICanReciveSignals>();
        recievers = FindObjectsOfType<MonoBehaviour>().OfType<ICanReciveSignals>().ToList();
        var rec = FindObjectsOfType(typeof(ICanReciveSignals)).ToList();


        recieversTransform = (FindObjectsOfType<MonoBehaviour>().OfType<ICanReciveSignals>() as Transform[]).ToList();
        Debug.Log(recieversTransform.Count);


        for(int i = 0; i < recievers.Count; i++)
        {
            recievers.Add(recieversTransform[i].GetComponent<ICanReciveSignals>());
        }
        
    }

    public void AddListener(GameObject listener)
    {
      //  recievers.Add(listener.GetComponent<ICanReciveSignals>());
       // recieversTransform.Add(listener.transform);
    }

    public void Send(TriggerSignals signal)
    {
        foreach (ICanReciveSignals reciever in recievers)
        {
            reciever.RecieveSignal(signal);
        }
    }

    public void SendWithRange(TriggerSignals signal, Vector2 pos, float range)
    {
        float sqrRange = range * range;

        for (int i = 0; i < recieversTransform.Count; i++)
        {
            float sqrDist = ((Vector2)recieversTransform[i].position - pos).sqrMagnitude;
            if (sqrDist < sqrRange)
            {
                recievers[i].RecieveSignal(signal);
            }
        }
    }
}
///////////////////////////////////////////////////////////////////////////////////////
public enum TriggerSignals { Earth, Wind, Fire }

public interface ICanReciveSignals
{
    void RecieveSignal(TriggerSignals signal);
}

public interface ICanSendSignals
{
    void SendSignal(TriggerSignals signal);
}
*/