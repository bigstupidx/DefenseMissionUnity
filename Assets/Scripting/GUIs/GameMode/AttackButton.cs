using UnityEngine;
using System.Collections;

public class AttackButton : MonoBehaviour, IEventSubscriber
{
    private float _timer;
    public float DistanceToAtack = 5000;

    public bool _update = false;

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "MissionFinished")
        {
            renderer.enabled = false;
            collider.enabled = false;
            _update = true;
            return;
        }

        if (_timer + 1 < Time.time)
        {
            switch (EventName)
            {
                case "AttackButtonPressed":
                    EventController.Instance.PostEvent("StartRocket", null);
                    break;
            }
            _timer = Time.time;
        }
    }

    void Awake()
    {
        _timer = Time.time;
        EventController.Instance.SubscribeToAllEvents(this);
    }

    #endregion

    void Update()
    {
        if (_update)
            return;
        if (Radar.Instance.DistanceToTarget < this.DistanceToAtack && !this.renderer.enabled
            && MissionController.Instance.CurrentState.Type == MissionStateType.Destroy)
        {
            renderer.enabled = true;
            collider.enabled = true;
        }

        if ((Radar.Instance.DistanceToTarget > this.DistanceToAtack && this.renderer.enabled) 
            || MissionController.Instance.CurrentState.Type != MissionStateType.Destroy)
        {
            renderer.enabled = false;
            collider.enabled = false;
        }
    }
}
