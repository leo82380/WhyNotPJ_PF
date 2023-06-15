using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TimeController : MonoBehaviour
{
  public static  TimeController Instance;
    public void Init()
    {
        Instance = this;
    }
    public void SetTimeFreeze(float freezeValue,float beforeDelay,float freezeTime)
    {
        StopAllCoroutines();
        StartCoroutine(TimeFreezeCoroutine(freezeValue, beforeDelay ,() =>
        {
            StartCoroutine(TimeFreezeCoroutine(1f, freezeTime));
        }));
    }

    private IEnumerator TimeFreezeCoroutine(float freezeValue, float beforeDelay, Action Callback = null)
    {
        yield return new WaitForSecondsRealtime(beforeDelay);
        Time.timeScale= freezeValue;
        Callback?.Invoke();
    }
}
