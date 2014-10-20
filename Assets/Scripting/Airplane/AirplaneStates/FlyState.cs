using System;
using DN;
using UnityEngine;

public class FlyState : IAirplaneState
{
    AirplaneController _plane;
    private Timer mUndeadTimer;
    public FlyState(AirplaneController Controller)
    {
        _plane = Controller;
        mUndeadTimer = new Timer();
        mUndeadTimer.Duration = 0.25f;
        mUndeadTimer.OnTick += OnTick;
        mUndeadTimer.Run();
    }

    private void OnTick(object sender, EventArgs eventArgs)
    {
        if(_collided)
        {
            _plane.State = AirplaneStates.Die;
        }

    }

    #region IAirplaneState implementation

    private bool _collided = false;
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Death"))
        {
            _collided = true;
            if (!mUndeadTimer.IsRunning)
            {
                _plane.State = AirplaneStates.Die;
            }
        }
        if (col.gameObject.CompareTag("MissionObject"))
        {
            _plane.State = AirplaneStates.Die;
        }
        if (col.gameObject.CompareTag("Runway"))
        {
            if (Vector3.Angle(Vector3.up,_plane.transform.up) < 45 && _plane.CurrentSpeed < _plane.MaxSpeed / 2 &&
                _plane.ChassisEnable)
            {
                Vector3 target = _plane.transform.position + _plane.transform.forward;
                target.y = _plane.transform.position.y;
                _plane.transform.LookAt(target);
                EventController.Instance.PostEvent("Landing", col.gameObject);
                _plane.CurrentRotation = _plane.TargetRotation;
                _plane.State = AirplaneStates.Ride;
            }
            else
            {
                _plane.State = AirplaneStates.Die;
            }
        }
    }

    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Death"))
        {
            _collided = false;
        }
    }

    public void Awake()
    {

    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        // LIFT
        Vector3 forward = _plane.transform.forward;
        if (forward.y>0)
            forward.y *= Mathf.Clamp(_plane.CurrentSpeed - _plane.MinFlySpead,0,100)/100.0f;
        if (_plane.transform.position.y>19900)
            forward.y *= Mathf.Clamp(100-(2000-_plane.transform.position.y),0,100)/100.0f;
        float rx = _plane.transform.rotation.eulerAngles.x;
//        if (rx > 180 && rx < 345)
//            _plane.CurrentSpeed += (rx - 345)*Time.fixedDeltaTime*5;
//        if (rx > 15 && rx < 180)
//            _plane.CurrentSpeed += (rx - 15)*Time.fixedDeltaTime*5;
        _plane.CurrentSpeed = Mathf.Clamp(_plane.CurrentSpeed, _plane.MinFlySpead, _plane.MaxSpeed*3);
        _plane.rigidbody.position += forward * _plane.CurrentSpeed * Time.fixedDeltaTime;

        // HORIZ
        if (Mathf.Abs(_plane.TargetRotation.x - _plane.CurrentRotation.x) > 0.5f)
        {

            float speed = Mathf.Abs(_plane.TargetRotation.x) < 0.1f ? _plane.BreakRotation.x :
                _plane.AccelRotation.x;
//            if (_plane.TargetRotation.x < _plane.CurrentRotation.x)
//                _plane.CurrentRotation.x -= speed * Time.fixedDeltaTime;
//            else
//                _plane.CurrentRotation.x += speed * Time.fixedDeltaTime;

            _plane.CurrentRotation.x = Mathf.Lerp(_plane.CurrentRotation.x, _plane.TargetRotation.x, Time.fixedDeltaTime * speed);

        } else
            _plane.CurrentRotation.x = _plane.TargetRotation.x;

        // VERT
        if (Mathf.Abs(_plane.TargetRotation.y - _plane.CurrentRotation.y) > 0.5f)
        {
            _plane.CurrentRotation.y = Mathf.Lerp(_plane.CurrentRotation.y, _plane.TargetRotation.y,
                Time.fixedDeltaTime*_plane.AccelRotation.y);

            //  Mathf.Sign(_plane.TargetRotation.y - _plane.CurrentRotation.y) * Time.fixedDeltaTime *_plane.AccelRotation.y;
        } else
            _plane.CurrentRotation.y = _plane.TargetRotation.y;

        Vector3 rot = _plane.transform.rotation.eulerAngles;
        rot.x = _plane.CurrentRotation.y;
        _plane.transform.rotation = Quaternion.Euler(rot);

        // POSITION / ROTATION
        if (Mathf.Abs(_plane.CurrentRotation.x) > 1f)
        {
            _plane.transform.Rotate(Vector3.up, _plane.CurrentRotation.x * Time.fixedDeltaTime, Space.World);
            rot = _plane.transform.rotation.eulerAngles;
            rot.z = (-_plane.CurrentRotation.x / _plane.MaxRotation.x) * 30.0f;
            _plane.transform.rotation = Quaternion.Euler(rot);
        } else
        {
            rot = _plane.transform.rotation.eulerAngles;
            rot.z = 0;
            _plane.transform.rotation = Quaternion.Euler(rot);
        }
        
        float target = Mathf.Max(_plane.TargetSpeed,_plane.MinFlySpead);
        if (Mathf.Abs(target - _plane.CurrentSpeed) > 1f)
        {
            if (_plane.CurrentSpeed - target < 1)
                _plane.CurrentSpeed += _plane.Acceleration * Time.fixedDeltaTime;
            else if (_plane.CurrentSpeed - target > 1)
                _plane.CurrentSpeed -= _plane.Breaking * Time.fixedDeltaTime * (_plane.ChassisEnable ? 2:1);
        }

        mUndeadTimer.Update(Time.fixedDeltaTime);


        // SHOWCASE
        _plane.Driver.Yaw = _plane.TargetRotation.x / _plane.MaxRotation.x;
        _plane.Driver.Pitch = _plane.TargetRotation.y / _plane.MaxRotation.y;
        _plane.Driver.Roll = _plane.TargetRotation.x / _plane.MaxRotation.x;
        _plane.Driver.OnDataChanged();
    }

    public void OnActivate()
    {
        //_plane.rigidbody.isKinematic = true;
        _plane.rigidbody.useGravity = false;
    }

    public void OnDeactivate()
    {

    }

    #endregion
}