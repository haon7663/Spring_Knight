using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyData : MonoBehaviour
{
    public Text explainText;
    public Image spriteImage;

    string longExplain;
    public int index;

    public void Start()
    {
        var property = PropertiesManager.Inst.property[index];

        float value = property.value * (property.timeType == TimeType.COUNT ? property.curCount + 1 : 1);
        string explain = property.explain.Replace("!", value.ToString());
        longExplain = property.longExplain.Replace("!", value.ToString());

        explainText.text = explain;
        spriteImage.sprite = property.sprite;
    }

    public void ButtonDown()
    {
        UIManager.Inst.SetExplainPanel(longExplain);
        PropertiesManager.Inst.SelectIndex(index);
    }
}
