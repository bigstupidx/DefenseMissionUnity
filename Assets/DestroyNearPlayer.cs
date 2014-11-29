using UnityEngine;
using System.Collections;

public class DestroyNearPlayer : MonoBehaviour 
{	
	public float Distance = 500f;

	void Update () 
	{
		Vector3 targetPosition = AirplaneController.Instance.gameObject.transform.position;	
		
		var direction = targetPosition - transform.position;
		var distance = direction.magnitude;

		if(distance < Distance)
		{
			SendMessage("DestroyBall", SendMessageOptions.DontRequireReceiver);
		}
	}
}
