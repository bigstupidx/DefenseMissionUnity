using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseLevel : MonoBehaviour
{

    public BaseLevel Instance { get; private set; }

    public Transform TakeOff;
    public MissionObject Landing;

    public List<WayPoint> WayPoints;
    public List<WayPoint> ReturnWayPoints;

    public List<MissionObject> Targets;

    public Vector2 Height;

    private List<State> _states;
    private int _currentState;

	// Use this for initialization
	void Start ()
	{
	    Instance = this;

        _states = new List<State>();
        _states.Add(new FollowingWaypoints(WayPoints));
        _states.Add(new FollowingWaypoints(ReturnWayPoints));
	    _Start();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _Update();
	}


    protected virtual void _Start()
    {
    }

    protected virtual void _Update()
    {
        if(CurrentState != null)
        if (CurrentState.Ended)
        {
            _currentState++;
        }
    }

    public MissionObject Target
    {
        get { return null; }
    }

    private State CurrentState 
    {
        get
        {
            if (_currentState >= _states.Count - 1) return null;
            return _states[_currentState];
        }
    }
}

internal abstract class State
{
    public bool Ended { get; protected set; }

    public abstract void Update();


    public abstract MissionObject GetTarget();
}

internal class FollowingWaypoints : State
{
    private readonly List<WayPoint> _wayPoints;
    private int _current;

    public FollowingWaypoints(List<WayPoint> wayPoints)
    {
        _wayPoints = wayPoints;


    }

    public override void Update()
    {
        if ((AirplaneController.Instance.transform.position - _wayPoints[_current].transform.position).magnitude < 1000)
        {
            _current += 1;

            if (_current >= _wayPoints.Count - 1)
            {
                Ended = true;
            }
        }
    }

    public override MissionObject GetTarget()
    {
        return _wayPoints[_current].GetComponent<MissionObject>();
    }
}