using UnityEngine;
using System.Collections;

public class MissionStateText : MonoBehaviour, IEventSubscriber
{
    public TextMesh MissionText;
    public TextMesh DistText;

    void Update()
    {
        DistText.text = "Distance: " + ((int)Radar.Instance.DistanceToTarget).ToString() + " m";    
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        MissionText.text = "Current: ";
        switch (EventName)
        {
            case "MissionChangeTarget": 
                if (!MissionController.Instance.Finished)
                    MissionText.text += MissionController.Instance.CurrentState.MissionStateText;
                break;
            case "MissionStarted":
                if (!MissionController.Instance.Finished)
                    MissionText.text += MissionController.Instance.CurrentState.MissionStateText;
                break;
            case "MissionFinished":
                //MissionText.text += "Mission succesfuly finished!";
                break;
            case "MissionFailed":
                MissionText.text += "Mission failed...";
                break;
        }
    }

    void Awake()
    {
        EventController.Instance.Subscribe("MissionChangeTarget", this);
        EventController.Instance.Subscribe("MissionFinished", this);
        EventController.Instance.Subscribe("MissionStarted", this);
        EventController.Instance.Subscribe("MissionFailed", this);
    }

    #endregion
}
