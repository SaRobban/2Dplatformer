using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_HealthBar : MonoBehaviour
{
    public Image fullHeartPreFab;
    public Image emptyHeartPreFab;

    public Transform fullHeartHolder;
    public Transform emptyHeartHolder;

    private GameObject[] fullHearts;
    void Start()
    {

        int maxNumberOfHearts = PlayerMain.mainCharacter.stats.FullHP;
        fullHearts = new GameObject[maxNumberOfHearts];

        for (int i = 0; i < maxNumberOfHearts; i++)
        {
            Instantiate(emptyHeartPreFab, emptyHeartHolder);
            fullHearts[i] = Instantiate(fullHeartPreFab, fullHeartHolder).gameObject;
        }

        PlayerMain.mainCharacter.healthSystem.healthChange += OnHealthChanged;
    }

    public void OnHealthChanged(int health)
    {
        Debug.Log("Current Health : " + health + "  ArrLength : " + fullHearts[0].name);

        for (int i = 0; i < fullHearts.Length; i++)
        {
            if (i >= health)
            {
                fullHearts[i].SetActive(false);
                continue;
            }
            fullHearts[i].SetActive(true);
        }
    }
}
