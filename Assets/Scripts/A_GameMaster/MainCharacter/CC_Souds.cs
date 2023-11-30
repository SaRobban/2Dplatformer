using UnityEngine;

public class CC_Souds : MonoBehaviour
{
    public AudioClip[] steps;
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
}
