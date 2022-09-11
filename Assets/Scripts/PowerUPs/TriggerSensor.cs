using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : Reqire input
//Sends to trigger recivers
public class TriggerSensor : MonoBehaviour
{
    [Header("On Trigger Recivers")]
    [SerializeField] public SensorReciverBase[] recivers;

    [SerializeField] public bool active = true;
    [SerializeField] public string requiresInput;
    [SerializeField] private string reactOnlyToTag = "Player";

    private IEnumerator monitorInput;

    IEnumerator MonitorInput()
    {
        while (!Input.GetButtonDown(requiresInput))
        {
            print("corutine running");
            yield return null;
        }
        SendEnterToRecivers();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active)
            return;

        if (!string.IsNullOrEmpty(requiresInput))
        {
            monitorInput = MonitorInput();
            StartCoroutine(monitorInput);
            return;
        }

        if (!string.IsNullOrEmpty(reactOnlyToTag))
        {
            if (collision.tag == reactOnlyToTag)
            {
                SendEnterToRecivers();
            }
            return;
        }
        SendEnterToRecivers();
    }

    void SendEnterToRecivers()
    {
        print("Send Enter");
        foreach (SensorReciverBase obj in recivers)
        {
            obj.Enter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!active)
            return;

        if (monitorInput != null)
            StopCoroutine(monitorInput);


        if (!string.IsNullOrEmpty(reactOnlyToTag))
        {
            if (collision.tag == reactOnlyToTag)
            {
                SendExitToRecivers();
            }
            return;
        }
        SendExitToRecivers();
    }
    void SendExitToRecivers()
    {
        print("Send Exit");
        foreach (SensorReciverBase obj in recivers)
        {
            obj.Exit();
        }
    }
}
