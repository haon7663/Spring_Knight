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
        GameManager.Gm.m_EnemySummonCount = 3;
        GameManager.Gm.m_Score = 0;
        GameManager.Gm.TileSize = 6;
        GameManager.Gm.MaxPower = 3;
        GameManager.Gm.paze = 0;
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
        SceneManager.LoadScene(1);
    }
}
