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

    public static readonly Dictionary<int, int> mLevelToEnemiesCount = new Dictionary<int, int>
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
        {18, 8},
        {19, 8},
        {20, 8},
    };

    public Transform WaypointsParent;
    public Transform SpawnsParent;

    private SpawnPoint[] mSpawnPoints;
    private WayPoint[] mWayPoints;
    private int mCurrentLevel;


    void Start ()
    {
        mWayPoints = WaypointsParent.GetComponentsInChildren<WayPoint>();

        var spawnsChildren = SpawnsParent.GetComponentsInChildren<Transform>();
        mSpawnPoints = spawnsChildren
            .Where(p => p != SpawnsParent)
            .Select(p => new SpawnPoint
            {
                IsFree = true,
                Transform = p,
                WayPoint = ClosestWayPoint(p.position)
            })
            .ToArray();

        SpawnTanksForLevel(TransportGOController.Instance.SelectedMissionID);
	}

    private WayPoint ClosestWayPoint(Vector3 position)
    {
        WayPoint min = null;
        float minDistance = 99999f;
        foreach (WayPoint wayPoint in mWayPoints)
        {
            float distance = (wayPoint.transform.position - position).magnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                min = wayPoint;
            }
        }

        return min;
    }

    private bool _baseEntered;
    private void Update()
    {
        if (!missionFinished && !MissionController.Instance.Failed)
        {
            bool all = true;
            for (int i = 0; i < CurrentTargetList.Count; i++)
            {
                GameObject p = CurrentTargetList[i];
                if (!p.GetComponent<MissionObject>().Destroyed)
                {
                    all = false;
                    break;
                }
            }
            if (all)
            {

                UnlockPlanesForCurrentLevel();

                EventController.Instance.PostEvent("MissionFinished", null);
                missionFinished = true;
            }
        }


        bool any = false;
        for (int i = 0; i < CurrentTargetList.Count; i++)
        {
            GameObject p = CurrentTargetList[i];
            if (!p.GetComponent<MissionObject>().Destroyed)
            {
                if (p.GetComponent<MoveTank>().EnteredBase)
                {
                    any = true;
                    break;
                }
            }
        }
        if (any)
        {
            if (!_baseEntered)
            {
                EventController.Instance.PostEvent("BaseEntered", null);
                _baseEntered = true;
            }
        }
        else
        {
            if (_baseEntered)
            {
                EventController.Instance.PostEvent("BaseExit", null);
                _baseEntered = false;
            }
        }
    }



    public void SpawnTanksForLevel(int level)
    {
        int enemiesCount = mLevelToEnemiesCount[level];
        mCurrentLevel = level;
        CurrentTargetList.Clear();
        for (int i = 0; i < enemiesCount; i++)
        {
            var freePoints = mSpawnPoints.Where(p => p.IsFree).ToList();
            var spawnPoint = freePoints[RandomTool.NextInt(0, freePoints.Count - 1)];

            var tank = GameObject.Instantiate(TankPrefab, spawnPoint.Transform.position, Quaternion.identity) as GameObject;
            MoveTank moveTank = tank.GetComponent<MoveTank>();
            moveTank.Target = spawnPoint.WayPoint;
            spawnPoint.IsFree = false;

            CurrentTargetList.Add(tank);
        }
    }

    private void UnlockPlanesForCurrentLevel()
    {
        switch (mCurrentLevel)
        {
            case 5:
                UnlockPlane(Airplanes.FA_22);
                break;
            case 11:
                UnlockPlane(Airplanes.FA_38);
                break;
        }
    }

    private void UnlockPlane(Airplanes id)
    {
        for (int i = 0; i < TransportGOController.Instance.PlanesInfo.Length; i++)
        {
            if (TransportGOController.Instance.PlanesInfo[i].ID == id)
            {
                TransportGOController.Instance.PlanesInfo[i].Locked = false;
                TransportGOController.Instance.PlanesInfo[i].Buyout = false;
            }
        }

        EventController.Instance.PostEvent("OnSaveData", null);
        //EventController.Instance.PostEvent("OnHideGUI", null);
        //EventController.Instance.PostEvent("OnShowAirplaneSelecting", null);
    }

    private class SpawnPoint
    {
        public Transform Transform;
        public WayPoint WayPoint;
        public bool IsFree = true;
    }
}
