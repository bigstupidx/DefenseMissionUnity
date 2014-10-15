using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIObject : MonoBehaviour, IEventSubscriber
{
    protected virtual void AwakeProc() {}
    protected virtual void EventProc(string EventName, GameObject Sender) {}
    protected List<string> SubscrabeOnEvents;

    public string ShowOnEvent = "";

    void Awake()
    {
        SubscrabeOnEvents = new List<string>();
        SubscrabeOnEvents.Add("OnHideGUI");
        SubscrabeOnEvents.Add(ShowOnEvent);
        AwakeProc();
        SubscrabeOnEvents = new List<string>(SubscrabeOnEvents.Distinct());
        foreach (string tag in SubscrabeOnEvents)
            EventController.Instance.Subscribe(tag, this);
    }

    public void OnEvent(string EventName, GameObject Sender)
    {
        EventProc(EventName, Sender);
        if (EventName == "OnHideGUI")
        {
            if (renderer)
                renderer.enabled = false;
            if (collider)
                collider.enabled = false;
        } else if (EventName == ShowOnEvent)
        {
            if (renderer)
                renderer.enabled = true;
            if (collider)
                collider.enabled = true;
        }
    }

    void OnDestroy() 
    {
        EventController.Instance.Unsubscribe(this);
    }
}
