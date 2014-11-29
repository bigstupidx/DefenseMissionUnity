using UnityEngine;
using System.Collections;
using MilitaryDemo;

public class EnemyModelChooser : MonoBehaviour {

	public GameObject[] EnemyVariants;


	// Use this for initialization
	void Start () 
	{
		GameObject prefab = RandomTool.NextChoice(EnemyVariants);
		GameObject model = Instantiate(prefab) as GameObject;
		model.transform.parent = transform;
		model.transform.localPosition = new Vector3();
		model.transform.localRotation = Quaternion.identity;
		model.transform.localScale = new Vector3(1,1,1);

	}
}
