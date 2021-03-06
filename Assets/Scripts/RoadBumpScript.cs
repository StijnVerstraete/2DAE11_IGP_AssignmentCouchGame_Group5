﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class RoadBumpScript : MonoBehaviour {

    [SerializeField] private GameObject _camera;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.GetComponent<CarControls>())
        {
            //get positionhandler
            collision.transform.GetComponent<PositionHandlerScript>().ScramblePlayers();
            Debug.Log("scramble");
            
            if (collision.tag == "PlayerCar")
            {
                _camera = collision.transform.Find("Edge Detection Camera").gameObject;
                //apply screenshake
                _camera.GetComponent<ScreenShake>().enabled = true;
                _camera.GetComponent<ScreenShake>().ShakeDuration = 2;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerCar")
        {
            //stop screenshake
            _camera.GetComponent<ScreenShake>().ShakeDuration = 0;
        }
    }
}
