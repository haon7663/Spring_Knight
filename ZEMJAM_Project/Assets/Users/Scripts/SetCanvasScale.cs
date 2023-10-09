using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvasScale : MonoBehaviour
{
    CanvasScaler thisCanvas;

    void Start()
    {
        thisCanvas = GetComponent<CanvasScaler>();

        float fixedAspectRatio = 9f / 19f;
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        //���� �ػ� ���� ������ �� �� ���
        if (currentAspectRatio > fixedAspectRatio) thisCanvas.matchWidthOrHeight = 1;
        //���� �ػ��� ���� ������ �� �� ���
        else if (currentAspectRatio < fixedAspectRatio) thisCanvas.matchWidthOrHeight = 0;
    }
}
