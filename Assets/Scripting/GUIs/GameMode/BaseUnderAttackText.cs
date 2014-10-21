using System;
using DN;
using UnityEngine;
using System.Collections;

public class BaseUnderAttackText : MonoBehaviour, IEventSubscriber {

    public AnimationCurve TextAlphaCurve;

    private Timer _hideRendererTimer;
    private TextMesh mText;

    // Use this for initialization
    void Start()
    {
        EventController.Instance.Subscribe("BaseEntered", this);
        EventController.Instance.Subscribe("BaseExit", this);
        _hideRendererTimer = new Timer { Infinite = true};

        mText = GetComponent<TextMesh>();
        mText.renderer.enabled = false;
    }



    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "BaseEntered")
        {
            ShowText();
        }
        else if (EventName == "BaseExit")
        {
            HideText();
        }
    }


    private void ShowText()
    {
        renderer.enabled = true;
        _hideRendererTimer.Run();
    }

    private void HideText()
    {
        renderer.enabled = false;
        _hideRendererTimer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        _hideRendererTimer.Update(Time.deltaTime);
        if (_hideRendererTimer.IsRunning)
        {
            Color color = mText.color;
            color.a = TextAlphaCurve.Evaluate(_hideRendererTimer.Elapsed);
            mText.color = color;
        }

    }

}
