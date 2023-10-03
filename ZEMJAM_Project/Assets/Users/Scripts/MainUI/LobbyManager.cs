using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] GameObject ShopView;
    [SerializeField] GameObject PlayerView;

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


    public void SetDailyLoginView(bool value)
    {
        DailyLoginView.SetActive(value);
    }
    public void SetMissionView(bool value)
    {
        MissionView.SetActive(value);
    }


    public void GameStart()
    {
        Fade.Inst.Fadein();
        StartCoroutine(LoadScene("Ingame", 0.5f));
    }
    IEnumerator LoadScene(string scnenName, float delay)
    {
        yield return YieldInstructionCache.WaitForSeconds(delay);
        SceneManager.LoadScene(scnenName);
    }
}
