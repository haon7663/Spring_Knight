using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.Inst.m_EnemySummonCount = 4;
        GameManager.Inst.m_Score = 0;
        TileManager.Inst.tileSize = 6;
        GameManager.Inst.MaxPower = 2;
        GameManager.Inst.paze = 0;
        GameManager.Inst.Managerhp = 3;
        StartCoroutine(MoveScene());
    }
    IEnumerator MoveScene()
    {
        Fade.instance.Fadein();
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Faze");
    }
}
