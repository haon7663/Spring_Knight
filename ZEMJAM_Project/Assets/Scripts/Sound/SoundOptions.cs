using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider BgmSlider;
    public Slider SfxSlider;

    public void SetBgmVolme()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }

    public void SetSFXVolme()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
    }
}
