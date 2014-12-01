using UnityEngine;
using System.Collections;

public enum MenuState
{
    Main,
    Options,
    AirplaneSelect,
    MissionSelect,
    HelpScreen
}

public class MenuController : MonoBehaviour, IEventSubscriber
{
    private static MenuController s_Instance = null;
    public GameObject TGO;
    
    public static MenuController Instance 
    {
        get 
        {
            if (s_Instance == null)
                s_Instance =  FindObjectOfType(typeof (MenuController)) as MenuController;
            
            if (s_Instance == null) 
            {
                GameObject obj = new GameObject("AManager");
                s_Instance = obj.AddComponent(typeof (MenuController)) as MenuController;
                Debug.Log ("Could not locate an AManager object.  AManager was Generated Automaticly.");
            }
            
            return s_Instance;
        }
    }
    
    void OnApplicationQuit() 
    {
        s_Instance = null;
    }

    public Camera Cam2D;

    void Awake()
    {
        GestureController.Instance.OnGestureStart = OnGestureStart;
        GestureController.Instance.OnGestureEnd = OnGestureEnd;

        if (FindObjectOfType<DataStorageController>() == null)
            GameObject.Instantiate(TGO);
    }

    void Start()
    {
        EventController.Instance.SubscribeToAllEvents(this);

        EventController.Instance.PostEvent("OnHideGUI", null);
        EventController.Instance.PostEvent("OnLoadedMenuScene", null);
        EventController.Instance.PostEvent("OnShowMainMenu", null);
        OptionsController.Instance.SelfSubscribe();
    }

    private GameObject _activeButton;
    private Texture _firstTex;

    public Gesture LastStartedGesture { get; private set; }

    void OnGestureStart(Gesture g)
    {
        LastStartedGesture = g;
        Ray ray = Cam2D.ScreenPointToRay(g.StartPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            EventController.Instance.PostEvent("OnPressObject", hit.collider.gameObject);
            g.OnAddingTouch+=OnGestureMoved;
        }
        else
            EventController.Instance.PostEvent("OnPressObject",null);
    }

    void OnGestureMoved(Gesture g)
    {
        if (Vector2.Distance(g.StartPoint,g.EndPoint)>5)
        {
            EventController.Instance.PostEvent("OnTouchMoved", null);
        }
    }

    void OnGestureEnd(Gesture g)
    {
        Ray ray = Cam2D.ScreenPointToRay(g.EndPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            EventController.Instance.PostEvent("OnReleaseObject",hit.collider.gameObject);
        else
            EventController.Instance.PostEvent("OnReleaseObject",null);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            EventController.Instance.PostEvent("OnGoBack",null);
    }

    private MenuState _menuState;
    public MenuState MenuMode { get { return _menuState; } }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "OnShowGameMode":
                AdMobAndroid.destroyBanner();
                Application.LoadLevel("main");
                break;

            case "OnRestoreDefaults":
                OptionsController.Instance.MusicLevel = 1;
                OptionsController.Instance.SFXLevel = 1;
                OptionsController.Instance.InvertAxisY = false;
                EventController.Instance.PostEvent("OnUpdateGUI",null);
                EventController.Instance.PostEvent("OnUpdateOptions",null);
                break;

            case "OnShowMainMenu":
                _menuState = MenuState.Main;
                if (GoogleAnalytics.instance)
                    GoogleAnalytics.instance.LogScreen("Main Menu");
                break;

            case "OnShowAirplaneSelecting":
                _menuState = MenuState.AirplaneSelect;
                if (GoogleAnalytics.instance)
                    GoogleAnalytics.instance.LogScreen("Airplane Scelecting");
                break;

            case "OnShowMissionSelecting":
                _menuState = MenuState.MissionSelect;
                if (GoogleAnalytics.instance)
                    GoogleAnalytics.instance.LogScreen("Mission Selecting");
                break;

            case "OnShowOptions":
                _menuState = MenuState.Options;
                if (GoogleAnalytics.instance)
                    GoogleAnalytics.instance.LogScreen("Setting menu");
                break;

            case "OnShowHelp":
                _menuState = MenuState.HelpScreen;
                if (GoogleAnalytics.instance)
                    GoogleAnalytics.instance.LogScreen("Help Screen");
                break;

            case "OnGoBack":
                if (_menuState != MenuState.Main)
                    EventController.Instance.PostEvent("OnHideGUI",null);
                switch (_menuState)
                {
                    case MenuState.AirplaneSelect:
                        EventController.Instance.PostEvent("OnShowMainMenu",null);
                        break;

                    case MenuState.MissionSelect:
                        EventController.Instance.PostEvent("OnShowAirplaneSelecting",null);
                        break;

                    case MenuState.HelpScreen:
                        EventController.Instance.PostEvent("OnShowMainMenu",null);
                        break;

                    case MenuState.Options:
                        EventController.Instance.PostEvent("OnSaveData",null);
                        EventController.Instance.PostEvent("OnShowMainMenu",null);
                        break;

                    case MenuState.Main:
                        //Application.Quit();
                        break;
                }
                break;

            case "OnResetProgress":
                for (int i=1;i<TransportGOController.Instance.Missions.Length;i++)
                    TransportGOController.Instance.Missions[i].Blocked = true;
                if (GoogleAnalytics.instance)
                    GoogleAnalytics.instance.LogScreen("Settings - Reset progress");
                break;

            case "OnResetPurchases":
                var skus = new string[] { "airplane_f22", "airplane_fa38" };
                GoogleIAB.queryInventory( skus );
                if (GoogleAnalytics.instance)
                    GoogleAnalytics.instance.LogScreen("Settings - Restore purchases");
                /*
                AirplaneInfo p = TransportGOController.GetPlaneInfo(Airplanes.Mirage);
                p.Locked = true;
                p.Buyout = false;
                p = TransportGOController.GetPlaneInfo(Airplanes.F_16);
                p.Locked = true;
                p.Buyout = true;
                p = TransportGOController.GetPlaneInfo(Airplanes.FA_22);
                p.Locked = true;
                p.Buyout = true;
                p = TransportGOController.GetPlaneInfo(Airplanes.FA_38);
                p.Locked = true;
                p.Buyout = true;
                p = TransportGOController.GetPlaneInfo(Airplanes.SAAB);
                p.Locked = false;
                p.Buyout = false;
                */
                break;

            case "OnShowMoreGames":
                Application.OpenURL("https://play.google.com/store/apps/developer?id=i6+Games");
                if (GoogleAnalytics.instance)
                    GoogleAnalytics.instance.LogScreen("More Games");
                break;

            case "OnDebugAddGold":
                OptionsController.Instance.PlayerMoney += 1000;
                EventController.Instance.PostEvent("OnUpdateOptions", null);
                EventController.Instance.PostEvent("OnSaveData", null);
                break;

            case "OnLoadedMenuScene":
                AdMobAndroid.destroyBanner();
                break;
        }
    }

    #endregion
}
