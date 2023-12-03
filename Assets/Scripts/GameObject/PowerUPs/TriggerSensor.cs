using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : Reqire input
//Sends to trigger recivers
public class TriggerSensor : MonoBehaviour
{
    public string TagFilter = "";


    [Header("On Trigger Recivers")]
    [SerializeField] public SensorReciverBase[] recivers;

    [Header("Sensor Settings")]
    [SerializeField] public bool requiresPlayerInteractionKey;
    [SerializeField] public bool requireTag;
    [SerializeField] private string[] reactOnlyToTags = new string[] { "Player" };


    [SerializeField] private bool subscribedToInteraction;

    private List<Collider2D> validObjectsInSensor = new List<Collider2D>();
    private bool activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (subscribedToInteraction && collision.tag == "Player")
        {
            validObjectsInSensor.Add(collision);
            PlayerManager.mainCharacter.A_OnInteract += SendEnterToRecivers;
            subscribedToInteraction = true;
            return;
        }

        if (requireTag)
        {
            foreach (string cTag in reactOnlyToTags)
            {
                if (cTag == collision.tag)
                {
                    validObjectsInSensor.Add(collision);
                    SendEnterToRecivers();
                    return;
                }
            }
            return;
        }

        validObjectsInSensor.Add(collision);
        SendEnterToRecivers();
        return;
    }



    public void SendEnterToRecivers()
    {
        if (activated)
            return;

        print("Send Enter");
        foreach (SensorReciverBase obj in recivers)
        {
            obj.Enter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (subscribedToInteraction && collision.tag == "Player")
        {
            PlayerManager.mainCharacter.A_OnInteract -= SendEnterToRecivers;
            subscribedToInteraction = false;
        }

        if (validObjectsInSensor.Contains(collision))
        {
            validObjectsInSensor.Remove(collision);
            if (validObjectsInSensor.Count == 0)
                SendExitToRecivers();
        }

    }
    void SendExitToRecivers()
    {
        print("Send Exit");
        foreach (SensorReciverBase obj in recivers)
        {
            obj.Exit();
        }
    }

    private void OnDisable()
    {
        if (subscribedToInteraction)
            PlayerManager.mainCharacter.A_OnInteract -= SendEnterToRecivers;
    }

    private void OnDestroy()
    {
        if (subscribedToInteraction)
            PlayerManager.mainCharacter.A_OnInteract -= SendEnterToRecivers;
    }
}
