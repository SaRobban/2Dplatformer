using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObserver : MonoBehaviour, IEvent
{
    [SerializeField] SpaceEvent se;
    private void Start()
    {
        se = GameObject.FindObjectOfType<SpaceEvent>();
        se.AddListnerToFire(this);
    }
    public void OnEvent()
    {
      //  print("Hej från event");
    }
    public void Input(string s)
    {
        print(s);
    }
}
