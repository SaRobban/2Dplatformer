using UnityEngine;
/// <summary>
/// Plays a clip then disable the gameobject.
/// </summary>
public class FX_PlayAnimationOnce : MonoBehaviour
{
    private Animator animator;
    private float clipPlayTime;
    private float time;
    private string clipName;
   
    void Init()
    {
        animator = GetComponent<Animator>();
        AnimatorClipInfo[]  clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        clipPlayTime = clipInfo[0].clip.length;
        clipName = clipInfo[0].clip.name;
    }

    private void OnEnable()
    {
        if (!animator)
            Init();

        time = clipPlayTime;
        animator.Play(clipName);
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
            gameObject.SetActive(false);
    }
}
