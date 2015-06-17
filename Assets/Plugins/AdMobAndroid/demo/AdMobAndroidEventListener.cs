using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AdMobAndroidEventListener : MonoBehaviour
{
    public TextMesh Log;
    public static AdMobAndroidEventListener Instance;
#if UNITY_ANDROID

    void Start()
    {
        AdMobAndroid.init("pub-9255742339770963");
        //AdMobAndroid.createBanner("ca-app-pub-9255742339770963/5809451896",
          //                        AdMobAndroidAd.tablet300x250, AdMobAdPlacement.Centered );

        AdMobAndroid.requestInterstital("ca-app-pub-9255742339770963/2137268291");
        Instance = this;
    }

	void OnEnable()
	{
		// Listen to all events for illustration purposes
		AdMobAndroidManager.dismissingScreenEvent += dismissingScreenEvent;
		AdMobAndroidManager.failedToReceiveAdEvent += failedToReceiveAdEvent;
		AdMobAndroidManager.leavingApplicationEvent += leavingApplicationEvent;
		AdMobAndroidManager.presentingScreenEvent += presentingScreenEvent;
		AdMobAndroidManager.receivedAdEvent += receivedAdEvent;
		AdMobAndroidManager.interstitialDismissingScreenEvent += interstitialDismissingScreenEvent;
		AdMobAndroidManager.interstitialFailedToReceiveAdEvent += interstitialFailedToReceiveAdEvent;
		AdMobAndroidManager.interstitialLeavingApplicationEvent += interstitialLeavingApplicationEvent;
		AdMobAndroidManager.interstitialPresentingScreenEvent += interstitialPresentingScreenEvent;
		AdMobAndroidManager.interstitialReceivedAdEvent += interstitialReceivedAdEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		AdMobAndroidManager.dismissingScreenEvent -= dismissingScreenEvent;
		AdMobAndroidManager.failedToReceiveAdEvent -= failedToReceiveAdEvent;
		AdMobAndroidManager.leavingApplicationEvent -= leavingApplicationEvent;
		AdMobAndroidManager.presentingScreenEvent -= presentingScreenEvent;
		AdMobAndroidManager.receivedAdEvent -= receivedAdEvent;
		AdMobAndroidManager.interstitialDismissingScreenEvent -= interstitialDismissingScreenEvent;
		AdMobAndroidManager.interstitialFailedToReceiveAdEvent -= interstitialFailedToReceiveAdEvent;
		AdMobAndroidManager.interstitialLeavingApplicationEvent -= interstitialLeavingApplicationEvent;
		AdMobAndroidManager.interstitialPresentingScreenEvent -= interstitialPresentingScreenEvent;
		AdMobAndroidManager.interstitialReceivedAdEvent -= interstitialReceivedAdEvent;
	}



	void dismissingScreenEvent()
	{
        if (Log)
		    Log.text = "dismissingScreenEvent" ;
	}


	void failedToReceiveAdEvent( string error )
	{
        if (Log)
		    Log.text =  "failedToReceiveAdEvent: " + error ;
	}


	void leavingApplicationEvent()
	{
        if (Log)
		    Log.text =  "leavingApplicationEvent" ;
	}


	void presentingScreenEvent()
	{
        if (Log)
		    Log.text =  "presentingScreenEvent" ;
	}


	void receivedAdEvent()
	{
        if (Log)
		    Log.text =  "receivedAdEvent" ;
	}


	void interstitialDismissingScreenEvent()
	{
        if (Log)
		    Log.text =  "interstitialDismissingScreenEvent" ;
	}


	void interstitialFailedToReceiveAdEvent( string error )
	{
        if (Log)
		    Log.text =  "interstitialFailedToReceiveAdEvent: " + error ;
	}


	void interstitialLeavingApplicationEvent()
	{
        if (Log)
		    Log.text =  "interstitialLeavingApplicationEvent" ;
	}


	void interstitialPresentingScreenEvent()
	{
        if (Log)
		    Log.text =  "interstitialPresentingScreenEvent" ;
	}


	void interstitialReceivedAdEvent()
	{
        if (Log)
		    Log.text =  "interstitialReceivedAdEvent" ;
	    if (_shouldShow)
	    {
	        if (AdMobAndroid.isInterstitalReady())
	        {
	            AdMobAndroid.displayInterstital();
	            _shown = true;
	        }
	    }

	    _canShow = true;
	}

    private bool _shown;
    private bool _shouldShow;
    private bool _canShow;
    public void ShowAd(bool force = false)
    {
        if (force)
        {
            _shown = false;
        }
        if (!_shown)
        {
            _shouldShow = true;
            Log.text = "Shouyld show";
            if (_canShow)
            {
                Log.text = "Can show";

                if (AdMobAndroid.isInterstitalReady())
                {
                    AdMobAndroid.displayInterstital();
                    _shown = true;
                }
            }
        }
    }

    public void DontShow()
    {
        _shouldShow = false;
    }
#endif
}


