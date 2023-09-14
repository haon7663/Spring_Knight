using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    CinemachineVirtualCamera cinemachineVirtualCamera;
    float shakeIntensity;
    float shakeTimer;

    private Camera MainCamera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        MainCamera = Camera.main;
        MainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void ShakeCamera(float intensity, float time)
    {
        shakeIntensity = intensity;
        shakeTimer = time;
    }
    private void Update()
    {
        if (shakeTimer > 0)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity * shakeTimer * 4;
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                MainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else MainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
