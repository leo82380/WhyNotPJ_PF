using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Awake()
    {
        CreateCameraManager();
    }
    private void CreateCameraManager()
    {
        GameObject camManager = new GameObject("CameraManager");
        camManager.AddComponent<CameraManager>().Init();
    }
}
