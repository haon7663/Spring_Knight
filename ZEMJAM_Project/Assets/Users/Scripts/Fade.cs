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
        StartCoroutine(Fadeout());
    }

    IEnumerator Fadeout()
    {
        m_Image.enabled = true; 
        m_Image.DOFade(0, 0.1f);
        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        if (GameManager.Inst)
        {
            GameManager.Inst.ChangeState(GameState.PLAY);
            if(GameManager.Inst.curPaze == 0)
                UIManager.Inst.SetPowerGrid(GameManager.Inst.maxPower);
            else
                UIManager.Inst.SetProperties(true);
        }
    }
    public void Fadein(float time = 0.1f)
    {
        m_Image.DOFade(1, time);
    }
}
