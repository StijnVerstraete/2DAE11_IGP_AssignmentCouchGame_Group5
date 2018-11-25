﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
public class OponentController : MonoBehaviour {

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _endPosition;

    public List<AxleInfo> axleInfos;

    [SerializeField] private float _maxMotorTorque;

    [SerializeField] private float _maxBreakTorque;

    [SerializeField] private float _maxSteeringAngle;

    [SerializeField] private float _maxSpeed;

    [SerializeField] private Rigidbody _carRigidbody;

    // Use this for initialization
    void Start () {
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.SetDestination(_endPosition.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float _steeringAngle=0, _motorTorque=0;

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
                axleInfo.leftWheel.motorTorque = _motorTorque;
                axleInfo.rightWheel.motorTorque = _motorTorque;
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
    }


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