using UnityEngine;
using System.Collections;

public class ProgressBar : GUIObject
{
    public float Progress;
    public Transform Button;
    public TextMesh Text;

    protected override void AwakeProc()
    {
        base.AwakeProc();
        SubscrabeOnEvents.Add("OnPressObject");
        SubscrabeOnEvents.Add("OnReleaseObject");
        SubscrabeOnEvents.Add("OnUpdateGUI");
    }

    protected override void EventProc(string EventName, GameObject Sender)
    {
        base.EventProc(EventName, Sender);

        switch (EventName)
        {
            case "OnPressObject":
                if (Sender == Button.gameObject)
                    _drag = true;
                break;

            case "OnReleaseObject":
                _drag = false;
                break;

            case "OnUpdateGUI":
                Text.text = ((int)(Progress * 100)) + "%";
                Button.localPosition = new Vector3(Progress - 0.5f, 0, -1);
                break;
        }
    }

    private bool _drag;

    void Update()
    {
        if (_drag)
        {
            Vector3 pos = transform.InverseTransformPoint(
                MenuController.Instance.Cam2D.ScreenPointToRay(
                MenuController.Instance.LastStartedGesture.EndPoint).origin);
            pos.y = 0;
            pos.z = -1;
            pos.x = Mathf.Clamp(pos.x,-0.5f,0.5f);
            Progress = pos.x + 0.5f;
            if (pos.x != Button.transform.localPosition.x)
            {
                Button.transform.localPosition = pos;
                Text.text = ((int)(Progress * 100)) + "%";
                EventController.Instance.PostEvent("OnUpdateOptions",gameObject);
            }
        }
    }
}
