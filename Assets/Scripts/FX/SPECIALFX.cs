using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIALFX : MonoBehaviour
{
    public static SPECIALFX Command;
    public FX_FirePlayOnce umphLeftPreFab;
    public FX_FirePlayOnce umphRightPreFab;

    public enum fxList { umpf }


    public FX_FirePlayOnce[] fxlist;
    private Dictionary<string, FX_FirePlayOnce> spawnFX = new Dictionary<string, FX_FirePlayOnce>();

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (Command != null && Command != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        Command = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ConvertFxlistToDictionary();
        umphLeftPool = FillPool(umphLeftPreFab, 3);
        umphRightPool = FillPool(umphRightPreFab, 3);
    }
    void ConvertFxlistToDictionary()
    {
        //TODO : Pool
        foreach (FX_FirePlayOnce fx in fxlist)
        {
            GameObject instance = Instantiate(fx.gameObject);
            instance.name = instance.name.Replace("(Clone)", "");
            spawnFX.Add(instance.name, instance.GetComponent<FX_FirePlayOnce>());
        }
    }
    /// <summary>
    /// Spawn a effect playing once, by name of prefab
    /// </summary>
    /// <param name="name"></param>
    /*
    public void FireFX(string name, Vector2 pos, Vector2 up)
    {
        spawnFX[name].gameObject.SetActive(true);
        spawnFX[name].transform.position = pos;// Fire(pos);
        spawnFX[name].transform.up = up;
    }
    */


    private int umphLeftPoolNum = 0;
    private FX_FirePlayOnce[] umphLeftPool;
    public void FireFxUmphLeft(Vector2 pos, Vector2 up)
    {
        umphLeftPool[umphLeftPoolNum].gameObject.SetActive(true);
        umphLeftPool[umphLeftPoolNum].transform.position = pos;// Fire(pos);
        umphLeftPool[umphLeftPoolNum].transform.up = up;
        umphLeftPoolNum = AddAndCheckIfLoop(umphLeftPoolNum, umphLeftPool.Length);
    }

    private int umphRightPoolNum = 0;
    private FX_FirePlayOnce[] umphRightPool;
    public void FireFxUmphRight(Vector2 pos, Vector2 up)
    {
        umphRightPool[umphRightPoolNum].gameObject.SetActive(true);
        umphRightPool[umphRightPoolNum].transform.position = pos;// Fire(pos);
        umphRightPool[umphRightPoolNum].transform.up = up;
        umphRightPoolNum = AddAndCheckIfLoop(umphRightPoolNum, umphRightPool.Length);
    }

    private int AddAndCheckIfLoop(int i, int arrayLength)
    {
        if(i +1 >= arrayLength)
        {
            return 0;
        }
        return i + 1;
    }

    private FX_FirePlayOnce[] FillPool(FX_FirePlayOnce objectToInstance, int size)
    {
        FX_FirePlayOnce[] pool = new FX_FirePlayOnce[3];
        for (int i = 0; i < pool.Length; i++)
        {
            GameObject instance = Instantiate(objectToInstance.gameObject);
            instance.name = instance.name.Replace("(Clone)", "");
            pool[i] = Instantiate(instance.GetComponent<FX_FirePlayOnce>());
            instance.gameObject.SetActive(false);
        }
        return pool;
    }
}
