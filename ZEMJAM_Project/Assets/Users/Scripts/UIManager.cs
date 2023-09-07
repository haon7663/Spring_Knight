using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst { get; private set; }
    void Awake() => Inst = this;


    [SerializeField] Text pazeText;

    public void SetPazeText(int paze)
    {
        pazeText.text = "Paze " + paze;
    }
}
