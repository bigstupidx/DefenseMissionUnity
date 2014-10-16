using System.Collections.Generic;
using System.Linq;
using MilitaryDemo;
using UnityEngine;
using System.Collections;

public class EnemySpawnController : MonoBehaviour
{
    public static List<GameObject> CurrentTargetList = new List<GameObject>(); 

    public GameObject TargetBase;
    public GameObject TankPrefab;

    private bool missionFinished;

    private readonly Dictionary<int, int> mLevelToEnemiesCount = new Dictionary<int, int>
    {
        {0, 1},
        {1, 2},
        {2, 3},
        {3, 3},
        {4, 3},
        {5, 4},
        {6, 4},
        {7, 5},
        {8, 5},
        {9, 6},
        {10, 6},
        {11, 6},
        {12, 7},
        {13, 7},
        {14, 7},
        {15, 8},
        {16, 8},
        {17, 8},
        {18, 9},
        {19, 9},
        {20, 10},
    };

    private SpawnPoint[] mSpawnPoints;


	void Start ()
	{
	    var children = GetComponentsInChildren<Transform>();
        mSpawnPoints = children
            .Select(p => new SpawnPoint {IsFree = true, Transform = p})
            .ToArray();

        SpawnTanksForLevel(TransportGOController.Instance.SelectedMissionID);
	}

    private void Update()
    {
        if (!missionFinished)
        {
            if (CurrentTargetList.All(p => p.GetComponent<MissionObject>().Destroyed))
            {
                EventController.Instance.PostEvent("MissionFinished", null);
                missionFinished = true;
            }
        }
    }

    public void SpawnTanksForLevel(int level)
    {
        int enemiesCount = mLevelToEnemiesCount[level];
        CurrentTargetList.Clear();
        for (int i = 0; i < enemiesCount; i++)
        {
            var freePoints = mSpawnPoints.Where(p => p.IsFree).ToList();
            var spawnPoint = freePoints[RandomTool.NextInt(0, freePoints.Count - 1)];

            var tank = GameObject.Instantiate(TankPrefab, spawnPoint.Transform.position, Quaternion.identity) as GameObject;
            MoveTank moveTank = tank.GetComponent<MoveTank>();
            moveTank.Target = TargetBase;

            CurrentTargetList.Add(tank);
        }
    }


    private class SpawnPoint
    {
        public Transform Transform;
        public bool IsFree = true;
    }
}
