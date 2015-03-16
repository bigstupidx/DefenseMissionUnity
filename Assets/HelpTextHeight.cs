using UnityEngine;
using System.Collections;

public class HelpTextHeight : MonoBehaviour {


    private TextMesh mText;

    void Start()
    {
        mText = GetComponent<TextMesh>();
        mText.text = "";
    }
	
	// Update is called once per frame
    private void Update()
    {
        if (AirplaneController.Instance.State == AirplaneStates.Fly &&
            MissionController.Instance.CurrentState is FollowingWaypoints)
        {
            if (BaseLevel.Instance.Height != 0)
            {
                mText.text = "Keep your height below " + BaseLevel.Instance.Height + "ft";
            }
            else
            {
                SetDefault();
            }
        }
        else
        {
            SetDefault();
        }
    }

    private void SetDefault()
    {
        if (mText.text != "")
            mText.text = "";
    }
}
