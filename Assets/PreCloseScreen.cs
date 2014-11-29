using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreCloseScreen : MonoBehaviour, IEventSubscriber
{
	public List<PreCloseScreenImageElement> Images;

	private bool LoadedIAS;

	void Start()
	{
		if(PrecloseScreenIAS.Instance.preReady)
		{
			PlaceImages();
		}

		EventController.Instance.Subscribe("LoadedIAS", this);
	}

	#region IEventSubscriber implementation
	public void OnEvent (string EventName, GameObject Sender)
	{
		if(EventName == "LoadedIAS")
		{
			PlaceImages();
		}
		else if(EventName == "OnQuitGame")
		{
			Application.Quit();
		}
		else if(EventName == "RateGame")
		{
			Application.OpenURL("market://details?id=com.supercell.clashofclans");	//TODO: ADD BUNDLE ID !!!!!!!
		}
	}

	private void PlaceImages()
	{
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
	#endregion
}
