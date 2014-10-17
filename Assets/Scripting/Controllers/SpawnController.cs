using UnityEngine;
using System.Collections;

public enum Airplanes
{
    None,
    F_16,
    FA_22,
    FA_38,
    Mirage,
    SAAB
}

public class SpawnController : MonoBehaviour 
{
    public Airplanes SpawnAirplane;
    public Transform AirplaneSpawnPoint;

    public GameObject F16_Prefab;
    public GameObject FA22_Prefab;
    public GameObject FA38_Prefab;
    public GameObject Mirage_Prefab;
    public GameObject SAAB_Prefab;

    void Awake()
    {
        if (TransportGOController.Instance &&
            TransportGOController.Instance.SelectedPlane != Airplanes.None)
            SpawnAirplane = TransportGOController.Instance.SelectedPlane;

        GameObject prefab = null;
        switch (SpawnAirplane)
        {
            case Airplanes.F_16:
                prefab = F16_Prefab;
                break;
            case Airplanes.FA_22:
                prefab = FA22_Prefab;
                break;
            case Airplanes.FA_38:
                prefab = FA38_Prefab;
                break;
            case Airplanes.Mirage:
                prefab = Mirage_Prefab;
                break;
            case Airplanes.SAAB:
                prefab = SAAB_Prefab;
                break;
        }

        prefab = GameObject.Instantiate(prefab, AirplaneSpawnPoint.position, AirplaneSpawnPoint.localRotation) as GameObject;
        CameraController.Instance.Target = prefab.transform;

        GameObject.Destroy(this);
    }
}
