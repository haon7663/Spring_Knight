using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum TimeType { ONE, COUNT, INFINITE }

[Serializable]
public struct Property
{
    public TimeType timeType;
    [DrawIf("timeType", TimeType.ONE)] public bool isUsed;
    [DrawIf("timeType", TimeType.COUNT)] public int maxCount;
    [DrawIf("timeType", TimeType.COUNT)] public int curCount;
    public bool isCalled;

    [Space]
    public Sprite sprite;
    public string explain;
    public string longExplain;
    [Space]
    public float value;
    [Space]
    public UnityEvent selectEvent;
}

public class PropertiesManager : MonoBehaviour
{
    public static PropertiesManager Inst;
    void Awake() => Inst = this;

    public Property[] property;

    [Space]
    [SerializeField] RectTransform propertyData;
    [SerializeField] Transform selectPanel;
    [SerializeField] int propertyCount;

    [Space]
    [SerializeField] SelectProperty m_SelectProperty;

    public void Start()
    {
        if(GameManager.Inst.saveProperty.Length != 0) property = GameManager.Inst.saveProperty;
        for (int i = 0; i < property.Length; i++)
            property[i].isCalled = false;
        for(int i = 0; i < propertyCount; i++)
        {
            RectTransform data = Instantiate(propertyData, selectPanel);
            data.GetComponent<PropertyData>().index = GetPropertyIndex();
            data.anchoredPosition = new Vector2(0, 190 - (510/ propertyCount) * i);
        }
    }
    public int GetPropertyIndex()
    {
        int ran = 0;
        do
        {
            ran = Random.Range(0, property.Length);
        } while (GetUsetype(ran) || property[ran].isCalled);

        property[ran].isCalled = true;
        return ran;
    }
    public bool GetUsetype(int ran, bool isChange = false)
    {
        bool result;

        switch (property[ran].timeType)
        {
            case TimeType.ONE:
                result = property[ran].isUsed;
                if (!result && isChange) property[ran].isUsed = true;
                break;
            case TimeType.COUNT:
                result = property[ran].curCount >= property[ran].maxCount;
                if(!result && isChange) property[ran].curCount++;
                break;
            default:
                result = false;
                break;
        }

        GameManager.Inst.saveProperty = property;
        return result;
    }
    public void SelectIndex(int index)
    {
        m_SelectProperty.index = index;
    }
}
