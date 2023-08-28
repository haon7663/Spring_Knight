using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceManager : MonoBehaviour
{
    public static DefenceManager Inst { get; private set; }
    void Awake() => Inst = this;

    public Transform Canvas;

    public Vector2 GetDefPosition(int index, int defence)
    {
        var def = defence - 1;
        var ind = index % 4;
        var vertical = Mathf.FloorToInt(def / 4);
        var horizontal = Mathf.FloorToInt(index / 4);
        var distance = vertical - horizontal;

        var setX = -(def % 4) + (ind * 2) - (distance * (3 - def % 4));
        var setY = vertical - horizontal * 2;

        return new Vector2(setX, setY);
    }
}
