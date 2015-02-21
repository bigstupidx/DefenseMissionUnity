using UnityEngine;
using System.Collections;

public class AttackButton : MonoBehaviour, IEventSubscriber
{
    private float _timer;
    public float DistanceToAtack = 5000;
    public float TimeDelay = 0.5f;

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

        if (_timer + TimeDelay < Time.time)
        {
            switch (EventName)
            {
                case "AttackButtonPressed":
                    _timer = Time.time;
                    EventController.Instance.PostEvent("StartRocket", null);
                    break;
            }
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
            && MissionController.Instance.CurrentState is DestroyTargetState)
        {
            renderer.enabled = true;
            collider.enabled = true;
        }

        if ((Radar.Instance.DistanceToTarget > this.DistanceToAtack && this.renderer.enabled) 
            || !(MissionController.Instance.CurrentState is DestroyTargetState))
        {
            if (this.renderer.enabled)
            {
                renderer.enabled = false;
                collider.enabled = false;
            }
        }
    }
}
