using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_DeathOverlay : MonoBehaviour
{
    [SerializeField] private Image m_overlay;
    private void Start()
    {
        m_overlay.gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        Debug.Log("Fade in death");
        m_overlay.gameObject.SetActive(true);
        StartCoroutine(FadeInDeath());
    }

    IEnumerator FadeInDeath()
    {
        float fade = 0;
        while (fade < 1)
        {
            fade += Time.deltaTime;
            Color col = m_overlay.color;
            col.a = fade;
            m_overlay.color = col;
            yield return null;
        }
    }

    public void Close()
    {
        m_overlay.gameObject.SetActive(false);
    }
}
