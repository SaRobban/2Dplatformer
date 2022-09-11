using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PS_ChangeGradient : MonoBehaviour
{
    [SerializeField] private Color MagicStart = Color.white;
    [SerializeField] private Color MagicMid = Color.magenta;
    [SerializeField] private Color MagicEnd = Color.black;
    [SerializeField] private Color IceStart = Color.white;
    [SerializeField] private Color IceMid = Color.blue;
    [SerializeField] private Color IceEnd = Color.black;
    [SerializeField] private Color FireStart = Color.white;
    [SerializeField] private Color FireMid = Color.yellow;
    [SerializeField] private Color FireEnd = Color.red;

    [SerializeField] private ParticleSystem ps;
    private void Start()
    {
        var col = ps.colorOverLifetime;
        col.enabled = true;

        ChangeToMagic();
    }
    public void ChangeToMagic()
    {
        var col = ps.colorOverLifetime;

        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Fixed;
        gradient.SetKeys(
          new GradientColorKey[]
        { new GradientColorKey(MagicStart.linear, 0.25f), new GradientColorKey(MagicMid.linear, 0.5f), new GradientColorKey(MagicEnd.linear,0.75f) }
        , new GradientAlphaKey[]
        { new GradientAlphaKey(MagicStart.a, 0.25f), new GradientAlphaKey(MagicMid.a, 0.5f), new GradientAlphaKey(MagicEnd.a, 0.75f) }
        );
        col.color = gradient;
    }

    public void ChangeToIce()
    {
        var col = ps.colorOverLifetime;

        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Fixed;
        gradient.SetKeys(
          new GradientColorKey[]
        { new GradientColorKey(IceStart, 0.25f), new GradientColorKey(IceMid, 0.5f), new GradientColorKey(IceEnd,0.75f) }
        , new GradientAlphaKey[]
        { new GradientAlphaKey(1.0f, 0.25f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(1.0f, 0.75f) }
        );
        col.color = gradient;
    }

    public void ChangeToFire()
    {
        var col = ps.colorOverLifetime;

        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Fixed;
        gradient.SetKeys(
          new GradientColorKey[]
        { new GradientColorKey(FireStart, 0.25f), new GradientColorKey(FireMid, 0.5f), new GradientColorKey(FireEnd,0.75f) }
        , new GradientAlphaKey[]
        { new GradientAlphaKey(1.0f, 0.25f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(1.0f, 0.75f) }
        );
        col.color = gradient;
    }



}