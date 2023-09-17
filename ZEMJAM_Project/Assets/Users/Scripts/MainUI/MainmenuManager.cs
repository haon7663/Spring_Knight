using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mainview { STAGE, INFINITE, STATS }
public enum Otherview { SETMODE, SETSTAGE, SHOP, PLAYER }
public class MainmenuManager : MonoBehaviour
{
    public static MainmenuManager Inst;
    void Awake() => Inst = this;

    public Mainview mainView;
    public Otherview otherView;

    [Space]
    [Header("Main")]
    [SerializeField] GameObject StageModeView;
    [SerializeField] GameObject InfiniteModeView;
    [SerializeField] GameObject StatsModeView;

    [Space]
    [Header("Other")]
    [SerializeField] GameObject SetModeView;
    [SerializeField] GameObject SetStageView;
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
    public void SetStageModeView(string view)
    {
        ResetModeView();

        var setView = (Mainview)Enum.Parse(typeof(Mainview), view);
        mainView = setView;
        switch (setView)
        {
            case Mainview.STAGE:
                StageModeView.SetActive(true);
                break;
            case Mainview.INFINITE:
                InfiniteModeView.SetActive(true);
                break;
            case Mainview.STATS:
                StatsModeView.SetActive(true);
                break;
        }

        ModeActive(false);
    }

    public void SetOtherView()
    {

    }
}
