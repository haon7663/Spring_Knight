using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Inst;
    void Awake() => Inst = this;

    [Space]
    [Header("SettingBar")]
    [SerializeField] GameObject setting;
    [SerializeField] GameObject defaultSetting;
    [SerializeField] GameObject soundSetting;

    [Space]
    [Header("PlayerSetting")]
    public bool onCameraFollow = true;
    public bool onVibration = false;

    Camera mainCamera;

    float saveTimeScale;
    Vector3 saveAngle;
    Vector3 savePos;

    void Start()
    {
        mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        if (setting.activeSelf)
        {
            mainCamera.transform.eulerAngles = saveAngle;
            mainCamera.transform.position = savePos;
            Time.timeScale = 0;
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
            OnSetting(true);
    }

    #region ButtonSetting
    public void OnSetting(bool isActive)
    {
        setting.SetActive(isActive);
        if (!isActive) Time.timeScale = saveTimeScale;
        else if (isActive)
        {
            saveAngle = mainCamera.transform.eulerAngles;
            savePos = mainCamera.transform.position;
            saveTimeScale = Time.timeScale;
        }
        GameManager.Inst.ChangeState(isActive ? GameState.PAUSE : GameState.PLAY);
        GameManager.Inst.isSetting = isActive;
    }
    public void OnReGame()
    {
        Time.timeScale = 1;
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
    #endregion

    #region Vibartion
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif
    public void Vibrate(long milliseconds)
    {
        if (IsAndroid() && onVibration)
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }
    private bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
    #endregion
}
