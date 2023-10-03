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
    [SerializeField] RectTransform[] propertieTransforms;
    [SerializeField] RectTransform propertyData;
    [SerializeField] Transform selectPanel;
    [SerializeField] Sprite frame_One;
    [SerializeField] Sprite frame_Count;
    [SerializeField] Sprite frame_Inf;

    [Space]
    [SerializeField] SelectProperty m_SelectProperty;

    public void Start()
    {
        if(GameManager.Inst.saveProperty.Length != 0) property = GameManager.Inst.saveProperty;
        for (int i = 0; i < property.Length; i++)
            property[i].isCalled = false;
        for(int i = 0; i < 3; i++)
        {
            RectTransform data = Instantiate(propertyData, propertieTransforms[i]);
            data.GetComponent<PropertyData>().index = GetPropertyIndex();
            data.anchoredPosition = Vector2.zero;
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
        if (!result && isChange)
        {
            GameManager.Inst.selectedPropertySprite.Add(property[ran].sprite);
        }

        GameManager.Inst.saveProperty = property;
        return result;
    }

    public Sprite GetFrame(int index)
    {
        Sprite sprite = property[index].timeType switch
        {
            TimeType.ONE => frame_One,
            TimeType.COUNT => frame_Count,
            _ => frame_Inf,
        };
        return sprite;
    }
    public void SelectIndex(int index)
    {
        m_SelectProperty.index = index;
    }
}
