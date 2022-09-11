/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public TriggerSignals signal = TriggerSignals.Earth;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerSignalManager.TriggerRadio.Send(signal);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            TriggerSignalManager.TriggerRadio.Send(TriggerSignals.Earth);
            TriggerSignalManager.TriggerRadio.SendWithRange(TriggerSignals.Earth, (Vector2)transform.position, 10);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            TriggerSignalManager.TriggerRadio.Send(TriggerSignals.Wind);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            TriggerSignalManager.TriggerRadio.Send(TriggerSignals.Fire);
        }
    }
}
*/