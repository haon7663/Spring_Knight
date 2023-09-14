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
    [SerializeField] Image pazeFilled;
    [SerializeField] RectTransform pazeGrid;
    [SerializeField] RectTransform pazePlayer;

    [Space]
    [Header("SetPower")]
    [SerializeField] Transform powerBar;
    [SerializeField] Image powerFilled;
    [SerializeField] RectTransform powerGrid;

    public void SetPaze(int curPaze, int maxPaze)
    {
        float pazeCount = maxPaze - 1;
        for(int i = 1; i <= pazeCount; i++)
        {
            RectTransform grid = Instantiate(pazeGrid, pazeBar);
            grid.anchoredPosition = new Vector3(i * (700 / (pazeCount+1)), 0);
        }

        pazeFilled.fillAmount = (curPaze) / (float)maxPaze;
        pazePlayer.anchoredPosition = new Vector2(58 + (curPaze - 1) * (700 / (pazeCount + 1)), pazePlayer.anchoredPosition.y);

        pazeFilled.DOFillAmount((curPaze+1) / (float)maxPaze, 0.75f);
        pazePlayer.DOAnchorPosX(58 + curPaze * (700 / (pazeCount + 1)), 0.75f);
    }

    public void SetPowerGrid(int maxPower)
    {
        float powerCount = maxPower - 2;
        for (int i = 1; i <= powerCount; i++)
        {
            RectTransform grid = Instantiate(powerGrid, powerBar);
            grid.anchoredPosition = new Vector3(i * (700 / (powerCount + 1)), 0);
        }
    }
    public void SetPower(float curPower)
    {
        float maxPower = GameManager.Inst.managerPower - 1;
        powerFilled.DOFillAmount((curPower-1) / maxPower, 0.3f);
    }
}
