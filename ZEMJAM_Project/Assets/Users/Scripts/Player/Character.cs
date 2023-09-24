using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Characters
{
    public GameObject slashPrf;
    public GameObject slashHitPrf;
    public GameObject afterImagePrf;
    public RuntimeAnimatorController animController;
}
public class Character : MonoBehaviour
{
    public Characters[] characters;

    [SerializeField] Animator m_Animator;
    [SerializeField] Movement m_Movement;
    [SerializeField] PlayerSpriteRenderer m_PlayerSpriteRenderer;

    public enum PlayerType { ROYALKNIGHT, ASSASSIN }
    public PlayerType playerType;

    void Awake()
    {
        var charType = characters[(int)playerType];
        m_Animator.runtimeAnimatorController = charType.animController;
        m_Movement.fireSlash = charType.slashPrf;
        m_Movement.fireHitEffect = charType.slashHitPrf;
        m_PlayerSpriteRenderer.afterImagePrf = charType.afterImagePrf;
    }
}
