using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Inst { get; private set; }
    void Awake() => Inst = this;


    [SerializeField] Text scoreText;
    public int score;

    public void KillScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
    public void BonusScore(int score)
    {
        this.score += score;
        scoreText.text = score.ToString();
    }
}
