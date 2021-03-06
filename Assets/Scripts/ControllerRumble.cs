﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerRumble : MonoBehaviour {

    private float _rumbleLength = 0.2f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //stop rumble after short burst
        if (_rumbleLength > 0)
        {
            _rumbleLength -= Time.fixedDeltaTime;
        }
        if (_rumbleLength <=0)
        {
            _rumbleLength = 0;

            foreach (int controller in gameObject.GetComponent<PositionHandlerScript>().Controllers)
            {
                if (controller == 1) { GamePad.SetVibration(PlayerIndex.One, 0f, 0f); }
                if (controller == 2) { GamePad.SetVibration(PlayerIndex.Two, 0f, 0f); }
                if (controller == 3) { GamePad.SetVibration(PlayerIndex.Three, 0f, 0f); }
                if (controller == 4) { GamePad.SetVibration(PlayerIndex.Four, 0f, 0f); }
            }
        }
        //Debug.Log(_rumbleLength);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //rumble
        if (collision.gameObject.tag == "Rumble")
        {
            foreach (int controller in gameObject.GetComponent<PositionHandlerScript>().Controllers)
            {
                if (controller == 1) { GamePad.SetVibration(PlayerIndex.One, 5f, 5f); }
                if (controller == 2) { GamePad.SetVibration(PlayerIndex.Two, 5f, 5f); }
                if (controller == 3) { GamePad.SetVibration(PlayerIndex.Three, 5f, 5f); }
                if (controller == 4) { GamePad.SetVibration(PlayerIndex.Four, 5f, 5f); }
            }
            _rumbleLength = 0.2f;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //rumble
        if (collision.gameObject.tag == "Rumble")
        {
            foreach (int controller in gameObject.GetComponent<PositionHandlerScript>().Controllers)
            {
                if (controller == 1) { GamePad.SetVibration(PlayerIndex.One, 5f, 5f); }
                if (controller == 2) { GamePad.SetVibration(PlayerIndex.Two, 5f, 5f); }
                if (controller == 3) { GamePad.SetVibration(PlayerIndex.Three, 5f, 5f); }
                if (controller == 4) { GamePad.SetVibration(PlayerIndex.Four, 5f, 5f); }
            }
            _rumbleLength = 0.2f;
        }
    }
}
