using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool isMotor;
    public bool isSteering;
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RespawnScript))]
public class CarControls : MonoBehaviour
{
    public List<AxleInfo> axleInfos;

    [SerializeField] private float _maxMotorTorque;

    [SerializeField] private float _maxSteeringAngle;

    [SerializeField] private float _maxSpeedDefault;

    [SerializeField] private float _midAirRotationSpeed;

    private float _maxSpeed;

    private Rigidbody _carRigidbody;

    private PositionHandlerScript _positionHandler;

    private List<PlayerPosition> _playerPositions;
    private List<DefaultPosition> _defaultPositions;

    //amount of players doing what - audio related
    public int[] PlayerActionsAcceleration = new int[4]; //-1 - brake //0 - neutral  //1-accelerate
    public int[] PlayerActionsSteering = new int[4]; //-1 steer left //0 - neutral //1-steerright
    public int AccelerationLevel;
    public int SteeringLevel;
    [SerializeField]
    float _steerLeft = 0, _steerRight = 0, _brake = 0, _gas = 0;
    private bool _anywheelsGrounded;

    private float _respawnTimer=0;

    [SerializeField] private float _respawnTime;
    [SerializeField] private GameObject _respawnPanel;
    [SerializeField] private Slider _respawnBar;

    private RespawnScript _respawnScript;
    [SerializeField]
    private GameObject Countdown;
    private CountDown _countDownScript;

    private List<int> _controllers;

    private Transform _canvas;
    private PauseMenuScript _pauseMenu;
    void Start()
    {
        _positionHandler = GetComponent<PositionHandlerScript>();
        _controllers = _positionHandler.Controllers;
        _playerPositions = _positionHandler.PlayerPositions;
        _defaultPositions = _positionHandler.DefaultPositions;
        _carRigidbody = GetComponent<Rigidbody>();
        _respawnScript = GetComponent<RespawnScript>();

        _countDownScript = Countdown.GetComponent<CountDown>();

        _maxSpeed = _maxSpeedDefault;

        _canvas = GameObject.Find("Canvas").transform;
        _pauseMenu = _canvas.GetComponent<PauseMenuScript>();
    }

    public void Update()
    {
        if (_pauseMenu.IsPaused) return;

        _anywheelsGrounded = CheckIfAnyWheelsAreGrounded();
        _steerLeft = 0; _steerRight = 0; _brake = 0; _gas = 0;
        
        if (_countDownScript.ShowCountDown) // if countdown is still bussy you cannot use the car
            return;

        for (int i = 0; i < _controllers.Count; i++)
        {
            //STEERING + GAS & BREAK

            float axisInput = Input.GetAxis("A" + _controllers[i] + "_Axis");
            //Debug.Log("controller " + i + ": " + _controllers[i]);
            GlowCharacterIfClickOnButtonA(i); // if you click on button the character is glowing

            switch (_playerPositions[i]._position)
            {
                case Positions.SteerLeft:
                    {
                        _steerLeft += _maxSteeringAngle * axisInput / 2;
                        //set playeractions to neutral - audio related
                        if (axisInput != 0)
                        {
                            PlayerActionsSteering[i] = -1;
                        }
                        else
                        {
                            PlayerActionsSteering[i] = 0;
                        }
                        PlayerActionsAcceleration[i] = 0;
                    } break;
                case Positions.SteerRight:
                    {
                        _steerRight += _maxSteeringAngle * axisInput / 2;
                        //set playeractions to neutral - audio related
                        if (axisInput != 0)
                        {
                            PlayerActionsSteering[i] = 1;
                        }
                        else
                        {
                            PlayerActionsSteering[i] = 0;
                        }
                        PlayerActionsAcceleration[i] = 0;
                    } break;
                case Positions.Brake:
                    {
                        _brake += _maxMotorTorque * axisInput / 2;
                        //adjust playeractions accordingly - audio related
                        if (axisInput != 0)
                        {
                            PlayerActionsAcceleration[i] = -1;
                        }
                        else
                        {
                            PlayerActionsAcceleration[i] = 0;
                        }
                        PlayerActionsSteering[i] = 0;
                    } break;
                case Positions.Gas:
                    {
                        _gas += _maxMotorTorque * axisInput / 2;
                        //adjust playeractions accordingly - audio related
                        if (axisInput != 0)
                        {
                            PlayerActionsAcceleration[i] = 1;
                        }
                        else
                        {
                            PlayerActionsAcceleration[i] = 0;
                        }
                        PlayerActionsSteering[i] = 0;
                    } break;
            }

            //MID-AIR ROTATING

            if (!_anywheelsGrounded)
            {
                float RightAxisInputX = Input.GetAxis("J" + _controllers[i] + "_RightHorizontal");
                float RightAxisInputY = Input.GetAxis("J" + _controllers[i] + "_RightVertical");

                    //get current rotation of car
                    Vector3 tempRotation = transform.eulerAngles;

                //lerp to rotation of ground
                tempRotation.x += -(_midAirRotationSpeed/_controllers.Count) * RightAxisInputY * Time.deltaTime;
                tempRotation.z += -(_midAirRotationSpeed / _controllers.Count)* RightAxisInputX * Time.deltaTime;

                //set rotation
                transform.rotation = Quaternion.Euler(tempRotation);

            }

        }
        //calculate acceleration level - audio related
        AccelerationLevel = PlayerActionsAcceleration[0] + PlayerActionsAcceleration[1] + PlayerActionsAcceleration[2] + PlayerActionsAcceleration[3];
        SteeringLevel = PlayerActionsSteering[0] + PlayerActionsSteering[1] + PlayerActionsSteering[2] + PlayerActionsSteering[3];




        //RESPAWNING
        //if any player is holding "b" show respawnbar
        if (CheckIfAnyPlayerIsHoldingB())
        {
            _respawnPanel.SetActive(true);
        }
        else
        {
            _respawnPanel.SetActive(false);

        }

        //if all players are holding "b", start respawntimer
        //when timer hits respawn time, respawn
        if (CheckIfAllPlayersAreHoldingB())
        {
            _respawnTimer += Time.deltaTime;

            if (_respawnTimer > _respawnTime)
            {
                _respawnScript.Respawn();
                _respawnTimer = 0;
            }
        }
        else
        {
            _respawnTimer = 0;
        }
        _respawnBar.value = _respawnTimer / _respawnTime;
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

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        //limit max speed
        LimitMaxSpeed();
    }

