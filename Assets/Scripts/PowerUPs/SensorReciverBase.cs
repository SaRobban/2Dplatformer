using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for trigger recivers
public class SensorReciverBase : MonoBehaviour
{
   public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }

    public virtual void Enter(Collider2D collision)
    {

    }
    public virtual void Exit(Collider2D collision)
    {

    }
}
