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
    public float SmoothingRotation = 0.1f;

    public float SleepRotation = 0.1f;

    private bool _pause;
    private bool _seeTarget = false;

    void Start()
    {
        transform.position = Target.position;
        transform.rotation = Target.rotation;
        EventController.Instance.SubscribeToAllEvents(this);
    }

    void FixedUpdate()
    {
        if (_seeTarget)
        {
            Transform target = MissionController.Instance.States[MissionController.Instance.States.Length-1].Target.gameObject.transform;
            StartCoroutine(FSleepRotation(Quaternion.LookRotation(target.position - transform.position)));
        }
        if (_pause)
            return;
        transform.position = Vector3.Lerp(transform.position, Target.position, SmoothingPosition);
        StartCoroutine(FSleepRotation(Target.rotation));
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
                break;
        }
    }

    #endregion

    IEnumerator FSleepRotation(Quaternion rot)
    {
        yield return new WaitForSeconds(SleepRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation,rot, SmoothingRotation);
    }
}
