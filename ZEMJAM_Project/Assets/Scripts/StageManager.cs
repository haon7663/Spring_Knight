using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.Gm.m_EnemySummonCount = 4;
        GameManager.Gm.m_Score = 0;
        GameManager.Gm.TileSize = 6;
        GameManager.Gm.MaxPower = 2;
        GameManager.Gm.paze = 0;
        GameManager.Gm.Managerhp = 3;
        StartCoroutine(MoveScene());
    }
    IEnumerator MoveScene()
    {
        Fade.instance.Fadein();
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Faze");
    }
}
