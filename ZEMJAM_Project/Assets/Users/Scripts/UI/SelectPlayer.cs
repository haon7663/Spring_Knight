using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayer : MonoBehaviour
{
    public PlayerType playerType;

    [Serializable]
    public struct PlayerInfo
    {
        public RuntimeAnimatorController idleAnimator;
        public string name;
        [TextArea] public string explain;
        public string skillName;
        [TextArea] public string skillExplain;

        public int hp;
        public int atk;
        public int bounce;
        public float speed;
    }

    [SerializeField] Animator playerStand;
    [SerializeField] Text playerName;
    [SerializeField] Text playerExplain;
    [SerializeField] Text skillName;
    [SerializeField] Text skillExplain;

    [SerializeField] PlayerInfo[] playerInfos;

    [SerializeField] OnOffButton statusButton;
    [SerializeField] OnOffButton explainButton;

    void Start()
    {
        ChangePlayer((int)playerType);
    }
    public void ChangePlayer(int index)
    {
        playerType = (PlayerType)index;

        var info = playerInfos[index];
        playerStand.runtimeAnimatorController = info.idleAnimator;
        playerName.text = info.name;
    }

    public void SetStatus(bool value)
    {
        if (value) //ON
        {
            statusButton.Button.sprite = statusButton.SpriteOn;
            statusButton.ButtonText.color = statusButton.TextOn;
        }
        else
        {
            statusButton.Button.sprite = statusButton.SpriteOff;
            statusButton.ButtonText.color = statusButton.TextOff;
        }
    }
    public void SetSkill(bool value)
    {
        if (value) //ON
        {
            explainButton.Button.sprite = explainButton.SpriteOn;
            explainButton.ButtonText.color = explainButton.TextOn;
        }
        else
        {
            explainButton.Button.sprite = explainButton.SpriteOff;
            explainButton.ButtonText.color = explainButton.TextOff;
        }
    }
}