    private void LimitMaxSpeed()
    {
        int playersOnGas = 0;

        for (int i = 0; i < _defaultPositions.Count; i++)
        {
            if(_defaultPositions[i]._position== Positions.Gas)
            {
                playersOnGas = _defaultPositions[i]._amountOfPlayers;
                if (playersOnGas == 0) playersOnGas = 1;
                break;
            }
        }

        _maxSpeed = Mathf.Lerp(_maxSpeed, _maxSpeedDefault / 2* playersOnGas, .5f);

        if (_carRigidbody.velocity.magnitude > _maxSpeed)
        {
            _carRigidbody.velocity = _carRigidbody.velocity.normalized * _maxSpeed;
        }
    }

    private bool CheckIfAllPlayersAreHoldingB()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (!Input.GetButton("B" + _controllers[i] + "_XboxButton"))
                return false;
        }
        return true;
    }

    private bool CheckIfAnyPlayerIsHoldingB()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (Input.GetButton("B" + _controllers[i] + "_XboxButton"))
                return true;
        }
        return false;
    }

    bool CheckIfAnyWheelsAreGrounded()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.leftWheel.isGrounded)
                return true;
            if (axleInfo.rightWheel.isGrounded)
                return true;
        }
        return false;
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

    private void GlowCharacterIfClickOnButtonA(int playerIndex) {
        //use a component Halo to show the players if you are pressing the button or not

        Transform player = _playerPositions[playerIndex]._transform;

        Behaviour t = player.GetChild(0).GetComponent("Halo") as Behaviour;

        //Debug.Log("GlowCharacterIfClickOnButtonA");
        if (Input.GetButtonDown("A" + _controllers[playerIndex] + "_XboxButton"))
        {

            Debug.Log(t.name);
            t.enabled = true;
        }

        if (Input.GetButtonUp("A" + _controllers[playerIndex] + "_XboxButton"))
        {
            t.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SpeedUP")
        {
            Debug.Log("speed car up");
            StartCoroutine(SpeedUp());
        }

        if (other.tag == "SpeedDown")
        {
            Debug.Log("speed car down");
            StartCoroutine(SpeedDown());
        }
    }

    private IEnumerator SpeedUp()
    {
        float time = Time.time;
        while (Time.time - time < 1f)
        {
            _maxSpeed = _maxSpeed * 1.05f;
            _carRigidbody.velocity = _carRigidbody.velocity * 1.05f;
            yield return null;
        }

    }

    private IEnumerator SpeedDown()
    {
        float time = Time.time;
        while (Time.time - time < .4f)
        {
            _maxSpeed = _maxSpeed / 1.05f;
            _carRigidbody.velocity = _carRigidbody.velocity / 1.05f;
            yield return null;
        }

    }
}