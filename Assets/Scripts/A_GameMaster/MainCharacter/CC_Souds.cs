using UnityEngine;

public class CC_Souds : MonoBehaviour
{
    public AudioClip[] steps;
    public AudioClip[] howls;
    public AudioClip dialog;
    public AudioClip thud;
    public AudioSource player;
    public void PlaySteps()
    {
        player.clip = steps[Random.Range(0, steps.Length)];
        player.Play();
    }

    public void PlayThud()
    {
        player.clip = thud;
        player.Play();
    }

    public float PlayHowl()
    {
        player.clip = howls[Random.Range(0, howls.Length)];
        float l = player.clip.length;
        player.Play();
        return l;
    }

    public void PlayDialog()
    {
        player.clip = dialog;
        player.Play();
    }
}
