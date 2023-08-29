using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimation : MonoBehaviour
{
    [SerializeField] Animator m_SwordAnimator;
    private Animator m_Animator;
    private Collison m_Collison;
    private Movement m_Movement;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Collison = GetComponent<Collison>();
        m_Movement = GetComponent<Movement>();
    }
    private void Update()
    {
        m_Animator.SetBool("onWall", m_Collison.onRight || m_Collison.onLeft);
        m_Animator.SetBool("onTopWall", m_Collison.onUp);
        m_Animator.SetBool("onBottomWall", m_Collison.onDown);
        m_Animator.SetBool("onMove", m_Movement.count > 0);
        m_Animator.SetBool("isAttack", m_Movement.isAttacking);
        m_SwordAnimator.SetBool("isAttack", m_Movement.isAttacking);
    }
}
