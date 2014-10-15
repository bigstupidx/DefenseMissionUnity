using UnityEngine;
using System.Collections;

public class PlaneSelecting_MoneyText : MonoBehaviour, IEventSubscriber
{
    void Awake()
    {
        gameObject.GetComponent<TextMesh>().text = 
            OptionsController.Instance.PlayerMoney.ToString();

        EventController.Instance.Subscribe("OnUpdateOptions", this);
        EventController.Instance.Subscribe("OnShowAirplaneSelecting", this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "OnUpdateOptions" || EventName == "OnShowAirplaneSelecting")
            gameObject.GetComponent<TextMesh>().text = 
                OptionsController.Instance.PlayerMoney.ToString();
    }

    #endregion
}
