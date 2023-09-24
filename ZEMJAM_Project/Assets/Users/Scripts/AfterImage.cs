using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AfterImage : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Color StartColor;
    [SerializeField] Color EndColor;
    [SerializeField] float time;

    void Start()
    {
        spriteRenderer.color = StartColor;
        spriteRenderer.DOColor(EndColor, time);
        Destroy(gameObject, time);
    }
}
