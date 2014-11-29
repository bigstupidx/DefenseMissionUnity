using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreCloseScreen : MonoBehaviour, IEventSubscriber
{
	public List<PreCloseScreenImageElement> Images;
	
	private bool _placedImages;

	void Start()
	{
		if(PrecloseScreenIAS.Instance.preReady)
		{
			PlaceImages();
		}

		EventController.Instance.Subscribe("LoadedIAS", this);
	}

	void Update()
	{
		if(!_placedImages)
		{
			if(PrecloseScreenIAS.Instance.preReady)
			{
				PlaceImages();
			}
		}
	}

	#region IEventSubscriber implementation
	public void OnEvent (string EventName, GameObject Sender)
	{
		if(EventName == "OnQuitGame")
		{
			Debug.Log("Quit game");
			Application.Quit();
		}
		else if(EventName == "RateGame")
		{
			Debug.LogWarning("SET GOOD BUNDLE VERSION");
			Application.OpenURL("market://details?id=com.supercell.clashofclans");	//TODO: ADD BUNDLE ID !!!!!!!
		}
		else if(EventName == "OnPrecloseAdClick")
		{
			ShowAdd();
		}
		else if(EventName == "OnClosePreCloseScreen")
		{
			Close ();
		}
	}

	void ShowAdd ()
	{
		throw new System.NotImplementedException ();
	}

	private void PlaceImages()
	{
		_placedImages = true;
		foreach (var texture in PrecloseScreenIAS.Instance.preBannerTextures) 
		{
			var imageElement = Images.Find(p => !p.Placed);
			if(imageElement == null)
			{
				break;
			}
			imageElement.Image.material.mainTexture = texture;
			imageElement.Placed = true;
		}
	}

	void Close ()
	{
		foreach (Transform t in transform) 
		{
			t.GetComponent<MeshRenderer> ().enabled = false;
			t.GetComponent<BoxCollider> ().enabled = false;
		}
		transform.GetComponent<MeshRenderer> ().enabled = false;
		transform.GetComponent<BoxCollider> ().enabled = false;
	}
	#endregion
}
