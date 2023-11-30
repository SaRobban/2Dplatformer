using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_CheckClipTimes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainCharacter owner = GetComponent<MainCharacter>();
        foreach (AnimationClip clip in owner.anim.runtimeAnimatorController.animationClips)
        {
            Debug.Log("ClipName : " + clip.name + "  lenght : " + clip.length);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
