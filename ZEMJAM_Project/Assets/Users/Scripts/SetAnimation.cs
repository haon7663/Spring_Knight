using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimation : MonoBehaviour
{
    Animator m_Animator;
    Animator m_SwordAnimator;
    Collison m_Collison;
    Movement m_Movement;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Collison = GetComponent<Collison>();
        m_Movement = GetComponent<Movement>();

        m_SwordAnimator = transform.GetChild(0).GetComponent<Animator>();
    }
    void Update()
    {
        m_Animator.SetBool("onWall", m_Collison.onRight || m_Collison.onLeft);
        m_Animator.SetBool("onTopWall", m_Collison.onUp);
        m_Animator.SetBool("onBottomWall", m_Collison.onDown);
        m_Animator.SetBool("onMove", m_Movement.count > 0);
        m_Animator.SetBool("isAttack", m_Movement.isAttacking);
        m_SwordAnimator.SetBool("isAttack", m_Movement.isAttacking);
    }

    public void AttackTrigger()
    {
        m_Animator.SetTrigger("attack");
    }

    public void Spin(bool value)
    {
        m_Animator.SetBool("isSpin", value);
        if(value) m_Animator.SetTrigger("spin");
    }
}
