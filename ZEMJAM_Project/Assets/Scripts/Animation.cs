using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator mAnimator;

    private Collison mCollison;
    private Movement mMovement;

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        mCollison = GetComponent<Collison>();
        mMovement = GetComponent<Movement>();
    }
    private void Update()
    {
        mAnimator.SetBool("onWall", mCollison.onRight || mCollison.onLeft);
        mAnimator.SetBool("onTopWall", mCollison.onUp);
        mAnimator.SetBool("onBottomWall", mCollison.onDown);
        mAnimator.SetBool("onMove", mMovement.m_Count >= 0);
        mAnimator.SetBool("isAttack", mMovement.m_isAtk);
    }
}
