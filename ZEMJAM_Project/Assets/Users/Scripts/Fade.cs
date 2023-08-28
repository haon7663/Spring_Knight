using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static Fade instance;
    private Animator mAnimator;
    private Image mImage;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        mImage = GetComponent<Image>();
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
