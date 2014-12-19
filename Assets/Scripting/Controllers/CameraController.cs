using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour, IEventSubscriber
{
    public static CameraController Instance { get; private set; }

    public CameraController()
    {
        Instance = this;
    }

    public Transform Target = null;

    public float SmoothingPosition = 0.7f;
    public float SmoothingRotation = 0.7f;

    public float SleepRotation = 0.1f;

    public List<CameraData> CameraDatas;

    public Transform MainCamera;

    private bool _pause;
    private bool _seeTarget = false;
    private CameraData _currentCameraData;
    private int _currentCameraDataIndex;


    public void ResetCamera()
    {
        _currentCameraDataIndex = CameraDatas.Count - 2;
        ChangeCamera();

        
    }

    public void ChangeCamera()
    {
        _currentCameraData = CameraDatas[_currentCameraDataIndex--];

        if (_currentCameraDataIndex >= CameraDatas.Count)
        {
            _currentCameraDataIndex = 0;
        }
        else if (_currentCameraDataIndex < 0)
        {
            _currentCameraDataIndex = CameraDatas.Count - 1;
        }

        UpdateCameraToCameraData();
    }

    private void UpdateCameraToCameraData()
    {
        SmoothingPosition = _currentCameraData.SmoothingPosition;
        SmoothingRotation = _currentCameraData.SmoothingRotation;
        SleepRotation = _currentCameraData.SleepRotation;

        MainCamera.localPosition = _currentCameraData.Position;
        MainCamera.localRotation = Quaternion.Euler(_currentCameraData.Rotation);

        foreach (Transform t in AirplaneController.Instance.Driver.Insides)
        {
            t.gameObject.SetActive(!_currentCameraData.Inside);
        }

        AirplaneController.Instance.Driver.InsideView.gameObject.SetActive(_currentCameraData.Inside);

        MainCamera.GetComponent<Camera>().fieldOfView = _currentCameraData.Fov;
        foreach (Camera c in  MainCamera.GetComponentsInChildren<Camera>())
        {
            c.fieldOfView = _currentCameraData.Fov;
        }

    }

    void Start()
    {
        CameraDatas = AirplaneController.Instance.GetComponent<CameraDataHolder>().CameraDatas;

        ResetCamera();

        transform.position = Target.position;
        transform.rotation = Target.rotation;
        EventController.Instance.SubscribeToAllEvents(this);
    }

    void FixedUpdate()
    {
        if (_seeTarget)
        {
            Vector3 target = MissionObject.LastDestroyedPosition;// MissionController.Instance.States[MissionController.Instance.States.Length-1].Target.gameObject.transform;
            StartCoroutine(FSleepRotation(Quaternion.LookRotation(target- transform.position)));
        }
        if (_pause)
            return;
        transform.position = Vector3.Lerp(transform.position, Target.position, SmoothingPosition);
        StartCoroutine(FSleepRotation(Target.rotation));
    }

    private void LateUpdate()
    {
        if (SmoothingPosition >= 1)
        {
            transform.position = Target.position;
        }

    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "OnShowPauseMenu":
                _pause = true;
                StopAllCoroutines();
                break;
                
            case "OnResume":
                _pause = false;
                break;

            case "MissionFailed":
                _pause = true;
                break;

            case "MissionFinished":
                _pause = true;
                _seeTarget = true;
                CameraController.Instance.ResetCamera();
                CameraController.Instance.ChangeCamera();
                break;
        }
    }
    #endregion

    IEnumerator FSleepRotation(Quaternion rot)
    {
        if (SleepRotation > 0)
        {
            yield return new WaitForSeconds(SleepRotation);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation,rot, SmoothingRotation);
    }



    [Serializable]
    public class CameraData
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public float SmoothingPosition;
        public float SmoothingRotation;

        public float SleepRotation;

        public float Fov = 40;

        public bool Inside;
    }
}
