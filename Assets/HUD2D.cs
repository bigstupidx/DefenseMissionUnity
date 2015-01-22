using UnityEngine;
using System.Collections;

public class HUD2D : MonoBehaviour
{
    public float HeightSizeDisplay = 10f;

    public float HeightYOffset;

    public Transform NumberHeightRoot;
    public GameObject NumberPointPrefab;

    private const float NumberOffset = 100f;
    private const float NumberOffsetHeight = 0.9f;



	void Start ()
	{
	    GenerateHeightIndicator();
	}
	
	void Update ()
	{
	    UpdateHeightIndicator();
	}

    private void GenerateHeightIndicator()
    {
        for (int i = -10; i < 100; i++)
        {
            GameObject numberPoint = GameObject.Instantiate(NumberPointPrefab) as GameObject;
            numberPoint.transform.parent = NumberHeightRoot;
            numberPoint.transform.localPosition = new Vector3(0, i * NumberOffsetHeight, 0);
            numberPoint.transform.localEulerAngles = new Vector3(0, 0, 0);
            numberPoint.transform.localScale = new Vector3(1, 1, 1);
            numberPoint.SetActive(true);

            numberPoint.GetComponentInChildren<TextMesh>().text = (i * NumberOffset).ToString();
        }
    }

    private void UpdateHeightIndicator()
    {
        float height = Mathf.Clamp(AirplaneController.Instance.transform.position.y * 1.28084f, 0, 1006660f);
        NumberHeightRoot.transform.localPosition = new Vector3(NumberHeightRoot.transform.localPosition.x, -(height / NumberOffset) * NumberOffsetHeight, NumberHeightRoot.transform.localPosition.z);
    }
}
