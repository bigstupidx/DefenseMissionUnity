using System.Collections.Generic;
using System.Linq;
using MilitaryDemo;
using UnityEngine;
using System.Collections;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject TargetBase;

    public GameObject TankPrefab;


    private readonly Dictionary<int, int> mLevelToEnemiesCount = new Dictionary<int, int>
    {
        {0, 1},
        {1, 2},
        {2, 3},
    };

    private SpawnPoint[] mSpawnPoints;


    // Use this for initialization
	void Start ()
	{
	    var children = GetComponentsInChildren<Transform>();
        mSpawnPoints = children
            .Select(p => new SpawnPoint {IsFree = true, Transform = p})
            .ToArray();

        Debug.Log(TransportGOController.Instance.SelectedMissionID);
        SpawnTanksForLevel(TransportGOController.Instance.SelectedMissionID);
	}

    public void SpawnTanksForLevel(int level)
    {
        int enemiesCount = mLevelToEnemiesCount[level];

        for (int i = 0; i < enemiesCount; i++)
        {
            var freePoints = mSpawnPoints.Where(p => p.IsFree).ToList();
            var spawnPoint = freePoints[RandomTool.NextInt(0, freePoints.Count - 1)];

            var tank = GameObject.Instantiate(TankPrefab, spawnPoint.Transform.position, Quaternion.identity) as GameObject;
            MoveTank moveTank = tank.GetComponent<MoveTank>();
            moveTank.Target = TargetBase;
        }


    }

    // Update is called once per frame
	void Update () {
	
	}


    private class SpawnPoint
    {
        public Transform Transform;
        public bool IsFree = true;
    }
}
