using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class IABUIManager : MonoBehaviourGUI
{
#if UNITY_ANDROID
	void OnGUI()
	{
		beginColumn();

		if( GUILayout.Button( "Initialize IAB" ) )
		{
			var key = "your public key from the Android developer portal here";
            key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgi0yiDopN5gCvXpzfEOVTSw2Z6owTN/4sk/xya3yHlG/fcsiDqRaWJvEHRjRIBvkNiEVL5s3qehtpQ+IkisTc7Z8bhs91hHqdOt7dB5IgSA6C7+yhWnxPZ8muHc/kk6BYJNwD9JNe/jXt373It0m25X3In2BE2X/tJuadeIT859ROKPzFzanSM5+q8dqQQCZq38YjN52IdxmqAlpcv1ZzbNCsfi30GNLSFO05iuhCwjbGCVF6aGJKzCzoFnM9PXhucRKSPxetKbTb9UmqOow5uLL2ZD9/D9DHRZS/RUvIFCP6mRX/vIqntnmPOMepQgD2k07RFsyANF+4UlJi7xF9QIDAQAB";
			GoogleIAB.init( key );
		}


		if( GUILayout.Button( "Query Inventory" ) )
		{
			// enter all the available skus from the Play Developer Console in this array so that item information can be fetched for them
			//var skus = new string[] { "com.prime31.testproduct", "android.test.purchased", "com.prime31.managedproduct", "com.prime31.testsubscription" };
            var skus = new string[] { "airplane_f16", "airplane_f22" };
			GoogleIAB.queryInventory( skus );
		}


		if( GUILayout.Button( "Are subscriptions supported?" ) )
		{
			Debug.Log( "subscriptions supported: " + GoogleIAB.areSubscriptionsSupported() );
		}


		if( GUILayout.Button( "Purchase Test Product" ) )
		{
			GoogleIAB.purchaseProduct( "airplane_f16" );
		}


		if( GUILayout.Button( "Consume Test Purchase" ) )
		{
			GoogleIAB.consumeProduct( "android.test.purchased" );
		}


		if( GUILayout.Button( "Test Unavailable Item" ) )
		{
			GoogleIAB.purchaseProduct( "android.test.item_unavailable" );
		}


		endColumn( true );


		if( GUILayout.Button( "Purchase Real Product" ) )
		{
			GoogleIAB.purchaseProduct( "com.prime31.testproduct", "payload that gets stored and returned" );
		}


		if( GUILayout.Button( "Purchase Real Subscription" ) )
		{
			GoogleIAB.purchaseProduct( "com.prime31.testsubscription", "subscription payload" );
		}


		if( GUILayout.Button( "Consume Real Purchase" ) )
		{
			GoogleIAB.consumeProduct( "com.prime31.testproduct" );
		}


		if( GUILayout.Button( "Enable High Details Logs" ) )
		{
			GoogleIAB.enableLogging( true );
		}


		if( GUILayout.Button( "Consume Multiple Purchases" ) )
		{
			var skus = new string[] { "com.prime31.testproduct", "android.test.purchased" };
			GoogleIAB.consumeProducts( skus );
		}

		endColumn();
	}
#endif
}
