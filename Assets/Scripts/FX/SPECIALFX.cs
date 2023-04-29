using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIALFX : MonoBehaviour
{
    public static SPECIALFX Command;
    private Dictionary<string, FX_pool> spawnFX;

    class FX_pool
    {
        int currentPoolnum = 0;
        GameObject[] fx;

        public FX_pool(Transform parent, GameObject animationPreFab, int poolSize)
        {
            currentPoolnum = 0;

            Transform holder = new GameObject().transform;
            holder.parent = parent;
            holder.name = animationPreFab.name + " POOL";

            fx = new GameObject[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                GameObject newFX = Instantiate(animationPreFab,holder);
                fx[i] = newFX;
                newFX.SetActive(false);
            }
        }
        public void TriggerObjectFromPoolAt(Vector3 pos, Vector3 dir)
        {
            if (fx[currentPoolnum].activeSelf)
            {
                string errorMessege = "<color=red>Pool out of Range : " + fx[0].name + "</color>";
                Debug.LogWarning(errorMessege);
                fx[currentPoolnum].SetActive(false);
            }

            fx[currentPoolnum].gameObject.SetActive(true);
            fx[currentPoolnum].transform.position = pos;
            fx[currentPoolnum].transform.up = dir;
            currentPoolnum++;
            if (currentPoolnum >= fx.Length)
                currentPoolnum = 0;
        }
    }

[Header("Pool Special fx")]
[SerializeField]    private GameObject[] FX_Prefabs;
    [SerializeField] private int poolSize = 3;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (Command != null && Command != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Command = this;
        DontDestroyOnLoad(this.gameObject);

        MakeDictionaryAndPoolFX();
    }

    private void MakeDictionaryAndPoolFX()
    {
        spawnFX = new Dictionary<string, FX_pool>();

        foreach (GameObject fx in FX_Prefabs)
        {
            FX_pool pool = new FX_pool(transform, fx, poolSize);
            spawnFX.Add(fx.name, pool);
        }
#if UNITY_EDITOR
        string names= "Special FX list : ";
        foreach (string key in spawnFX.Keys)
        {
            names += key + ", ";
        }
        Debug.Log("<color=yellow>" + names.ToString() + "</color>");
#endif

    }


    // Start is called before the first frame update
    void Start()
    {
    }


    public void Fire(string name, Vector3 pos, Vector3 dir)
    {
        if (spawnFX.TryGetValue(name, out FX_pool pool))
        {
            pool.TriggerObjectFromPoolAt(pos, dir);
        }
    }
}
