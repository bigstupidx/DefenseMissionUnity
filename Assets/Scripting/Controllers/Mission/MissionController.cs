using UnityEngine;
using System;

[Serializable]
public enum MissionStateType
{
    Takeoff,
    Transport,
    Observation,
    Destroy,
    Landing
}

[Serializable]
public class MissionState
{
    public MissionStateType Type;
    public MissionObject Target;
    public float FParam;
    public string MissionStateText;
}

public class MissionController : MonoBehaviour, IEventSubscriber
{
    #region IEventSubscriber implementation


    public void OnEvent(string EventName, GameObject Sender)
    {
        //print(string.Format("Event '{0}', sender {1}", EventName, Sender.name));
        //MissionObject o = Sender.GetComponent<MissionObject>();
        switch (EventName)
        {
            case "Takeoff":
                if (CurrentState.Type == MissionStateType.Takeoff)
                    GoToNextState();
                break;
            case "Landing":
                //if (CurrentState.Type == MissionStateType.Landing && o == CurrentTarget)
             //       GoToNextState();
                break;
            case "Crash":
                if (!Finished)
                {
                    EventController.Instance.PostEvent("MissionFailed", gameObject);
                    Failed = true;
                }
                break;
            case "ViewZoneEnter":
                _viewZoneTimer = 0;
                break;
            case "ViewZoneExit":
                _viewZoneTimer = -1;
                break;
            case "MissionObjectDestroyed":
                //if (CurrentState.Type == MissionStateType.Destroy && o == CurrentTarget)
                //    GoToNextState();
                break;
            case "MissionFinished":
                Finished = true;
                break;
        }
    }

    #endregion

    public bool Finished = false;
    public bool Failed = false;
    public float FinalPayment = 100;

    public static MissionController Instance { get; private set; }

    public MissionController()
    {
        Instance = this;
    }

    public MissionState[] States;

    private int _currentState = 0;
    private float _viewZoneTimer = -1;

    public MissionObject CurrentTarget 
    { 
        get 
        { 
            return _currentState<States.Length ? States [_currentState].Target :
                States[States.Length-1].Target; 
        } 
    }

    public MissionState CurrentState 
    { 
        get 
        { 
            return _currentState<States.Length ? States [_currentState] :
                States[States.Length-1];
        } 
    }

    private void GoToNextState() 
    {
        _currentState++;
        EventController.Instance.PostEvent("MissionChangeTarget",gameObject);
        if (_currentState == States.Length)
        {
            Finished = true;
            EventController.Instance.PostEvent("MissionFinished",gameObject);
        }
        if (_currentState == 1 && States.Length != 1)
        {
            EventController.Instance.PostEvent("MissionStarted",gameObject);
        }
    }

    void Awake()
    {
        int id = TransportGOController.Instance.SelectedMissionID;
        Debug.Log("Mission " + id + " loading...");

        int count = 0;
        while (TransportGOController.Instance.Missions[id].Targets[count].ID!=-1)
            count++;

        States = new MissionState[count + 1];

        States [0] = new MissionState();
        States [0].Type = MissionStateType.Takeoff;
        States [0].Target = DataStorageController.GetMissionObjectByID(DataStorageController.Instance.MissionRunwaysID [0].ID)  ;
        States [0].MissionStateText = "Take off from runway";

        for (int i=1; i<States.Length; i++)
        {
            States [i] = new MissionState();
            States [i].Type = MissionStateType.Destroy;
            MissionObjectData data = TransportGOController.Instance.Missions [id].Targets [i - 1];
            States [i].Target = DataStorageController.GetMissionObjectByID(data.ID);
            States[i].MissionStateText = "Destroy enemy tank!";
                //TransportGOController.Instance.Missions [id].Targets[i-1].Objective;
        }

        FinalPayment = TransportGOController.Instance.Missions [id].Payment;

        EventController.Instance.Subscribe("Takeoff", this);
        EventController.Instance.Subscribe("Landing", this);
        EventController.Instance.Subscribe("Crash", this);
        EventController.Instance.Subscribe("ViewZoneEnter", this);
        EventController.Instance.Subscribe("ViewZoneExit", this);
        EventController.Instance.Subscribe("MissionObjectDestroyed", this);
        EventController.Instance.Subscribe("MissionFinished", this);
    }

    void Start()
    {
        EventController.Instance.PostEvent("MissionChangeTarget",gameObject);
    }

    void Update()
    {
        if (_viewZoneTimer >= 0 && CurrentState.Type == MissionStateType.Observation)
        {
            _viewZoneTimer+=Time.deltaTime;
            if (_viewZoneTimer > CurrentState.FParam)
            {
                _viewZoneTimer = -1;
                GoToNextState();
            }
        }
    }
}
