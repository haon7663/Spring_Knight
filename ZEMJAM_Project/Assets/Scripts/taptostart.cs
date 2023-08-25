using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class taptostart : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);
    }
    private void Start()
    {
        GameManager.Inst.enemySummonCount = 4;
        GameManager.Inst.m_Score = 0;
        TileManager.Inst.tileSize = 7;
        GameManager.Inst.maxPower = 2;
        GameManager.Inst.paze = 0;
    }
    private bool isOne = false;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isOne)
        {
            Fade.instance.Fadein();
            Invoke(nameof(GameStart), 0.5f);
            isOne = true;
        }
    }
    private void GameStart()
    {
        SceneManager.LoadScene("Stage");
    }
}
