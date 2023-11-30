using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public GameObject flame;
    public GameObject lightFX;
    SpawnLocation thisLocation;
    private void Awake()
    {
        thisLocation = new SpawnLocation(SceneManager.GetActiveScene().name, transform.position+ Vector3.back);
        TurnOutCandle();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LightCandle();
        }
    }

    void LightCandle()
    {
        flame.SetActive(true);
        lightFX.SetActive(true);
        PlayerMain.mainCharacter.stats.SetSpawnPoint(thisLocation);
    }

    void TurnOutCandle()
    {
        flame.SetActive(false);
        lightFX.SetActive(false);
    }
}

public class SpawnLocation
{
    public string sceneName;
    public Vector3 location;
    public SpawnLocation(string sceneName, Vector3 location)
    {
        this.sceneName = sceneName;
        this.location = location;
    }
}
