﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour {

    public Transform RespawnPoint;

    [SerializeField] private bool _isOpponent;

    public void Respawn()
    {
        transform.position = RespawnPoint.position;
        transform.rotation = RespawnPoint.localRotation;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Debug.Log("Respawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RespawnPoint")
        {
            if (_isOpponent && RespawnPoint == other.transform)
                Respawn();

            RespawnPoint = other.transform;
        }
    }
}
