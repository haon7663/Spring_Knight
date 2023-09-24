using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Inst;
    void Awake() => Inst = this;

    [SerializeField] GameObject barrier;
    public bool isInvincible;
    public void SetBarrier(bool isInvincible)
    {
        barrier.SetActive(isInvincible);
        this.isInvincible = isInvincible;
    }
}
