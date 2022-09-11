using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelGrid : MonoBehaviour
{
    public Transform[] snapThis;

    public GameObject gridPlace;
    public GameObject fakePixel;
    public GameObject displayObject;

    private GameObject[,] pixelArray;
    

    // Start is called before the first frame update
    void Start()
    {
        pixelArray = new GameObject[32, 32];
        for(int x = 0; x < 32; x++)
        {
            for(int y = 0; y < 32; y++)
            {
                pixelArray[x, y] = Instantiate(fakePixel, new Vector2(x * 0.03125f, y * 0.03125f), Quaternion.identity);
                pixelArray[x, y].transform.localScale = Vector3.one * 0.03125f;
                pixelArray[x, y].GetComponent<Renderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        BoxCollider col = displayObject.GetComponent<BoxCollider>();
        if (col.bounds.Contains(Vector3.zero)){
            print("Zero");
        }
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {

                if (col.bounds.Contains(pixelArray[x, y].transform.position))
                {
                    pixelArray[x, y].GetComponent<Renderer>().enabled = true;
                }
                else
                {
                   pixelArray[x, y].GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }
}
