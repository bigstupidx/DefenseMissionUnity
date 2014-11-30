using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreCloseScreen : MonoBehaviour, IEventSubscriber
{
	public List<PreCloseScreenImageElement> Images;
    public PreCloseScreen Instance;
	
	private bool _placedImages;

	void Start()
	{
	    Instance = this;

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
            imageElement.Placed = true;
        }
    }

    void Close()
    {
        gameObject.SetActive(false);
//        foreach (Transform t in transform)
//        {
//            if (t.GetComponent<MeshRenderer>())
//            {
//                t.GetComponent<MeshRenderer>().enabled = false;
//            }
//            if (t.GetComponent<BoxCollider>())
//            {
//                t.GetComponent<BoxCollider>().enabled = false;
//            }
//        }
//        transform.GetComponent<MeshRenderer>().enabled = false;
//        transform.GetComponent<BoxCollider>().enabled = false;
    }
}
