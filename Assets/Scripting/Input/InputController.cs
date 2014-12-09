using System;
using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour, IEventSubscriber
{
	public AirplaneController Plane;

    public Rect VertPosition;

    public Rect NavigateTexturePosition;
    public Texture NavigateButtonTexture;

    public Rect ChassiTexturePosition;
    public Texture ChassiButtonTexture;

    private Vector2 _navigate;
    private Vector2 _relNav = Vector2.zero;
    private bool _navigatePress=false;
    private int _navID;

    private GestureController _g;

    void Awake()
    {
        _g = gameObject.AddComponent<GestureController>();
        _g.OnGestureStart += OnGestureStart;
        _g.OnGestureEnd += OnGestureEnd;
        Input.multiTouchEnabled = true;
    }

    void Start()
    {
        Plane = AirplaneController.Instance;
        Input.gyro.enabled = true;
        EventController.Instance.SubscribeToAllEvents(this);
    }

    private Transform _lever;
    private float _leverLevel = -0.45f;
    private int _leverID = -1;

    private Transform _navButton;
    private Vector3 _navFirstPosition;

    void OnGestureStart(Gesture g)
    {
        Ray ray = GUICameraController.Instance.camera.ScreenPointToRay(g.StartPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            switch(hit.collider.gameObject.name)
            {
                case "Lever":
                    _lever = hit.collider.gameObject.transform;
                    _leverID = g.ID;
                    g.OnAddingTouch += OnGestureStay;
                    break;
                case "Button_Chassi":
                    Plane.ChassisEnable = !Plane.ChassisEnable;
                    break;
                case "Shoot":
                    EventController.Instance.PostEvent("AttackButtonPressed",gameObject);
                    break;
                case "ChangeView":
                    Debug.Log("Change");
                    CameraController.Instance.ChangeCamera();
                    break;
                case "NavigationButton":
                    _navigatePress = true;
                    _navID = g.ID;
                    g.OnAddingTouch += OnGestureStay;
                    _navigate = g.StartPoint;
                    _relNav = Vector2.zero;
                    _navButton = hit.collider.gameObject.transform;
                    _navFirstPosition = _navButton.localPosition;
                    break;
                default:
                    EventController.Instance.PostEvent("OnPressObject", hit.collider.gameObject);
                    break;
            }
        }
        else
            EventController.Instance.PostEvent("OnPressObject",null);
    }

    void OnGestureStay(Gesture g)
    {
        if (g.ID == _leverID)
        {
            Ray ray = GUICameraController.Instance.camera.ScreenPointToRay(g.EndPoint);
            Vector3 pos = _lever.localPosition;
            _leverLevel = Mathf.Clamp(_lever.parent.InverseTransformPoint(ray.origin).y,-0.45f,0.4f);
            pos.y = _leverLevel;
            _lever.localPosition = pos;
        } else
        {
            _relNav = g.EndPoint - _navigate;
            if (OptionsController.Instance.InvertAxisY)
                _relNav.y *= -1;

            Ray ray = GUICameraController.Instance.camera.ScreenPointToRay(g.EndPoint);
            Vector3 pos = _navButton.parent.InverseTransformPoint(ray.origin);
            pos.z = 5;
            pos = _navFirstPosition - pos;

            if (pos.x < 0.5f && pos.x > -0.5f)
                _relNav.x = 0;

            if (pos.magnitude < 1)
                _navButton.localPosition = _navFirstPosition - pos;
            else
                _navButton.localPosition = _navFirstPosition - pos.normalized;
        }
    }

    void OnGestureEnd(Gesture g)
    {
        if (g.ID == _leverID)
        {
            _leverID = -1;
            //_lever.localPosition = new Vector3(0.1f,0,-1);
        } else
        if (g.ID == _navID && _navigatePress)
        {
            _navigatePress = false;
            _relNav = Vector2.zero;
            _navButton.localPosition = _navFirstPosition;
        }

        Ray ray = GUICameraController.Instance.camera.ScreenPointToRay(g.EndPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            EventController.Instance.PostEvent("OnReleaseObject",hit.collider.gameObject);
        else
            EventController.Instance.PostEvent("OnReleaseObject",null);
    }

    void Update()
    {
        UpdatePlaneRotation();

        StartCoroutine(ChangeSpeed(_leverLevel + 0.45f));
    }

    private float rotX = 0;
    private float rotY = 0;

    private void UpdatePlaneRotation()
    {
        const float staticAngle = 295;
        const float maxOffset = 25f;

        Vector2 rot = _relNav/50;
        var deviceRotation = Input.gyro.attitude.eulerAngles;

        if (Application.isEditor && DeviceEmu.Instance.Gyroscope)
        {
            deviceRotation = DeviceEmu.Instance.transform.rotation.eulerAngles;
        }

        if (Application.isEditor && !DeviceEmu.Instance.Gyroscope)
        {
            UpdateEditorRotation();
        }
        else
        {
            var x = GetXRotation(deviceRotation);
            var y = GetYRotation(deviceRotation);


            Debug.Log(" X " + x + " Y " + y);
            Plane.Rotation = new Vector2(Mathf.Clamp(x*2.5f, -1, 1), Mathf.Clamp(-y*2.5f, -1, 1));
        }
    }

    private static float GetXRotation(Vector3 deviceRotation)
    {
        float x = 0;
        const float deadZoneYrom = 5;
        const float deadZoneYTo = 355;

        const float minYRotationFrom = 5;
        const float minYRotationTo = 45;

        const float maxYRotationFrom = 320;
        const float maxYRotationTo = 355;

        if (deviceRotation.y <= deadZoneYrom || deviceRotation.y >= deadZoneYTo)
        {
            x = 0;
        }
        else if (deviceRotation.y > minYRotationFrom && deviceRotation.y < minYRotationTo)
        {
            x = -((deviceRotation.y - minYRotationFrom) / (minYRotationTo - minYRotationFrom));
        }
        else if (deviceRotation.y > maxYRotationFrom && deviceRotation.y < maxYRotationTo)
        {
            x = 1f - (deviceRotation.y - maxYRotationFrom) / (maxYRotationTo - maxYRotationFrom);
        }
        return x;
    }

    private static float GetYRotation(Vector3 deviceRotation)
    {
        float y = 0;
        const float deadZoneXFrom = 334;
        const float deadZoneXTo = 336;

        const float minXRotationFrom = 311;
        const float minXRotationTo = 334;

        const float maxXRotationFrom = 336;
        const float maxXRotationTo = 359;

        if (deviceRotation.x >= deadZoneXFrom && deviceRotation.x <= deadZoneXTo)
        {
            y = 0;
        }
        else if (deviceRotation.x > minXRotationFrom && deviceRotation.x < minXRotationTo)
        {
            y = 1f - ((deviceRotation.x - minXRotationFrom)/(minXRotationTo - minXRotationFrom));
        }
        else if (deviceRotation.x > maxXRotationFrom && deviceRotation.x < maxXRotationTo)
        {
            y = -(deviceRotation.x - maxXRotationFrom)/(maxXRotationTo - maxXRotationFrom);
        }
        return y;
    }



    private void UpdateEditorRotation()
    {
        float x = Input.GetKey(KeyCode.RightArrow)
            ? 1f
            : Input.GetKey(KeyCode.LeftArrow)
                ? -1f
                : 0f;
        float y = Input.GetKey(KeyCode.UpArrow)
            ? 1f
            : Input.GetKey(KeyCode.DownArrow)
                ? -1f
                : 0f;

        Plane.Rotation = new Vector2(Mathf.Clamp(x, -1, 1), Mathf.Clamp(y, -1, 1));
    }

    IEnumerator ChangeSpeed(float NewSpeed)
    {
        yield return new WaitForSeconds(0.3f);
        Plane.Speed = NewSpeed;
    }

    public Rect PercentToScreen(Rect Percent)
    {
        return new Rect(Percent.xMin * Screen.width,
                        Percent.yMin * Screen.height,
                        Percent.width * Screen.width,
                        Percent.height * Screen.height);
    }

    private Rect SwapCoords(Rect tex)
    {
        return new Rect(tex.xMin,
                        Screen.height - (tex.yMin + tex.height),
                        tex.width, tex.height);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "MissionFinished")
            _relNav.y = -50;
    }

    #endregion
}
