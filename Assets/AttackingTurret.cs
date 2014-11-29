using UnityEngine;
using System.Collections;
using MilitaryDemo;

public class AttackingTurret : MonoBehaviour 
{
	public float Distance = 7000;
	public float ShootingRate = 3.0f;
	public Vector2 ShootingRateRandom = new Vector2(0, 2f);


	[SerializeField]
	private Transform spawnPoint;
		
	[SerializeField]
	private GameObject fireEffect;

	[SerializeField]
	private GameObject bulletEffect;

	private float _elapsed;
	
	void Update () 
	{
		Vector3 targetPosition = AirplaneController.Instance.gameObject.transform.position;	

		var direction = targetPosition - (transform.position - AirplaneController.Instance.CurrentSpeed*AirplaneController.Instance.transform.forward*1f + RandomTool.NextUnitVector3());
		var distance = direction.magnitude;

		var to = Quaternion.LookRotation(direction);
		var result = Quaternion.Lerp(transform.localRotation, to, Time.deltaTime*10);

		transform.rotation = result;

		transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

		if(distance < Distance)
		{
			if(_elapsed > ShootingRate)
			{
				if(RandomTool.NextBool(0.3f))
				{
					_elapsed = RandomTool.NextSingle(ShootingRateRandom.x, ShootingRateRandom.y);
					GameObject bullet = GameObject.Instantiate(bulletEffect, spawnPoint.position, to) as GameObject;

					GameObject.Instantiate(fireEffect, spawnPoint.position, to);
				}
			}
			else
			{
				_elapsed += Time.deltaTime;
			}
		}
	}
}
