using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BaseLevel : MonoBehaviour
{

    public static BaseLevel Instance { get; private set; }

    public Transform TakeOffPos;

    public MissionObject TakeOff;
    public MissionObject Landing;

    public List<WayPoint> WayPoints;
    public List<WayPoint> ReturnWayPoints;

    public List<MissionObject> Targets;

    public bool DeathOutside;

    public float Height;

    private List<State> _states;
    private int _currentState;


    public void NextState()
    {
        _currentState += 1;

        if (CurrentState != null)
        {
            if (CurrentState.Ended)
            {
                NextState();
            }
            else
            {
                CurrentState.Start();
                EventController.Instance.PostEvent("MissionChangeTarget", gameObject);
            }
        }
        else
        {
            EventController.Instance.PostEvent("MissionFinished", gameObject);
        }

    }

	// Use this for initialization
	void Start ()
	{
	    Instance = this;

	    _states = new List<State>
	    {
	        new TakeoffState(TakeOff),
	        new FollowingWaypoints(WayPoints),
	        new DestroyTargetState(Targets),
	        new FollowingWaypoints(ReturnWayPoints),
	        new LandingState(Landing)
	    };

	    CurrentState.Start();

	    _Start();

        EventController.Instance.PostEvent("MissionChangeTarget", gameObject);

	}
	
	// Update is called once per frame
	void Update ()
	{
	    bool death = true;

	    if (AirplaneController.Instance.transform.position.y > Height)
	    {
	        death = true;

	    }
	    else
	    {
	        if (DeathOutside)
	        {
	            foreach (var wayPoint in WayPoints)
	            {
	                if (Vector3.Distance(wayPoint.transform.position, AirplaneController.Instance.transform.position) <
	                    wayPoint.SafeZoneAround)
	                {
	                    death = false;
                        break;
	                }
	            }
	        }
	    }

	    if (death)
	    {

	    }

	    _Update();
	}


    protected virtual void _Start()
    {
    }

    protected virtual void _Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();

            if (CurrentState.Ended)
            {
                NextState();
            }
        }
    }

    public MissionObject Target
    {
        get
        {
            if (CurrentState == null)
                return null;
            
            return CurrentState.GetTarget();
        }
    }

    public State CurrentState 
    {
        get
        {
            if (_currentState > _states.Count - 1) return null;
            return _states[_currentState];
        }
    }


}
