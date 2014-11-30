using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreCloseScreen : MonoBehaviour, IEventSubscriber
{
    public GameObject Items;

	public List<PreCloseScreenImageElement> Images;
    [NonSerialized]
    public PreCloseScreen Instance;
	
	private bool _placedImages;
    private float _timeScaleBefore;
	void Start()
	{
	    Instance = this;
	    Close();
		if(PrecloseScreenIAS.Instance.preReady)
		{
			PlaceImages();
		}

		EventController.Instance.Subscribe("LoadedIAS", this);
        EventController.Instance.Subscribe("OnPrecloseAdClick", this);
        EventController.Instance.Subscribe("OnClosePreCloseScreen", this);
        EventController.Instance.Subscribe("RateGame", this);
	}

	void Update()
	{
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        if (Items.activeSelf)
	        {
	            Close();
	        }
	        else
	        {
	            Show();
	        }
	    }

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
            Debug.Log("Show ad");
			ShowAdd();
		}
        else if (EventName == "OnClosePreCloseScreen")
		{
            Debug.Log("Close preclose screen");
			Close ();
		}
	}

	#endregion


    void ShowAdd()
    {
        throw new System.NotImplementedException();
    }

    private void PlaceImages()
    {
        _placedImages = true;
        foreach (var texture in PrecloseScreenIAS.Instance.preBannerTextures)
        {
            var imageElement = Images.Find(p => !p.Placed);
            if (imageElement == null)
            {
                break;
            }
            imageElement.Image.material.mainTexture = texture;
            imageElement.GuiObject.MainTexture = texture as Texture2D;
            imageElement.GuiObject.ActiveTexture = texture as Texture2D; 
            imageElement.Placed = true;
        }
    }

    private void Show()
    {
        _timeScaleBefore = Time.timeScale;
        Time.timeScale = 0;
        Items.SetActive(true);
    }

    void Close()
    {
        Time.timeScale = _timeScaleBefore;
        Items.SetActive(false);
    }
}
