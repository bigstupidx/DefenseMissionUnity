using UnityEngine;
using System.Collections;

public class Button : GUIObject
{
    public Texture2D MainTexture = null;
    public Texture2D ActiveTexture = null;

    public string[] CallWhenPress;

    protected override void AwakeProc()
    {
        base.AwakeProc();
        SubscrabeOnEvents.Add("OnPressObject");
        SubscrabeOnEvents.Add("OnReleaseObject");
        renderer.material.SetTexture("_MainTex", MainTexture);
    }

    private GameObject LastPressedObj;
    protected override void EventProc(string EventName, GameObject Sender)
    {
        base.EventProc(EventName, Sender);

        switch (EventName)
        {
            case "OnPressObject":
                LastPressedObj = Sender;
                if (Sender == gameObject)
                {
                    renderer.material.SetTexture("_MainTex", ActiveTexture);
                    if (Sender == gameObject)
                        EventController.Instance.PostEvent("OnPlayButtonPress",null);
                }
                break;

            case "OnReleaseObject":
                renderer.material.SetTexture("_MainTex", MainTexture);
                if (Sender == gameObject && LastPressedObj == gameObject)
                {
                    EventController.Instance.PostEvent("OnPlayButtonRelease",null);
                    foreach (string e in CallWhenPress)
                        EventController.Instance.PostEvent(e,gameObject);
                }
                break;
        }
    }
}
