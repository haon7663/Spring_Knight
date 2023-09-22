using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyData : MonoBehaviour
{
    public Text explainText;
    public Image spriteImage;

    void Start()
    {
        Property property = PropertiesManager.Inst.GetProperty();

        float value = property.value * (property.timeType == TimeType.COUNT ? property.curCount : 1);
        string explain = property.explain.Replace("!", value.ToString());

        explainText.text = explain;
        spriteImage.sprite = property.sprite;
    }
}
