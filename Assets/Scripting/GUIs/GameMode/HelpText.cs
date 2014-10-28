using UnityEngine;
using System.Collections;

public class HelpText : MonoBehaviour, IEventSubscriber
{
    public Transform Base;
    public float DistanceFromBase = 100f;

    private TextMesh mText;

    private bool mTargeting;

	void Start ()
	{
	    mText = GetComponent<TextMesh>();
        EventController.Instance.Subscribe("TargetingInProgress", this);
        EventController.Instance.Subscribe("TargetingInProgressEnd", this);
        EventController.Instance.Subscribe("TargetingDeactive", this);
	}
	
    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "TargetingInProgress")
        {
            mTargeting = true;
        }
        else if (EventName == "TargetingInProgressEnd")
        {
            mTargeting = false;
        }
        else if (EventName == "TargetingDeactive")
        {
            mTargeting = false;
        }
    }

    public void Update ()
    {
        if (Time.timeScale == 0)
        {
            mText.text = "";

            return;
        }

        float distance = Vector3.Distance(AirplaneController.Instance.transform.position, Base.transform.position);

        if (distance > DistanceFromBase && renderer.enabled)
        {
            mText.text = "Protect the base!";
        }
        else if (mTargeting && AirplaneController.Instance.State == AirplaneStates.Fly && AirplaneController.Instance.CurrentSpeed > 165f)
        {
            mText.text = "Slow down!";
        }
        else
        {
            mText.text = "";
        }
    }


}
