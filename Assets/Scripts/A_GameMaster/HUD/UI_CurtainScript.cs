using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_CurtainScript : MonoBehaviour
{
    public Color color;
    public float alpha;

    private Image blank;
    private Coroutine runningOperation;

    void Awake()
    {
        if (TryGetComponent(out Image img))
        {
            blank = img;
            color = blank.color;
            alpha = blank.color.a;
        }
        else
        {
            Debug.LogError("No image object on " + transform.name);
        }
    }

    public void FadeOut(float seconds)
    {
        float speed = 1 / seconds;
        if (runningOperation != null)
        {
            Debug.Log("Runnning");
            StopCoroutine(runningOperation);

        }
        runningOperation = StartCoroutine(C_FadeOut(speed));
    }
    public void FadeIn(float seconds)
    {
        float speed = 1 / seconds;
        if (runningOperation != null)
        {
            Debug.Log("Runnning");
            StopCoroutine(runningOperation);
        }
        runningOperation = StartCoroutine(C_FadeIn(speed));
    }

    private IEnumerator C_FadeIn(float speed)
    {
        Debug.Log("FadeIN");
        blank.enabled = true;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * speed;
            color.a = alpha;
            blank.color = color;
            yield return null;
        }
        alpha = 0;
        color.a = alpha;
        blank.color = color;

        blank.enabled = false;
        runningOperation = null;
    }
    private IEnumerator C_FadeOut(float speed)
    {
        blank.enabled = true;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * speed;
            color.a = alpha;
            blank.color = color;
            yield return null;
        }
        alpha = 1;
        color.a = alpha;
        blank.color = color;

        runningOperation = null;
    }
}
