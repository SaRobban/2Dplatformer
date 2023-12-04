using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : Reqire input
//Sends to trigger recivers
public class TriggerSensor : MonoBehaviour
{
    [Header("Trigger Recivers")]
    [SerializeField] public SensorReciverBase[] recivers;

    [Header("Sensor Settings")]
    [SerializeField] private bool actOnPlayerInteraction;
    [SerializeField] public bool requiresPlayerInteractionKey;
    [SerializeField] public bool requireTag;
    [SerializeField] private string[] reactOnlyToTags = new string[] { "Player" };

    [Header("Graphics")]
    [SerializeField] private Animator animator;

    bool subscribedToPlayer;

    private bool activated;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ValidCollider(collision.tag))
            return;

        if (ActOnlyOnPlayerInteraction())
            return;

        OnInteract();
        return;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Unsubscribe();
    }
    private void OnDisable()
    {
        Unsubscribe();
    }
    private void OnDestroy()
    {
        Unsubscribe();
    }

    void Unsubscribe()
    {
        if (subscribedToPlayer)
        {
            PlayerManager.mainCharacter.A_OnInteract -= OnInteract;
            subscribedToPlayer = false;
        }
    }
    bool ValidCollider(string tag)
    {
        if (requireTag)
        {
            foreach (string compTag in reactOnlyToTags)
            {
                if (compTag == tag)
                    return true;
            }
        }
        return false;
    }
    bool ActOnlyOnPlayerInteraction()
    {
        if (actOnPlayerInteraction)
        {
            PlayerManager.mainCharacter.A_OnInteract += OnInteract;
            subscribedToPlayer = true;
            return true;
        }
        return false;
    }
    public void OnInteract()
    {
        if (activated)
        {
            SetInActive();
            return;
        }
        SetActive();
    }
    void SetActive()
    {
        foreach (SensorReciverBase obj in recivers)
        {
            obj.Active();
        }
        animator.Play("Open");
        activated = true;
    }
    void SetInActive()
    {
        foreach (SensorReciverBase obj in recivers)
        {
            obj.InActive();
        }

        animator.Play("Close");
        activated = false;
    }

}
