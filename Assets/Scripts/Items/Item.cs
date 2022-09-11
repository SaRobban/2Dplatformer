using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool activated;
    public bool info;
    public string infoText;
    public GameObject speachBubble;
    private GameObject textObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Activate()
    {
        if (!activated)
        {
            if (info)
            {
                activated = true;
                textObject = Instantiate(speachBubble, transform.position + Vector3.up * 1.5f, Quaternion.identity);
                textObject.GetComponent<TextMesh>().text = infoText;
            }
        }
    }

    public void DeActivate()
    {
        activated = false;
        if (textObject != null)
        {
            Destroy(textObject);
        }
    }
}
