using UnityEngine;
using System.Collections;

public class BriefScreen : MonoBehaviour, IEventSubscriber
{

    public TextMesh TextMesh;

	// Use this for initialization
	void Start ()
	{
        if(TransportGOController.Instance.SelectedMissionID == 0)
        {
            Time.timeScale = 0f;
            EventController.Instance.Subscribe("OnBriefHide", this);
            EventController.Instance.Subscribe("OnBriefHideTilt", this);
            EventController.Instance.Subscribe("OnBriefHideJoy", this);
        }
        else
        {
            Destroy(gameObject);
        }
	}

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "OnBriefHide")
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }
        if (EventName == "OnBriefHideTilt")
        {
            OptionsController.Instance.Tilt = true;
            PlayerPrefs.SetInt("Tilt", OptionsController.Instance.Tilt ? 1 : 0);
        }
        if (EventName == "OnBriefHideJoy")
        {
            OptionsController.Instance.Tilt = false;
            PlayerPrefs.SetInt("Tilt", OptionsController.Instance.Tilt ? 1 : 0);
        }
    }
}
