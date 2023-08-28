using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.Inst.SetGame();
        StartCoroutine(MoveScene());
    }
    IEnumerator MoveScene()
    {
        Fade.instance.Fadein();
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Faze");
    }
}
