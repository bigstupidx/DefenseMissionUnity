using System.Linq;
using UnityEngine;
using System.Collections;

public class MissionStateText : MonoBehaviour, IEventSubscriber
{
    public FriendlyBase FriendlyBase;

    public TextMesh MissionText;
    public TextMesh DistText;
    public TextMesh HealthText;
    public TextMesh EnemiesLeftText;
    private int oldCount = -1;
    private int oldHealth = 0;
    void Update()
    {
        DistText.text = "Distance: " + (int)Radar.Instance.DistanceToTarget + "m";

        if (oldHealth != (int) FriendlyBase.CurrentHealth)
        {
            oldHealth = (int)FriendlyBase.CurrentHealth;
            HealthText.text = "Base status: " + oldHealth + "%";
        }

        var count = GetDestroyedEnemiesCount();

        if (oldCount != count)
        {
            oldCount = count;
            EnemiesLeftText.text = "Enemies left: " + count;
        }
    }

    private static int GetDestroyedEnemiesCount()
    {
        int count = 0;
        for (int i = 0; i < EnemySpawnController.CurrentTargetList.Count; i++)
        {
            GameObject p = EnemySpawnController.CurrentTargetList[i];
            if (!p.GetComponent<MissionObject>().Destroyed) count++;
        }
        return count;
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
