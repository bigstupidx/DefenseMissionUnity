using UnityEngine;
using System.Collections;

public class BriefScreen : MonoBehaviour, IEventSubscriber {

	// Use this for initialization
	void Start ()
	{
	    Time.timeScale = 0f;
        EventController.Instance.Subscribe("OnBriefHide", this);
	}

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "OnBriefHide")
        {
            Debug.Log("YEah");
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }
    }
}
