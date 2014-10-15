using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
                    elem.GetComponent<TextMesh>().text = "Mission "+(i+1)+" -";

                    elem = GameObject.Instantiate(MissionTitlePrefab) as GameObject;
                    elem.transform.parent = plane.transform;
                    elem.transform.localPosition = new Vector3( i<9 ? -0.238f : -0.22f ,0.16f,-1);
                    elem.transform.localScale = new Vector3(0.009f,0.08f,1);
                    elem.GetComponent<TextMesh>().text = info.MissionTitle;

                    elem = GameObject.Instantiate(MissionTextPrefab) as GameObject;
                    elem.transform.parent = plane.transform;
                    elem.transform.localPosition = new Vector3(-0.446f,-0.22f,-1);
                    elem.transform.localScale = new Vector3(0.0064f,0.0544f,1);
                    elem.GetComponent<TextMesh>().text = info.MissionText;

                    elem = GameObject.Instantiate(MissionNumberPrefab) as GameObject;
                    elem.transform.parent = plane.transform;
                    elem.transform.localPosition = new Vector3(0.42f,0.22f,-1);
                    elem.transform.localScale = new Vector3(0.0064f,0.0544f,1);
                    elem.GetComponent<TextMesh>().text = info.Payment.ToString();
                    elem.name = "Payment";

                    elem = GameObject.Instantiate(MissionNumberPrefab) as GameObject;
                    elem.transform.parent = plane.transform;
                    elem.transform.localPosition = new Vector3(0.42f,-0.22f,-1);
                    elem.transform.localScale = new Vector3(0.0064f,0.0544f,1);
                    elem.GetComponent<TextMesh>().text = info.Distance+"m";
                    elem.name = "Distance";

                    if (info.Blocked)
                    {
                        elem = GameObject.Instantiate(LockPlanePrefab) as GameObject;
                        elem.transform.parent = plane.transform;
                        elem.transform.localPosition = new Vector3(0,0,-2);
                        elem.transform.localScale = new Vector3(1,1,1);

                        elem = GameObject.Instantiate(LockerPrefab) as GameObject;
                        elem.transform.parent = plane.transform;
                        elem.transform.localPosition = new Vector3(0.4f,0,-3);
                        elem.transform.localScale = new Vector3(0.0788f,0.7455f,1);
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
