using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Movement mMovement;
    private Animator mAnimator;
    private Collison mCollison;

    private SpriteRenderer mSpriteRenderer;

    private AudioSource mAudioSource;


    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mMovement = GetComponentInParent<Movement>();
        mAnimator = GetComponent<Animator>();
        mCollison = GetComponentInParent<Collison>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!(mMovement.m_Count < 0 || mCollison.onCollision))
        {
            mSpriteRenderer.enabled = true;
        }
        else
        {
            mSpriteRenderer.enabled = false;
        }
    }
}
