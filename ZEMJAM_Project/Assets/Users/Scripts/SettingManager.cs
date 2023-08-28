using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] GameObject setting;
    [SerializeField] GameObject defaultSetting;
    [SerializeField] GameObject soundSetting;

    Camera mainCamera;

    float saveTimeScale;
    Vector3 saveAngle;
    Vector3 savePos;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        if (setting.activeSelf)
        {
            mainCamera.transform.eulerAngles = saveAngle;
            mainCamera.transform.position = savePos;
            Time.timeScale = 0;
        }
    }
    public void OnSetting()
    {
        setting.SetActive(!setting.activeSelf);
        if (!setting.activeSelf) Time.timeScale = saveTimeScale;
        else if (setting.activeSelf)
        {
            saveAngle = mainCamera.transform.eulerAngles;
            savePos = mainCamera.transform.position;
            saveTimeScale = Time.timeScale;
        }
        GameManager.Inst.isSetting = setting.activeSelf;
    }
    void OnApplicationPause(bool pauseStatus)
    {
        setting.SetActive(pauseStatus);
    }
    public void OnReGame()
    {
        SceneManager.LoadScene("Faze");
    }
    public void OnMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage");
    }
    public void OnSoundSetting()
    {
        defaultSetting.SetActive(!defaultSetting.activeSelf);
        soundSetting.SetActive(!soundSetting.activeSelf);
    }
}
