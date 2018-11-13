using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _objectPositions; //0 - steerLeft  1 - steerRight    2 - brake     3 - accelerate

    [SerializeField]
    private float _switchSpeed;

    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        //diagionals
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
        {
            transform.position = Vector3.Lerp(transform.position, _objectPositions[0].transform.position, _switchSpeed * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
        {
            transform.position = Vector3.Lerp(transform.position, _objectPositions[1].transform.position, _switchSpeed * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
        {
            transform.position = Vector3.Lerp(transform.position, _objectPositions[2].transform.position, _switchSpeed * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
        {
            transform.position = Vector3.Lerp(transform.position, _objectPositions[3].transform.position, _switchSpeed * Time.deltaTime);
        }
        //horizontal/vertical
        if (Input.GetAxis("Horizontal")!=0 && Input.GetAxis("Vertical")==0)
        {
            transform.position = Vector3.Lerp(transform.position,_objectPositions)
        }
    }
}
