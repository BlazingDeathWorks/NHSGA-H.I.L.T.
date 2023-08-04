using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }
    public CinemachineBasicMultiChannelPerlin noise { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        noise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Update()
    {
        if (noise.m_AmplitudeGain > 0)
        {
            noise.m_AmplitudeGain -= 17f * Time.deltaTime;
            if (noise.m_AmplitudeGain < 0)
            {
                noise.m_AmplitudeGain = 0;
            }
        }
    }
}