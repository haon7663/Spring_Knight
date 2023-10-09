using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Inst;
    void Awake() => Inst = this;

    [Space]
    [Header("SettingBar")]
    [SerializeField] GameObject setting;
    [SerializeField] GameObject defaultSetting;
    [SerializeField] GameObject soundSetting;
    [SerializeField] Transform propertyContent;
    [SerializeField] GameObject selectedProperty;
    [SerializeField] ContentSizeFitter csf;

    [Space]
    [Header("PlayerSetting")]
    public bool onCameraFollow = true;
    public bool onVibration = false;
    public int cameraShakeSize = 3;

    Camera mainCamera;

    public int savePropertyCount = 0;
    float saveTimeScale;
    Vector3 saveAngle;
    Vector3 savePos;

    void Start()
    {
        mainCamera = Camera.main;

        setting.GetComponent<SettingController>().UIInitializing();
        CinemachineShake.Inst.SetShake(cameraShakeSize);
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
        if(pauseStatus && GameManager.Inst.m_GameState == GameState.PLAY)
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

            var sprites = GameManager.Inst.selectedPropertySprite;
            for (int i = savePropertyCount; i < sprites.Count; i++)
            {
                GameObject property = Instantiate(selectedProperty);
                property.transform.SetParent(propertyContent.GetChild(Mathf.FloorToInt(i / 3)), false);
                property.transform.GetChild(0).GetComponent<Image>().sprite = sprites[i];
            }
            savePropertyCount = sprites.Count;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)csf.transform);
        }
        GameManager.Inst.ChangeState(isActive ? GameState.PAUSE : GameState.PLAY);
        GameManager.Inst.isSetting = isActive;
    }
    public void OnReGame()
    {
        Time.timeScale = 1;
        GameManager.Inst.ResetLoad("InGame");
    }
    public void OnMain()
    {
        Time.timeScale = 1;
        GameManager.Inst.ResetLoad("Lobby");
    }
    public void OnSoundSetting(bool onSound)
    {
        defaultSetting.SetActive(!onSound);
        soundSetting.SetActive(onSound);
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
