using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RespawnScript))]
public class OpponentBehaviour : MonoBehaviour {

    public List<AxleInfo> axleInfos;

    [SerializeField] private float _maxMotorTorque;

    [SerializeField] private float _maxBreakTorque;

    [SerializeField] private float _maxSteeringAngle;

    [SerializeField] private float _maxSpeed;

    [SerializeField] private Rigidbody _carRigidbody;

    private Transform _transform;

    private float _steeringAngle=0;

    private RespawnScript _respawnScript;

    [SerializeField] private float _respawnTime;

    private float _respawntimer = 0;

    public void Start()
    {
        _transform = GetComponent<Transform>();
        _respawnScript = GetComponent<RespawnScript>();
    }

    public void FixedUpdate()
    {
        RaycastHit hit;
        float _angleRight = 0, _angleLeft = 0;
        Vector3 _hitPointLeft= Vector3.zero, _hitPointRight=Vector3.zero;
        if (Physics.Raycast(_transform.position, _transform.right, out hit, 100f, 1 << LayerMask.NameToLayer("Map")))
        {
            _angleRight = Vector3.Angle(_transform.forward , hit.normal);
            _hitPointRight = hit.point;
        }
        if (Physics.Raycast(_transform.position, -_transform.right, out hit, 100f, 1 << LayerMask.NameToLayer("Map")))
        {
            _angleLeft = Vector3.Angle(_transform.forward, hit.normal);
            _hitPointLeft = hit.point;
        }
        //Debug.Log("_angleRight: "+ _angleRight);
        //Debug.Log("_angleLeft: " + _angleLeft);

        //bocht die naar links gaat
        if(_angleRight>=90 && _angleLeft <= 90)
        {
            float avg = (_angleRight+_angleLeft) / 2;
            //_steeringAngle = Mathf.Lerp(_steeringAngle, -_angleRight + 90, 1f);
            //_steeringAngle = Mathf.Lerp(_steeringAngle, avg-90, 1f);
            _steeringAngle = Mathf.Lerp(_steeringAngle, _angleLeft - 90, 1f);
            //Debug.Log("avg: " + avg);

        }
        else
        {
            //bocht die naar rechts gaat
            if(_angleRight <= 90 && _angleLeft >= 90)
            {
                float avg = (_angleRight +_angleLeft) / 2;
                //_steeringAngle = Mathf.Lerp(_steeringAngle, _angleLeft + 90, 1f);
                //_steeringAngle = Mathf.Lerp(_steeringAngle, avg-90, 1f);
                _steeringAngle = Mathf.Lerp(_steeringAngle, -_angleRight + 90, 1f);
            }
            else
            {
                //weg versmalling
                if(_angleRight >= 90 && _angleLeft >= 90)
                {
                    if ((_transform.position - _hitPointLeft).magnitude > (_transform.position - _hitPointRight).magnitude)
                    {
                        _steeringAngle = Mathf.Lerp(_steeringAngle, -_angleRight + 90, 1f);
                    }
                    else
                    {
                        _steeringAngle = Mathf.Lerp(_steeringAngle, _angleLeft - 90, 1f);
                    }
                }
                else
                {
                    //weg verbreeding
                    if (_angleRight <= 90 && _angleLeft <= 90)
                    {
                        if ((_transform.position - _hitPointLeft).magnitude > (_transform.position - _hitPointRight).magnitude)
                        {
                            _steeringAngle = Mathf.Lerp(_steeringAngle, -_angleRight + 90, 1f);
                        }
                        else
                        {
                            _steeringAngle = Mathf.Lerp(_steeringAngle, _angleLeft - 90, 1f);
                        }
                    }
                }
            }
        }
        _steeringAngle = Mathf.Clamp(_steeringAngle, -30, 30);

        //for both the front and back axle
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.isSteering)
            {
                axleInfo.leftWheel.steerAngle = _steeringAngle;
                axleInfo.rightWheel.steerAngle = _steeringAngle;
            }
            if (axleInfo.isMotor)
            {
                axleInfo.leftWheel.motorTorque = _maxMotorTorque;
                axleInfo.rightWheel.motorTorque = _maxMotorTorque;
            }
            //if (axleInfo.isBreak)
            //{
            //    axleInfo.leftWheel.brakeTorque = _break;
            //    axleInfo.rightWheel.brakeTorque = _break;
            //}
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        //rotate car if it's flying through air
        if (!CheckIfAnyWheelsAreGrounded())
        {
            if (Physics.Raycast(_transform.position, Vector3.down, out hit, 100f, 1 << LayerMask.NameToLayer("Map")))
            {
                //get the rotation of the ground
                Quaternion groundRotation = Quaternion.LookRotation(Vector3.Cross(hit.normal, Vector3.down));

                //get current rotation of car
                Vector3 tempRotation = _transform.eulerAngles;

                //lerp to rotation of ground
                tempRotation.x = Mathf.LerpAngle(tempRotation.x, groundRotation.eulerAngles.x, .04f);
                tempRotation.z = Mathf.LerpAngle(tempRotation.z, groundRotation.eulerAngles.z, .04f);

                _transform.eulerAngles = tempRotation;

                //_transform.rotation = Quaternion.RotateTowards(_transform.rotation, tempQ, 2f);

                //Debug.Log("transform x: " + _transform.eulerAngles.x + " y: " + _transform.eulerAngles.y + " z: " + _transform.eulerAngles.z +
                //    "    plane x: " + groundRotation.eulerAngles.x + " y: " + groundRotation.eulerAngles.y + " z: " + groundRotation.eulerAngles.z);
            }
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

    bool CheckIfAnyWheelsAreGrounded()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.leftWheel.isGrounded)
                return true;
            if(axleInfo.rightWheel.isGrounded)
                return true;
        }
        return false;
    }

    private void OnCollisionStay(Collision collision)
    {
        _respawntimer += Time.deltaTime;

        if (_respawntimer > _respawnTime)
        {
            if (!CheckIfAnyWheelsAreGrounded())
            {
                _respawnScript.Respawn();
            }
            _respawntimer = 0;
        }


        //temp code, may be removed
        if (_carRigidbody.velocity.magnitude<2)
        {
            //_respawnScript.Respawn();
            //_respawntimer = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _respawntimer = 0;
    }

}
