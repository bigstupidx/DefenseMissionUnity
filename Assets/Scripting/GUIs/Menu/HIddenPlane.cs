using UnityEngine;
using System.Collections;

public class HIddenPlane : GUIObject 
{
    public Color StartColor;
    public Color EndColor;
    public float AnimTime = 3;

    protected override void EventProc(string EventName, GameObject Sender)
    {
        base.EventProc(EventName, Sender);
        if (EventName == base.ShowOnEvent)
        {
            renderer.material.SetColor("_Color",StartColor);
            StartCoroutine(Anim());
        }
    }

    IEnumerator Anim()
    {
        float time = UnityEngine.Time.time;
        float delta = 0;
        while ((delta = Time.time - time) < AnimTime)
        {
            Color col = Color.Lerp(StartColor,EndColor,delta/AnimTime);
            renderer.material.SetColor("_Color",col);
            yield return new WaitForEndOfFrame();
        }
        renderer.material.SetColor("_Color",EndColor);
    }
}
