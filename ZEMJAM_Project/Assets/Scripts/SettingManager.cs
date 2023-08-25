using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public GameObject Setting;
    private Camera m_MainCamera;
    private float saveTimeScale;

    public GameObject m_DefaultSetting;
    public GameObject m_SoundSetting;

    private Vector3 saveAngle;
    private Vector3 savePos;

    private void Start()
    {
        m_MainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        if (Setting.activeSelf)
        {
            m_MainCamera.transform.eulerAngles = saveAngle;
            m_MainCamera.transform.position = savePos;
            Time.timeScale = 0;
        }
    }
    public void OnSetting()
    {
        Setting.SetActive(!Setting.activeSelf);
        if (!Setting.activeSelf) Time.timeScale = saveTimeScale;
        else if (Setting.activeSelf)
        {
            saveAngle = m_MainCamera.transform.eulerAngles;
            savePos = m_MainCamera.transform.position;
            saveTimeScale = Time.timeScale;
        }
        GameManager.Inst.doSetting = Setting.activeSelf;
    }
    void OnApplicationPause(bool pauseStatus)
    {
        Setting.SetActive(pauseStatus);
    }
    public void OnReGame()
    {
        GameManager.Inst.enemySummonCount = 3;
        GameManager.Inst.m_Score = 0;
        TileManager.Inst.tileSize = 7;
        GameManager.Inst.maxPower = 3;
        GameManager.Inst.paze = 0;
        SceneManager.LoadScene("Faze");
    }
    public void OnMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage");
    }
    public void OnSoundSetting()
    {
        m_DefaultSetting.SetActive(!m_DefaultSetting.activeSelf);
        m_SoundSetting.SetActive(!m_SoundSetting.activeSelf);
    }
}
