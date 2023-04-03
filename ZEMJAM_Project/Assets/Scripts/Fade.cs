using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public static Fade instance;
    private Animator mAnimator;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        Fadeout();
    }

    private void Fadeout()
    {
        mAnimator.Play("FadeOut");
    }
    public void Fadein()
    {
        mAnimator.Play("FadeIn");
    }
}
