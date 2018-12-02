using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBumpScript : MonoBehaviour {

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.GetComponent<CarControls>())
        {
            collision.transform.GetChild(0).GetComponent<PositionHandlerScript>().ScramblePlayers();
            Debug.Log("scramble");
        }
    }
}
