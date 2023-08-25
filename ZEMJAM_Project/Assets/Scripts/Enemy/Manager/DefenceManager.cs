using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceManager : MonoBehaviour
{
    public static DefenceManager Inst { get; private set; }

    void Awake() => Inst = this;

    public Vector2 GetDefPosition(int index, int defence)
    {
        var def = defence - 1;
        var ind = index % 4;

        var setX = -def + ind * 2;
        var setY = (Mathf.FloorToInt(def / 4) - 1) - index * 2;

        return new Vector2(setX, setY);
    }
}
