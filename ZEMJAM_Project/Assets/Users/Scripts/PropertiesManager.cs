using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum TimeType { ONE, COUNT, MAX }

[Serializable]
public struct Property
{
    public TimeType timeType;
    [DrawIf("timeType", TimeType.ONE)] public bool isCalled;
    [DrawIf("timeType", TimeType.COUNT)] public int maxCount;
    [DrawIf("timeType", TimeType.COUNT)] public int curCount;

    [Space]
    public string explain;
    public Image image;
    [Space]
    public UnityEvent selectEvent;
}

public class PropertiesManager : MonoBehaviour
{
    public static PropertiesManager Inst;
    void Awake() => Inst = this;

    public Property[] property;

    public Property GetPropertyIndex()
    {
        int ran = 0;
        do
        {
            ran = Random.Range(0, property.Length);
        } while (GetUsetype(ran));

        return property[ran];
    }

    bool GetUsetype(int ran)
    {
        bool result;

        Debug.Log(property[ran].timeType);
        switch (property[ran].timeType)
        {
            case TimeType.ONE:
                result = property[ran].isCalled;
                property[ran].isCalled = true;
                break;
            case TimeType.COUNT:
                result = property[ran].curCount >= property[ran].maxCount;
                if(!result) property[ran].curCount++;
                break;
            default:
                result = false;
                break;
        }
        return result;
    }
}
