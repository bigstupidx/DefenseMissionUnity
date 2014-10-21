using UnityEngine;
using System.Collections;

public class BriefScreen : MonoBehaviour, IEventSubscriber
{

    public TextMesh TextMesh;

	// Use this for initialization
	void Start ()
	{
        int enemiesCount = EnemySpawnController.mLevelToEnemiesCount[TransportGOController.Instance.SelectedMissionID];
	    TextMesh.text = "There will be " + enemiesCount +  (enemiesCount == 1? " enemy" : " enemies");

	    Time.timeScale = 0f;
        EventController.Instance.Subscribe("OnBriefHide", this);
	}

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "OnBriefHide")
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }
    }
}
