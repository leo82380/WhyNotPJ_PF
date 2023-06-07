using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
   public static CameraManager Instance;
    CinemachineBrain _cmBrain;
    CinemachineBasicMultiChannelPerlin _perlin;

    public void Init()
    {
        Instance = this;
         _cmBrain = Camera.main.GetComponent<CinemachineBrain>();
        _perlin = GameObject.Find("CM vcam0").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void CallShake(float AmplitudeGain =1, float waitTime=.3f, float duration=1)
    {
        StartCoroutine(Shake(AmplitudeGain, waitTime, duration));
    }
    IEnumerator Shake(float AmplitudeGain,float waitTime,float duration)
    {
        _perlin.m_AmplitudeGain = AmplitudeGain;
        yield return new WaitForSeconds(waitTime);
        DOTween.To(() => _perlin.m_AmplitudeGain, x => _perlin.m_AmplitudeGain = x, 0, duration);
    }
}
