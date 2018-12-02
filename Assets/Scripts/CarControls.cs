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

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RespawnScript))]
public class CarControls : MonoBehaviour
{
    public List<AxleInfo> axleInfos;

    [SerializeField] private float _maxMotorTorque;

    [SerializeField] private float _maxBreakTorque;

    [SerializeField] private float _maxSteeringAngle;

    [SerializeField] private float _maxSpeed;

    private Rigidbody _carRigidbody;

    [SerializeField] private PositionHandlerScript _positionHandler;

    private List<PlayerPosition> _playerPositions;

    //amount of players doing what - audio related
    [SerializeField]private int[] _playerActions = new int[4]; //-1 - brake //0 - neutral  //1-accelerate
    public int AccelerationLevel;

    float _steerLeft = 0, _steerRight = 0, _brake = 0, _gas = 0;

    private float _respawnTimer=0;

    [SerializeField] private float _respawnTime;

    private RespawnScript _respawnScript;

    public void Start()
    {
        _playerPositions = _positionHandler.PlayerPositions;
        _carRigidbody = GetComponent<Rigidbody>();
        _respawnScript = GetComponent<RespawnScript>();

    }

    public void Update()
    {
        _steerLeft = 0; _steerRight = 0; _brake = 0; _gas = 0;

        for (int i = 0; i < PlayerPrefs.GetInt("AmountOfPlayers", 0); i++)
        {
            switch (_playerPositions[i]._position)
            {
                case Positions.SteerLeft:
                    _steerLeft += _maxSteeringAngle * Input.GetAxis("A" + (i + 1) + "_Axis") / 2;
                    //set playeractions to neutral - audio related
                    _playerActions[i] = 0;
                    break;
                case Positions.SteerRight:
                    _steerRight += _maxSteeringAngle * Input.GetAxis("A" + (i + 1) + "_Axis") / 2;
                    //set playeractions to neutral - audio related
                    _playerActions[i] = 0;
                    break;
                case Positions.Brake:
                    _brake += _maxMotorTorque * Input.GetAxis("A" + (i + 1) + "_Axis") / 2;
                    //adjust playeractions accordingly - audio related
                    if(Input.GetAxis("A" + (i + 1) + "_Axis") != 0)
                    {
                        _playerActions[i] = -1;
                    }
                    else
                    {
                        _playerActions[i] = 0;
                    }
                    break;
                case Positions.Gas:
                    _gas += _maxMotorTorque * Input.GetAxis("A" + (i + 1) + "_Axis") / 2;
                    //adjust playeractions accordingly - audio related
                    if (Input.GetAxis("A" + (i + 1) + "_Axis") != 0)
                    {
                        _playerActions[i] = 1;
                    }
                    else
                    {
                        _playerActions[i] = 0;
                    }
                    break;
            }
        }
        //calculate acceleration level - audio related
        AccelerationLevel = _playerActions[0] + _playerActions[1] + _playerActions[2] + _playerActions[3];


        //if (Application.isEditor)
        //{
        //    _steerRight = _maxSteeringAngle * Input.GetAxis("Horizontal");
        //    _gas = _maxMotorTorque * Input.GetAxis("Vertical");
        //}

        if (CheckIfAllPlayersAreHoldingB())
        {
            _respawnTimer += Time.deltaTime;

            if (_respawnTimer > _respawnTime)
            {
                _respawnScript.Respawn();
                _respawnTimer = 0;
            }
        }
        else _respawnTimer = 0;
    }

    public void FixedUpdate()
    {
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

        //limit max speed
        if (_carRigidbody.velocity.magnitude > _maxSpeed)
        {
            _carRigidbody.velocity = _carRigidbody.velocity.normalized * _maxSpeed;
        }
        //Debug.Log(_carRigidbody.velocity);
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

    private bool CheckIfAllPlayersAreHoldingB()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("AmountOfPlayers", 0); i++)
        {
            if (!Input.GetButton("B" + (i + 1) + "_XboxButton"))
                return false;
        }
        return true;
    }
}