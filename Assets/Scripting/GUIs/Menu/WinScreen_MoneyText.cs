using UnityEngine;
using System.Collections;

public class WinScreen_MoneyText : MonoBehaviour, IEventSubscriber
{
    void Start()
    {
        //gameObject.GetComponent<TextMesh>().text = TransportGOController.Instance.Missions [TransportGOController.Instance.SelectedMissionID].Payment.ToString();
        EventController.Instance.Subscribe("MissionFinished", this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "MissionFinished")
        {
            OptionsController.Instance.PlayerMoney += TransportGOController.Instance.Missions [TransportGOController.Instance.SelectedMissionID].Payment;
            EventController.Instance.PostEvent("OnSaveData", null);
        }
    }

    #endregion
}
