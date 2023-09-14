using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    public static Fade Inst;
    void Awake() => Inst = this;

    [SerializeField] Image m_Image;

    private void Start()
    {
        Fadeout();
    }

    void Fadeout()
    {
        m_Image.enabled = true; 
        m_Image.DOFade(0, 0.1f);
    }
    public void Fadein(float time = 0.1f)
    {
        m_Image.DOFade(1, time);
    }
}
