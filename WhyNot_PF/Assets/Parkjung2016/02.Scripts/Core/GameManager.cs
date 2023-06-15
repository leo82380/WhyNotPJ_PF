using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Awake()
    {
        CreateCameraManager();
        CreateTimeController();
    }
    private void CreateCameraManager()
    {
        GameObject camManager = new GameObject("CameraManager");
        camManager.transform.SetParent(transform);
        camManager.AddComponent<CameraManager>().Init();
    }
    private void CreateTimeController()
    {
        GameObject timeCon = new GameObject("TimeController");
       timeCon.transform.SetParent(transform);

        timeCon.AddComponent<TimeController>().Init();
    }
}
