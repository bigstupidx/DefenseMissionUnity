using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class PlaneMissionsList : MonoBehaviour, IEventSubscriber
{
    public GameObject BackPlanePrefab;
    public GameObject MissionPreTitlePrefab;
    public GameObject MissionTitlePrefab;
    public GameObject MissionTextPrefab;
    public GameObject MissionNumberPrefab;
    public GameObject LockPlanePrefab;
    public GameObject LockerPrefab;
    public GameObject PlayButtonPrefab;
    public Texture MainPlaneTexture;
    public Texture ActivePlaneTexture;

    public int SelectedMission;

    private List<GameObject> _missionPlanes;


    private readonly Dictionary<int, string> _missionDescriptions = new Dictionary<int, string>
    {
        {0, @"Learn to fly cadet! Follow the guided route in order to complete the training."},
        {1, @"Ready for the real thing? Fly to the enemy munitions factory and destroy it."},
        {2, @"Stay on the offense, we need to strike at their base!"},
        {3, @"There are reports of a hidden enemy comms tower, we need to find."},
        {4, @"Seek and destroy the enemy comms tower."},
        {5, @"Enemy defenses are getting stronger, we need to be more careful."},
        {6, @"Enemy planes have been spotted near by, destroy them before they take off."},
        {7, @"Oil reserves are being used by the enemy, we need to cut them off."},
        {8, @"Multiple submarines are sighted offshore, take them out!"},
        {9, @"Production seems to have ramped up in some sectors, we need to stop them."},
        {10, @"Their comms are back online, be careful!"},
        {11, @"Enemy defenses are rebuilding, stop them before it’s too late."},
        {12, @"The weather looks to be getting worse but we still need to take down the target."},
        {13, @"More submarines are coming to our shores, go take them out!"},
        {14, @"Enemy planes have been spotted near by, destroy them before they take off."},
        {15, @"Enemies on the ground are gathering in big numbers, Get high and take them down."},
        {16, @"Oil rigs are being built off the coast, stop the enemy supply right now!."},
        {17, @"Enemy reinforcements inbound, take them out!"},
        {18, @"We’re close to pushing them back, let’s keep it going."},
        {19, @"They’re retreating, time to deal the final blow."},
    };


    private readonly Dictionary<int, string> _fullDescriptions = new Dictionary<int, string>
    {
        {0, @"Tutorial- Start on Island Runway,teach player controls, follow waypoints towards target (fake building) to take it out, then follow more waypoints to Island Runway to finish."},
        {1, @"Take off from the Aircraft Carrier Ship, then fly towards enemy factory to take it out, fly back to Aircraft Carrier Ship to finish."},
        {2, @"Take off from the Aircraft Carrier Ship, then fly towards enemy base to take it out, fly to Island Runway to finish."},
        {3, @"Take off from the Island Runway, fly to multiple islands to survey the enemy locations, return to Aircraft Carrier Ship."},
        {4, @"Take off from Aircraft Carrier Ship, fly towards enemy radar tower, must stay below 200 Feet to avoid being detected (If detected, then player fails). Take out radar tower, return to Aircraft Carrier Ship to finish."},
        {5, @"Take off from the Aircraft Carrier Ship (Try to have this moved into a place far from the island), fly a specific route to avoid being hit by enemy gun/tanks (If player gets hit then player fails), take enemy turrets/tanks, fly to Island Runway to finish."},
        {6, @"Take off from the Aircraft Carrier Ship, then fly towards enemy airport to take out multiple planes on the runway (2 or 3 planes), fly back to Aircraft Carrier Ship to finish."},
        {7, @"Take off from the Aircraft Carrier Ship, then fly towards enemy oil rig  to take it out, fly back to Aircraft Carrier Ship to finish."},
        {8, @"Take off from the Aircraft Carrier Ship, then fly towards enemy submarines to take them out, fly to Aircraft Carrier Ship to finish."},
        {9, @"Take off from the Island Runway, then fly towards enemy factories (two factories on two islands to take them out, fly back to Aircraft Carrier Ship to finish."},
        {10, @"Take off from the Aircraft Carrier Ship, fly towards enemy radar tower, must stay below 200 Feet to avoid being detected (If detected, then player fails). Take out radar tower, return to Aircraft Carrier Ship to finish."},
        {11, @"Take off from the Aircraft Carrier Ship, fly a specific route to avoid being hit by enemy gun/tanks (If player gets hit then player fails), take enemy turrets/tanks (More than the previous mission), fly to Aircraft Carrier Ship to finish."},
        {12, @"Take off from the Aircraft Carrier Ship, then fly towards the enemy base to take it out, then take a specific route back to land on the Island Runway."},
        {13, @"Take off from the Aircraft Carrier Ship, then fly towards enemy submarines to take them out, fly to Aircraft Carrier Ship to finish."},
        {14, @"Take off from the Island Runway, then fly towards enemy base to take out multiple planes on the runway (4 planes), fly back to Aircraft Carrier Ship to finish."},
        {15, @"Take off from the Aircraft Carrier Ship, then fly up to altitude of 2000 feet (Have to reach this height before attacking). Take out enemy truck at the enemy base (5 trucks), return to Island runway to finish. "},
        {16, @"Take off from the Aircraft Carrier Ship, then fly towards enemy oil rigs to take it out, fly back to Aircraft Carrier Ship to finish."},
        {17, @"Take off from the Aircraft Carrier Ship, then fly towards enemy base to take out multiple tanks and enemy base. Then make your way to other islands to take out multiple submarines and radar tower. Return to the Island runway to finish."},
        {18, @"Take off from the Aircraft Carrier Ship, In order, take out the enemy factory, oil rigs (2), radar tower, airport, and enemy base. Return to the Aircraft Carrier Ship to finish."},
        {19, @"Take off from the Aircraft Carrier Ship, follow a specific route to avoid tanks and turrets, then fly to altitude of 1500 feet. Then take out multiple targets of turrets, tanks, trucks, submarines (Make it 20 in total, 5 of each) and an enemy base. Then return to the Island runway to finish."},
    };

    void Start()
    {
        _missionPlanes = new List<GameObject>();
        EventController.Instance.Subscribe("OnReLoadMissionsList", this);
        EventController.Instance.Subscribe("OnPressObject", this);
        EventController.Instance.Subscribe("OnReleaseObject", this);
        EventController.Instance.Subscribe("OnTouchMoved", this);
        EventController.Instance.Subscribe("OnShowMissionSelecting", this);

        EventController.Instance.PostEvent("OnReLoadMissionsList", null);
    }

    #region IEventSubscriber implementation
    public void OnEvent(string EventName, GameObject Sender)
    {
        int i;
        switch (EventName)
        {
            case "OnReLoadMissionsList":
                // destory all old planes
                foreach (GameObject p in _missionPlanes)
                {
                    for (i=0;i<p.transform.childCount;i++)
                    {
                        EventController.Instance.Unsubscribe(
                            p.transform.GetChild(i).GetComponent<GUIObject>() as IEventSubscriber);
                        Destroy(p.transform.GetChild(i).gameObject);
                    }
                    EventController.Instance.Unsubscribe(
                        p.GetComponent<GUIObject>() as IEventSubscriber);
                    Destroy(p);
                }
                _missionPlanes.Clear();
                TransportGOController.Instance.SelectedMissionID = SelectedMission;
                for (i=0;i<TransportGOController.Instance.Missions.Length;i++)
                {
                    MissionInfo info = TransportGOController.Instance.Missions[i];
                    GameObject plane = GameObject.Instantiate(BackPlanePrefab) as GameObject;
                    _missionPlanes.Add(plane);
                    plane.transform.parent = transform;
                    plane.transform.localPosition = new Vector3(0,-i*2,-1);
                    plane.transform.localScale = new Vector3(16,1.89f,1);
                    plane.name = "MissionPlane "+(i+1);
                    /*
                    if (i == SelectedMission)
                    {
                        plane.renderer.material.SetTexture("_MainTex",ActivePlaneTexture);
                        GameObject but = GameObject.Instantiate(PlayButtonPrefab) as GameObject;
                        but.transform.parent = plane.transform;
                        but.transform.localPosition = new Vector3(0.25f,0,-1);
                        but.transform.localScale = new Vector3(0.1875f,0.518f,1);
                    }
                    else
                    */
                        plane.renderer.material.SetTexture("_MainTex",MainPlaneTexture);
                    
                    GameObject elem = GameObject.Instantiate(MissionPreTitlePrefab) as GameObject;
                    elem.transform.parent = plane.transform;
                    elem.transform.localPosition = new Vector3(-0.446f,0.1f,-1);
                    elem.transform.localScale = new Vector3(0.009f,0.08f,1);
                    elem.GetComponent<TextMesh>().text = "Mission " + (i + 1);//+" -";

//                    elem = GameObject.Instantiate(MissionTitlePrefab) as GameObject;
//                    elem.transform.parent = plane.transform;
//                    elem.transform.localPosition = new Vector3( i<9 ? -0.238f : -0.22f ,0.16f,-1);
//                    elem.transform.localScale = new Vector3(0.009f,0.08f,1);
//                    elem.GetComponent<TextMesh>().text = info.MissionTitle;

                    elem = GameObject.Instantiate(MissionTextPrefab) as GameObject;
                    elem.transform.parent = plane.transform;
                    elem.transform.localPosition = new Vector3(-0.446f,-0.22f,-1);
                    elem.transform.localScale = new Vector3(0.0064f,0.0544f,1);
                    elem.GetComponent<TextMesh>().text = _missionDescriptions[i]; //info.MissionText;

//                    elem = GameObject.Instantiate(MissionTextPrefab) as GameObject;
//                    elem.transform.parent = plane.transform;
//                    elem.transform.localPosition = new Vector3(0.2505657f, -0.22f, -1);
//                    elem.transform.localScale = new Vector3(0.0064f, 0.0544f, 1);
//                    elem.GetComponent<TextMesh>().text = "Weather: " + WeatherControl.GetWeatherNameForLevel(i);

                    elem = GameObject.Instantiate(MissionNumberPrefab) as GameObject;
                    elem.transform.parent = plane.transform;
                    elem.transform.localPosition = new Vector3(0.4f,0.0f,-1);
                    elem.transform.localScale = new Vector3(0.0064f,0.0544f,1);
                    elem.GetComponent<TextMesh>().text = WinScreen_MoneyText.GetMoneyReward(i).ToString();//info.Payment.ToString();
                    elem.name = "Payment";

//                    elem = GameObject.Instantiate(MissionNumberPrefab) as GameObject;
//                    elem.transform.parent = plane.transform;
//                    elem.transform.localPosition = new Vector3(0.42f,-0.22f,-1);
//                    elem.transform.localScale = new Vector3(0.0064f,0.0544f,1);
//                    elem.GetComponent<TextMesh>().text = info.Distance+"m";
//                    elem.name = "Distance";

                    if (info.Blocked)
                    {
//                        for (int j = 0; j < 3; j++)
//                        {
//                            elem = GameObject.Instantiate(LockPlanePrefab) as GameObject;
//                            elem.transform.parent = plane.transform;
//                            elem.transform.localPosition = new Vector3(0, 0, -2);
//                            elem.transform.localScale = new Vector3(1, 1, 1);
//                        }

                        elem = GameObject.Instantiate(MissionTextPrefab) as GameObject;
                        elem.transform.parent = plane.transform;
                        elem.transform.localPosition = new Vector3(-0.1f, 0, -3.5f);
                        elem.transform.localScale = new Vector3(0.0064f, 0.0544f, 1);
                        elem.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                        
                        elem.GetComponent<TextMesh>().fontSize = 96;
                        elem.GetComponent<TextMesh>().text = "LOCKED!";//info.MissionText;

//
                        elem = GameObject.Instantiate(LockerPrefab) as GameObject;
                        elem.transform.parent = plane.transform;
                        elem.transform.localPosition = new Vector3(0,0,-3);
                        elem.transform.localScale = new Vector3(1,0.93f,1);
                    }
                }

                break;

            case "OnPressObject":
                for (i=0;i<this._missionPlanes.Count;i++)
                    if (Sender == this._missionPlanes[i])
                        _touch = true;
                break;

            case "OnTouchMoved":
                if (!_drag)
                {
                    _drag = true;
                    _dragStart = MenuController.Instance.Cam2D.ScreenPointToRay(
                        MenuController.Instance.LastStartedGesture.EndPoint).origin.y;
                    _dragStartPos = transform.position;
                }

                break;

            case "OnReleaseObject":
                for (i=0;i<this._missionPlanes.Count;i++)
                    if (Sender == this._missionPlanes[i] && !_drag && !TransportGOController.Instance.Missions[i].Blocked)
                {
                    //SelectedMission = i;
                    TransportGOController.Instance.SelectedMissionID = i;
                    EventController.Instance.PostEvent("OnHideGUI", null);
                    EventController.Instance.PostEvent("OnShowLoading", null);
                    EventController.Instance.PostEvent("OnShowGameMode", null);
                    break;
                }
                _drag = false;
                _touch = false;

                break;

            case "OnShowMissionSelecting":
                EventController.Instance.PostEvent("OnReLoadMissionsList", null);
                break;
        }
    }
    #endregion

    private float _dragStart;
    private Vector3 _dragStartPos;
    private bool _touch = false;
    private bool _drag;
    private float _dragVelocity = 0;

    void Update()
    {
        if (_drag)
        {
            Vector3 pos = _dragStartPos;
            Vector3 dragEnd = MenuController.Instance.Cam2D.ScreenPointToRay(
                MenuController.Instance.LastStartedGesture.EndPoint).origin;
            pos.y += (_dragVelocity = dragEnd.y - _dragStart);
            float a = transform.parent.InverseTransformPoint(pos).y;
            if (a >= 3 && a <= (TransportGOController.Instance.Missions.Length - 2.5f) * 2)
                transform.position = pos;
            _dragVelocity *= 0.5f;
        }
        // получилась не очень качественная листалка
        /* else if (Mathf.Abs(this._dragVelocity) > 0.1f)
        {
            Vector3 pos = transform.position + new Vector3(0,this._dragVelocity,0);
            float a = transform.parent.InverseTransformPoint(pos).y;
            if (a >= 3 && a <= (TransportGOController.Instance.Missions.Length - 2.5f) * 2)
                transform.position = pos;
            _dragVelocity *= 0.95f;
        } */
    }
}
