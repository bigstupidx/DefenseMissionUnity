using UnityEngine;
using System.Collections;

public class TutorialGUI : GUIObject 
{
    public TextMesh Text;
    private bool _showTakeoffText = true;

    public GameObject TargetArrowPrefab;
    private GameObject _arrow;

    protected override void AwakeProc()
    {
        base.AwakeProc();
        SubscrabeOnEvents.Add("Landing");
        SubscrabeOnEvents.Add("Takeoff");
        SubscrabeOnEvents.Add("WheelsUp");
        SubscrabeOnEvents.Add("WheelsDown");
        SubscrabeOnEvents.Add("OnHideGUI");
        SubscrabeOnEvents.Add("ViewZoneEnter");
        SubscrabeOnEvents.Add("ViewZoneExit");
        SubscrabeOnEvents.Add("MissionFinished");
    }

    void Start()
    {
        if (!(TransportGOController.Instance.SelectedMissionID == 0))
        {
            GameObject.Destroy(gameObject);
            return;
        }
        _arrow = GameObject.Instantiate(TargetArrowPrefab) as GameObject; 
        _arrow.transform.parent = CameraController.Instance.gameObject.transform;
        _arrow.transform.localPosition = new Vector3(0, 6, -14.8f);
        _arrow.transform.localScale = new Vector3(7, 7, 1);
        _arrow.transform.localRotation = Quaternion.Euler(300, 0, 270);
        Color col = _arrow.transform.GetChild(0).renderer.material.GetColor("_Color");
        col.a = 0;
        _arrow.transform.GetChild(0).renderer.material.SetColor("_Color", col);
    }

    void Update()
    {
        _arrow.transform.localRotation = Quaternion.Euler(280, 0, 270);
        Vector3 b = AirplaneController.Instance.GetMissionObject().transform.position - CameraController.Instance.transform.position;
        b.y = 0;
        Vector3 f = CameraController.Instance.transform.forward;
        f.y = 0;
        float a = Vector3.Angle(f, b);
        f = CameraController.Instance.transform.right;
        f.y = 0;
        if (Vector3.Angle(f, b) > 90)
            a *= -1;
        _arrow.transform.Rotate(_arrow.transform.forward, a,Space.World);
                                               
    }

    protected override void EventProc(string EventName, GameObject Sender)
    {
        //base.EventProc(EventName, Sender);

        switch (EventName)
        {
            case "OnHideGUI":
                renderer.enabled = false;
                Text.renderer.enabled = false;
                break;

            case "WheelsUp":
                renderer.enabled = false;
                Text.renderer.enabled = false;
                _showTakeoffText = false;
                break;

            case "WheelsDown":
                renderer.enabled = false;
                Text.renderer.enabled = false;
                break;

            case "Takeoff":
                if (_showTakeoffText)
                {
                    Text.text = "Hide landing gear\nto increase speed";
                    renderer.enabled = true;
                    Text.renderer.enabled = true;
                    _arrow.transform.GetChild(0).renderer.enabled = true;
                    Color col = _arrow.transform.GetChild(0).renderer.material.GetColor("_Color");
                    col.a = 1;
                    _arrow.transform.GetChild(0).renderer.material.SetColor("_Color", col);
                }
                break;

            case "ViewZoneEnter":
                if (MissionController.Instance.CurrentState.Type == MissionStateType.Landing
                    && !AirplaneController.Instance.ChassisEnable)
                {
                    Text.text = "Lower gear\nto land";
                    renderer.enabled = true;
                    Text.renderer.enabled = true;
                }
                break;

            case "ViewZoneExit":
                if (!_showTakeoffText)
                {
                    renderer.enabled = false;
                    Text.renderer.enabled = false;
                }
                break;

            case "MissionFinished":
                _arrow.transform.GetChild(0).renderer.enabled = false;
                break;
        }
    }
}
