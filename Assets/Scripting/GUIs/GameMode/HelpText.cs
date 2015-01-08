﻿using UnityEngine;
using System.Collections;

public class HelpText : MonoBehaviour, IEventSubscriber
{
    public float DistanceFromBase = 100f;

    private TextMesh mText;

    private bool mTargeting;

	void Start ()
	{
	    mText = GetComponent<TextMesh>();
	}
	
    public void OnEvent(string EventName, GameObject Sender)
    {
    }

    public void Update ()
    {
        if (Time.timeScale == 0)
        {
            mText.text = "";
            return;
        }

        float distance = Vector3.Distance(AirplaneController.Instance.transform.position,
            MissionController.Instance.CurrentTarget.transform.position);

        if (MissionController.Instance.CurrentState.Type == MissionStateType.Landing &&
            AirplaneController.Instance.State == AirplaneStates.Fly)
        {
            if (distance < DistanceFromBase)
            {
                mText.text = "Slow down!";
            }
            else
            {
                mText.text = "Fly to the base!";
            }
        }
        else
        {
            mText.text = "";
        }
    }


}
