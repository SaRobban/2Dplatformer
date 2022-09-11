using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpaceEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent space; 


    [SerializeField] public List< IEvent> listners = new List<IEvent>();
    
    public void AddListnerToFire(IEvent eventScript)
    {
        listners.Add(eventScript);
       // print(listners.Count);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            space?.Invoke();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            OnFire();
        }
    }

    void OnFire()
    {
        if(listners != null)
        {
            foreach(IEvent listner in listners)
            {
                listner.OnEvent();
            }
        }
//        fire?.OnEvent();
    }

}
