using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("SetPaze")]
    [SerializeField] Transform pazeBar;
    [SerializeField] RectTransform pazeGrid;
    [SerializeField] Image pazeFilled;

    public void SetPazeGrid(int maxPaze)
    {
        float pazeCount = maxPaze - 2;
        for(int i = 1; i <= pazeCount; i++)
        {
            RectTransform grid = Instantiate(pazeGrid, pazeBar);
            grid.anchoredPosition = new Vector3(i * (700 / (pazeCount+1)), 0);
            pazeFilled.DOFillAmount(GameManager.Inst.curPaze / GameManager.Inst.maxPaze, 0.25f);
        }
    }
}
