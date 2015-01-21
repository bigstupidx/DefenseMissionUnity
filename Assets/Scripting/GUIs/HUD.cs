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

    public Radar Radar;

    public float HeightSizeDisplay = 10f;
    public Transform NumberHeightRoot;
    public GameObject NumberPointPrefab;

    private const float NumberOffset = 100f;
    private const float NumberOffsetHeight = 2f;

	// Use this for initialization
	void Start ()
	{
	    GenerateHeightIndicator();
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    Speed.text = AirplaneController.Instance.CurrentSpeed.ToString("0.0");
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



	    UpdateHeightIndicator();
	}

    private void GenerateHeightIndicator()
    {
        for (int i = -2; i < 50; i++)
        {
            GameObject numberPoint = GameObject.Instantiate(NumberPointPrefab) as GameObject;
            numberPoint.transform.parent = NumberHeightRoot;
            numberPoint.transform.localPosition = new Vector3(0, i * NumberOffsetHeight, 0);
        }
    }

    private void UpdateHeightIndicator()
    {
        float height = Mathf.Clamp(AirplaneController.Instance.transform.position.y, 0, 1006660f);


        NumberHeightRoot.transform.localPosition = new Vector3(0, (height / NumberOffset) * NumberOffsetHeight, 0);

    }
}
