using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GoogleIABEventListener : MonoBehaviour
{

#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
	}


    private void QueryInventory()
    {
        var skus = new string[] { "airplane_f22", "airplane_fa38" };
        GoogleIAB.queryInventory( skus );
    }

	void billingSupportedEvent()
	{
		//Log.text= "billingSupportedEvent" ;
        QueryInventory();
	}


	void billingNotSupportedEvent( string error )
	{
		//Log.text= "billingNotSupportedEvent: " + error ;
	}


	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
	{
		//Log.text= string.Format( "queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count ) ;
		//Prime31.Utils.logObject( purchases );
		//Prime31.Utils.logObject( skus );
        foreach (var data in purchases)
        {
            switch (data.productId)
            {
                case "airplane_f22":
                    TransportGOController.GetPlaneInfo(Airplanes.FA_22).Buyout = false;
                    TransportGOController.GetPlaneInfo(Airplanes.FA_22).Locked = false;
                    break;
                case "airplane_fa38":
                    TransportGOController.GetPlaneInfo(Airplanes.FA_38).Buyout = false;
                    TransportGOController.GetPlaneInfo(Airplanes.FA_38).Locked = false;
                    break;
            }
        }
        EventController.Instance.PostEvent("OnSaveData", null);
        EventController.Instance.PostEvent("OnUpdateGUI", null);

        if (MenuController.Instance.MenuMode == MenuState.AirplaneSelect)
        {
            EventController.Instance.PostEvent("OnHideGUI",null);
            EventController.Instance.PostEvent("OnShowAirplaneSelecting",null);
        }
	}


	void queryInventoryFailedEvent( string error )
	{
		//Log.text= "queryInventoryFailedEvent: " + error ;
	}


	void purchaseCompleteAwaitingVerificationEvent( string purchaseData, string signature )
	{
		//Log.text= "purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature ;
	}
	

	void purchaseSucceededEvent( GooglePurchase purchase )
	{
		//Log.text= "purchaseSucceededEvent: " + purchase ;
        QueryInventory();
	}


	void purchaseFailedEvent( string error )
	{
		//Log.text= "purchaseFailedEvent: " + error ;
        // error = "User canceled. (response: - 1005:User ....."
        // error = "Unable to buy item (response: 7......"

        // buy
        // error = "Null data in IAB result (response: - 1002:....."

        QueryInventory();
	}


	void consumePurchaseSucceededEvent( GooglePurchase purchase )
	{
		//Log.text= "consumePurchaseSucceededEvent: " + purchase ;
        QueryInventory();
	}


	void consumePurchaseFailedEvent( string error )
	{
		//Log.text= "consumePurchaseFailedEvent: " + error ;
	}


#endif
}


