using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MilitaryDemo;

public class WeatherControl : MonoBehaviour
{
    public GameObject AttachTo;
    public Light MainLight;

    public GameObject SunnyPrefab;
    public GameObject RainyPrefab;
    public GameObject VeryRainyPrefab;
    public GameObject SnowyPrefab;

    public List<WeatherData> Data;

    public Renderer Sky;



	void Start ()
	{
	    var data = Data.GetRandom();
	    var weatherObject = CreateWeatherEffect(data.Type);

	    if (weatherObject != null)
	    {
	        weatherObject.transform.parent = AttachTo.transform;
	        weatherObject.transform.localPosition = data.Positon;
	    }
//	    MainLight.color = data.LightColor;
//	    MainLight.intensity = data.Intensity;
//
//        Sky.sharedMaterial.SetFloat("_Blend", data.Skyblend);
	}

    private GameObject CreateWeatherEffect(TWeather weatherType)
    {
        GameObject prefab = null;
        switch (weatherType)
        {
            case TWeather.Sunny:
                prefab = SunnyPrefab;
                break;
            case TWeather.Rainy:
                prefab = RainyPrefab;

                break;
            case TWeather.VeryRainy:
                prefab = VeryRainyPrefab;

                break;
            case TWeather.Snowy:
                prefab = SnowyPrefab;
                break;
            default:
                throw new ArgumentOutOfRangeException("weatherType");
        }

        if (prefab == null)
        {
            return null;
        }

        return GameObject.Instantiate(prefab) as GameObject;
    }

    // Update is called once per frame
	void Update () {
	
	}

    public enum TWeather
    {
        Sunny,
        Rainy,
        VeryRainy,  
        Snowy
    }

    [Serializable]
    public class WeatherData
    {
        public float Intensity;
        public Color LightColor;
        public TWeather Type;

        public float Skyblend;

        public Vector3 Positon;
    }
}
