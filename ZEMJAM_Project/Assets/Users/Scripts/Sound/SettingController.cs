using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField] Slider[] slider;
    [SerializeField] Text[] sliderValueText;

    [SerializeField] Toggle audioVolumeToggle;
    [SerializeField] Toggle onCameraFollowToggle;
    [SerializeField] Toggle onVibrationToggle;

    SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        slider[0].onValueChanged.AddListener(value => soundManager.AudioControl("Master", slider[0]));
        slider[1].onValueChanged.AddListener(value => soundManager.AudioControl("BGM", slider[1]));
        slider[2].onValueChanged.AddListener(value => soundManager.AudioControl("SFX", slider[2]));

        audioVolumeToggle.onValueChanged.AddListener(value => soundManager.ToggleAudioVolume());
        onCameraFollowToggle.onValueChanged.AddListener(value => SettingManager.Inst.onCameraFollow = value);
        onVibrationToggle.onValueChanged.AddListener(value => SettingManager.Inst.onVibration = value);

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Update()
    {
        for (var i = 0; i < slider.Length; i++)
        {
            sliderValueText[i].text = ((int)(slider[i].value * 2.5f + 100)).ToString();
        }
    }

    public void UIInitializing()
    {
        audioVolumeToggle.isOn = SaveManager.Inst.saveData.playSoundToggle;
        onCameraFollowToggle.isOn = SaveManager.Inst.saveData.cameraFollowToggle;
        onVibrationToggle.isOn = SaveManager.Inst.saveData.vibrationToggle;

        AudioListener.volume = SaveManager.Inst.saveData.playSoundToggle ? 0 : 1;
        SettingManager.Inst.onCameraFollow = SaveManager.Inst.saveData.cameraFollowToggle;
        SettingManager.Inst.onVibration = SaveManager.Inst.saveData.vibrationToggle;

        for (var i = 0; i < slider.Length; i++)
        {
            slider[i].value = SaveManager.Inst.saveData.volume[i];
        }
    }

    void PlayerSettingSave()
    {
        for (var i = 0; i < slider.Length; i++)
        {
            SaveManager.Inst.saveData.volume[i] = slider[i].value;
        }
        SaveManager.Inst.saveData.playSoundToggle = audioVolumeToggle.isOn;
        SaveManager.Inst.saveData.cameraFollowToggle = onCameraFollowToggle.isOn;
        SaveManager.Inst.saveData.vibrationToggle = onVibrationToggle.isOn;
        SaveManager.Inst.Save();
    }

    void OnSceneUnloaded(Scene current)
    {
        PlayerSettingSave();
    }

    void OnApplicationQuit()
    {
        PlayerSettingSave();
    }
}