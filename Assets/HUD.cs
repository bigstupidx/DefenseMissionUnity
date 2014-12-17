using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    public TextMesh Speed;
    public TextMesh Height;
    public Transform Rotator;
    public Transform Center;
    public Transform Top;
    public Transform Bottom;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    Speed.text = AirplaneController.Instance.CurrentSpeed.ToString();
	    Height.text = ((int)(AirplaneController.Instance.transform.position.y*1.28084f)).ToString();

        var rotation = Center.rotation.eulerAngles;
	    rotation.z = -transform.rotation.z*0.5f;
        Center.eulerAngles = rotation;


	    if (rotation.x < 360 && rotation.x > 260)
	    {
	        rotation.x = rotation.x - 360;

	    }
        else
	    if (rotation.x > 20)
	    {
	        rotation.x = 20;

	    }

        float ceof = ((rotation.x + 20f)/40);

        var pos = Center.localPosition;

	    pos.y = Mathf.Lerp(Top.localPosition.y, Bottom.localPosition.y, 1f - ceof)*1f;

        Center.localPosition = pos;
	}
}
