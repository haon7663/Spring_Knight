using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]
public struct OnOffButton
{
    public Image Button;
    public Text ButtonText;
    public Sprite SpriteOn;
    public Sprite SpriteOff;
    public Color TextOn;
    public Color TextOff;
}
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Inst;
    void Awake() => Inst = this;

    public GameMode gameMode;

    [Space]
    [Header("Main")]
    [SerializeField] GameObject StageModeView;
    [SerializeField] GameObject InfiniteModeView;
    [SerializeField] GameObject StatsModeView;

    [Space]
    [Header("Other")]
    [SerializeField] GameObject SetModeView;
    [SerializeField] GameObject SetStageView;
    [SerializeField] GameObject DailyLoginView;
    [SerializeField] GameObject MissionView;
    [SerializeField] GameObject RankingView;

    [Space]
    [Header("ItemGain")]
    [SerializeField] PrizeDatas prizeDatas;
    [SerializeField] GameObject ItemGainView;
    [SerializeField] Transform ItemGainContent;
    [SerializeField] GainPrizeInfo ItemPrefab;
    List<GameObject> gainItem = new List<GameObject>();

    [Space]
    [Header("Menus")]
    [SerializeField] OnOffButton CharacterMenuButton;
    [SerializeField] OnOffButton HomeMenuButton;
    [SerializeField] OnOffButton ShopMenuButton;
    [SerializeField] GameObject CharacterMenu;
    [SerializeField] GameObject HomeMenu;
    [SerializeField] GameObject ShopMenu;

    [Space]
    [Header("Value")]
    [SerializeField] Text GoldText;
    [SerializeField] Image LevelFilled;

    void Start()
    {
        Time.timeScale = 1;
        SetHomeView(true);
        SetCharacterView(false);
        SetShopView(false);
    }

    void Update()
    {
        var saveData = SaveManager.Inst.saveData;
        GoldText.text = string.Format("{0:#,###}", saveData.gold);
        LevelFilled.fillAmount = saveData.curExp / saveData.maxExp;
    }
    void ResetModeView()
    {
        StageModeView.SetActive(false);
        InfiniteModeView.SetActive(false);
        StatsModeView.SetActive(false);
    }
    public void ModeActive(bool value)
    {
        SetModeView.SetActive(value);
    }
    public void StageActive(bool value)
    {
        SetStageView.SetActive(value);
    }
    public void OpenItemActive(int amounts, PrizeType prizeTypes)
    {
        ItemGainView.SetActive(true);
        GainPrizeInfo gainPrize = Instantiate(ItemPrefab, ItemGainContent);
        gainPrize.itemImage.sprite = prizeDatas.GetItemSprite(prizeTypes);
        gainPrize.amountText.text = "x" + amounts.ToString();
        gainItem.Add(gainPrize.gameObject);
    }
    public void CloseItemActive()
    {
        foreach(GameObject item in gainItem)
            Destroy(item);
        ItemGainView.SetActive(false);
    }
    public void SetStageModeView(string view)
    {
        ResetModeView();

        var setView = (GameMode)Enum.Parse(typeof(GameMode), view);
        gameMode = setView;
        SceneVariable.gameMode = setView;
        switch (setView)
        {
            case GameMode.STAGE:
                StageModeView.SetActive(true);
                break;
            case GameMode.INFINITE:
                InfiniteModeView.SetActive(true);
                break;
            case GameMode.GOLD:
                StatsModeView.SetActive(true);
                break;
        }

        ModeActive(false);
    }

    public void SetCharacterView(bool value)
    {
        CharacterMenu.SetActive(value);
        if (value) //ON
        {
            CharacterMenuButton.Button.sprite = CharacterMenuButton.SpriteOn;
            CharacterMenuButton.ButtonText.color = CharacterMenuButton.TextOn;
        }
        else
        {
            CharacterMenuButton.Button.sprite = CharacterMenuButton.SpriteOff;
            CharacterMenuButton.ButtonText.color = CharacterMenuButton.TextOff;
        }
    }
    public void SetHomeView(bool value)
    {
        HomeMenu.SetActive(value);
        if (value) //ON
        {
            HomeMenuButton.Button.sprite = HomeMenuButton.SpriteOn;
            HomeMenuButton.ButtonText.color = HomeMenuButton.TextOn;
        }
        else
        {
            HomeMenuButton.Button.sprite = HomeMenuButton.SpriteOff;
            HomeMenuButton.ButtonText.color = HomeMenuButton.TextOff;
        }
    }
    public void SetShopView(bool value)
    {
        ShopMenu.SetActive(value);
        if (value) //ON
        {
            ShopMenuButton.Button.sprite = ShopMenuButton.SpriteOn;
            ShopMenuButton.ButtonText.color = ShopMenuButton.TextOn;
        }
        else
        {
            ShopMenuButton.Button.sprite = ShopMenuButton.SpriteOff;
            ShopMenuButton.ButtonText.color = ShopMenuButton.TextOff;
        }
    }


    public void SetDailyLoginView(bool value)
    {
        DailyLoginView.SetActive(value);
    }
    public void SetMissionView(bool value)
    {
        MissionView.SetActive(value);
    }
    public void SetRankingView(bool value)
    {
        RankingView.SetActive(value);
    }

    public void GameStart()
    {
        Fade.Inst.Fadein();
        StartCoroutine(LoadScene("InGame", 0.5f));
    }
    IEnumerator LoadScene(string scnenName, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Debug.Log("GameStart");
        SceneManager.LoadScene(scnenName);
    }
}
