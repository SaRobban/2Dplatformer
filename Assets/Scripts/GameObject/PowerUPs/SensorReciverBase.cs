using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for trigger recivers
public class SensorReciverBase : MonoBehaviour
{
   public virtual void Active()
    {

    }
    public virtual void InActive()
    {

    }

    public virtual void Active(Collider2D collision)
    {

    }
    public virtual void InActive(Collider2D collision)
    {

    }
}
