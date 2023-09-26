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
    public GameObject skillObjPrf_1;
    public GameObject skillObjPrf_2;
    public RuntimeAnimatorController animController;
}
public class Character : MonoBehaviour
{
    public static Character Inst;

    public Characters[] characters;

    [SerializeField] Animator m_Animator;
    [SerializeField] Movement m_Movement;
    [SerializeField] PlayerSpriteRenderer m_PlayerSpriteRenderer;

    public enum PlayerType { ROYALKNIGHT, ASSASSIN }
    public PlayerType playerType;

    void Awake()
    {
        Inst = this;

        var charType = characters[(int)playerType];
        m_Animator.runtimeAnimatorController = charType.animController;
        m_Movement.slash = charType.slashPrf;
        m_Movement.hitEffect = charType.slashHitPrf;
        m_PlayerSpriteRenderer.afterImagePrf = charType.afterImagePrf;
        m_Movement.skill = charType.skillObjPrf_1;
    }
}
