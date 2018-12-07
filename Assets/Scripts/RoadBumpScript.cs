using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBumpScript : MonoBehaviour {

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.GetComponent<CarControls>())
        {
            //get positionhandler (first child)
            collision.transform.GetComponent<PositionHandlerScript>().ScramblePlayers();
            Debug.Log("scramble");
        }
    }
}
