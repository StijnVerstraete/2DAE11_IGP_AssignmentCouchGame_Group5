using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool isMotor;
    public bool isBreak;
    public bool isSteering;
}


public class cartestscript : MonoBehaviour
{
    public List<AxleInfo> axleInfos;

    [SerializeField] private float _maxMotorTorque;

    [SerializeField] private float _maxBreakTorque;

    [SerializeField] private float _maxSteeringAngle;

    [SerializeField] private float _maxSpeed;

    [SerializeField] private Rigidbody _carRigidbody;

    [SerializeField] private PositionHandlerScript _positionHandler;

    private List<PlayerPosition> _playerPositions;

    public void Start()
    {
        _playerPositions = _positionHandler.PlayerPositions;
    }

    public void FixedUpdate()
    {
        float _steerLeft=0, _steerRight=0, _brake=0, _gas=0;

        for (int i = 0; i < _playerPositions.Count; i++)
        {
            if (_playerPositions[i]._transform != null)
            {
                switch (_playerPositions[i]._position)
                {
                    case Positions.SteerLeft: _steerLeft += _maxSteeringAngle * Input.GetAxis("A" + (i+1) + "_Axis")/2; break;
                    case Positions.SteerRight: _steerRight += _maxSteeringAngle * Input.GetAxis("A" + (i + 1) + "_Axis") / 2; break;
                    case Positions.Brake: _brake += _maxMotorTorque * Input.GetAxis("A" + (i + 1) + "_Axis") / 2; break;
                    case Positions.Gas: _gas += _maxMotorTorque * Input.GetAxis("A" + (i + 1) + "_Axis") / 2; break;
                }
            }
        }

        if (Application.isEditor)
        {
            _steerRight = _maxSteeringAngle * Input.GetAxis("Horizontal");
            _gas = _maxMotorTorque * Input.GetAxis("Vertical");
        }

        //for both the front and back axle
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.isSteering)
            {
                axleInfo.leftWheel.steerAngle = _steerRight - _steerLeft;
                axleInfo.rightWheel.steerAngle = _steerRight - _steerLeft;
            }
            if (axleInfo.isMotor)
            {
                axleInfo.leftWheel.motorTorque = _gas - _brake;
                axleInfo.rightWheel.motorTorque = _gas - _brake;
            }
            //if (axleInfo.isBreak)
            //{
            //    axleInfo.leftWheel.brakeTorque = _break;
            //    axleInfo.rightWheel.brakeTorque = _break;
            //}
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        if (_carRigidbody.velocity.magnitude > _maxSpeed)
        {
            _carRigidbody.velocity = _carRigidbody.velocity.normalized * _maxSpeed;
        }
        Debug.Log(_carRigidbody.velocity);
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

}