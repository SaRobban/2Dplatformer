using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Lightning : MonoBehaviour
{
    [Header("SizeSettings")]
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] int crackels = 10;
    [SerializeField] float range = 1;
    [Header("BehaviourSettings")]
    [SerializeField] float intrevall = 0.05f;
    [SerializeField] float disableIfRangeAbove = 3;


    Transform[] nubs;
    Transform[] lines;
    float t = 0.05f;



    // Start is called before the first frame update
    public void Init(GameObject spark, GameObject line)
    {
        nubs = new Transform[crackels];
        lines = new Transform[crackels];

        for (int i = 0; i < crackels; i++)
        {
            nubs[i] = Instantiate(spark).transform;
            nubs[i].rotation = Quaternion.Euler(0, 0, Random.Range(-360, 360));
            lines[i] = Instantiate(line).transform;
        }
    }

    public void UpdateTrail(bool active)
    {
        t -= Time.deltaTime;

        if (t < 0)
        {
            SwisselAndAddPos();
            VariableWitdh();
            if (!active)
                gameObject.SetActive(EndTrail());

            t = intrevall;
        }
    }

    void SwisselAndAddPos()
    {
        Transform lastNub = nubs[nubs.Length - 1];
        Transform lastLine = lines[lines.Length - 1];

        for (int i = nubs.Length - 1; i > 0; i--)
        {
            nubs[i] = nubs[i - 1];
            lines[i] = lines[i - 1];
        }

        lastNub.position = transform.position + (Vector3)Random.insideUnitCircle * range;
        nubs[0] = lastNub;

        Vector3 dir = lastNub.position - nubs[1].position;
        float length = dir.magnitude;

        //If lightning is teleported dont draw lines over screen
        if (length > disableIfRangeAbove)
            lastLine.gameObject.SetActive(false);
        else
        {
            lastLine.gameObject.SetActive(lastNub.gameObject.activeSelf);
        }

        lastLine.position = lastNub.position - dir * 0.5f;
        lastLine.up = dir;
        lastLine.localScale = new Vector3(lineWidth, length, lineWidth);
        lines[0] = lastLine;
    }

    void VariableWitdh()
    {
        int scale = nubs.Length;
        for (int i = 0; i < nubs.Length; i++)
        {
            Vector3 localScale = lines[i].localScale;
            localScale.x = lineWidth * scale * 0.25f;
            lines[i].localScale = localScale;
            scale--;
        }
    }

    public void StartTrail()
    {
        for (int i = 0; i < crackels; i++)
        {
            nubs[i].position = transform.position;
            lines[i].position = transform.position;

            lines[i].gameObject.SetActive(true);
            nubs[i].gameObject.SetActive(true);
        }
        SwisselAndAddPos();
    }

    public void ResetPosition()
    {
        for (int i = 0; i < crackels; i++)
        {
            nubs[i].position = transform.position;
            lines[i].position = transform.position;
        }
    }

    bool EndTrail()
    {
        for (int i = nubs.Length - 1; i >= 0; i--)
        {
            if (nubs[i].gameObject.activeSelf)
            {
                lines[i].gameObject.SetActive(false);
                nubs[i].gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }
}
