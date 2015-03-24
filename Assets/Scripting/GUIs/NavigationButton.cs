using UnityEngine;
using System.Collections;

public class NavigationButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (OptionsController.Instance.Tilt)
	    {
	        if (renderer.enabled)
	        {
	            renderer.enabled = false;
	            collider.enabled = false;
	        }
	    }
	    else
	    {
	        if (!renderer.enabled)
	        {
	            renderer.enabled = true;
	            collider.enabled = true;
	        }
	    }
	}
}
