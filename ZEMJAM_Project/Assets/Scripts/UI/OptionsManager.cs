using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public GameObject m_Right;
    public GameObject m_Left;
    public Text m_Explain;
    public Image m_Image;

    public GameObject m_Options;

    public void OnOptionSetting()
    {
        m_Options.SetActive(!m_Options.activeSelf);
    }
}
