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
    [SerializeField] Image powerWhiteFilled;
    [SerializeField] RectTransform powerGrid;
    [SerializeField] Text powerText;

    [Space]
    [Header("SetScore")]
    [SerializeField] Text scoreText;

    [Space]
    [Header("SetTimer")]
    [SerializeField] GameObject timeBundle;
    [SerializeField] Image timeFilled;
    [SerializeField] Text timeText;
    float timer;

    [Space]
    [Header("SetProperties")]
    [SerializeField] Image propertiesWindow;
    [SerializeField] RectTransform propertiesPanel;
    [SerializeField] RectTransform longExplainPanel;
    [SerializeField] Text loneExplain;
    public bool onProperties;

    [Space]
    [Header("SetCombo")]
    [SerializeField] Transform comboPanel;
    [SerializeField] RectTransform comboPrefab;
    [SerializeField] Vector2 comboStartPos;
    [SerializeField] Vector2 comboOriginPos;

    Text saveComboText;
    RectTransform pazeRect, powerRect;

    void Start()
    {
        pazeRect = pazeBar.transform.parent.GetComponent<RectTransform>();
        powerRect = powerBar.transform.parent.GetComponent<RectTransform>();
        SwapUI(false, 0.4f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            SetProperties(!onProperties);
        }

        if (timeFilled.gameObject.activeSelf && !GameManager.Inst.onPlay) return;

        scoreText.text = GameManager.Inst.score.ToString();
        timeFilled.fillAmount = timer / GameManager.Inst.maxTimer;
        timeText.text = timer.ToString("F1") + "sec";
        timer -= Time.deltaTime;
    }

    public void OpenTimer(float time)
    {
        timeBundle.SetActive(true);
        timer = time;
    }
    public void OpenScore()
    {
        scoreText.gameObject.SetActive(true);
    }

    public void SetProperties(bool onProperties)
    {
        this.onProperties = onProperties;
        if (onProperties)
        {
            GameManager.Inst.ChangeState(GameState.PAUSE);
            propertiesPanel.anchoredPosition = new Vector2(0, 1600);
            propertiesWindow.DOFade(0.7f, 0.25f).SetUpdate(true);
            propertiesPanel.DOAnchorPosY(0, 0.25f).SetUpdate(true);
        }
        else
        {
            GameManager.Inst.ChangeState(GameState.PLAY);
            propertiesWindow.DOFade(0, 0.25f).SetUpdate(true);
            propertiesPanel.DOAnchorPosY(1600, 0.25f).SetUpdate(true);
        }
    }
    public void SetExplainPanel(string longExplain)
    {
        loneExplain.text = longExplain;
    }

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
            grid.anchoredPosition = new Vector3(i * (468 / (powerCount + 1)) + 216.5f, 0);
        }
    }
    public void SetPower(float curPower)
    {
        float maxPower = GameManager.Inst.maxPower - 1;
        var value = curPower - 1 - maxPower;
        var fillValue = (curPower - 1) / maxPower;
        if (value > 0)
        {
            DOTween.Kill(powerFilled);
            powerText.text = " +" + value.ToString();
            powerText.rectTransform.DOAnchorPosX(93f, 0.15f);
            powerText.DOFade(1, 0.1f);
            powerFilled.DOFillAmount(1, 0.01f);
        }
        else
        {
            DOTween.Kill(powerFilled);
            powerText.rectTransform.DOAnchorPosX(12, 0.15f);
            powerText.DOFade(0, 0.1f);
            powerFilled.DOFillAmount(fillValue, 0.03f);
            powerWhiteFilled.DOFillAmount(fillValue, 0.3f);
        }
    }
    public void SwapUI(bool onPower, float time)
    {
        if (GameManager.Inst.m_GameMode != GameMode.STAGE) return;

        pazeRect.gameObject.SetActive(!onPower);
        powerRect.gameObject.SetActive(onPower);
        pazeRect.DOAnchorPosY(onPower ? 1000 : 800, time);
        powerRect.DOAnchorPosX(onPower ? 0 : 880, time).SetEase(Ease.OutExpo);
    }

    public void AddCombo(int value)
    {
        DeleteCombo(0.5f);
        var combo = Instantiate(comboPrefab, comboPanel);
        combo.anchoredPosition = comboStartPos;
        combo.DOAnchorPos(comboOriginPos, 0.5f).SetEase(Ease.OutQuint);
        saveComboText = combo.GetComponent<Text>();
        saveComboText.text = value.ToString();
        saveComboText.DOFade(1, 0.25f);
    }
    void DeleteCombo(float time)
    {
        if (saveComboText == null) return;
        saveComboText.DOFade(0, time);
        Destroy(saveComboText.gameObject, time);
    }
}
