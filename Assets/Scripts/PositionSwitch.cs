using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _objectPositions; //0 - steerLeft  1 - steerRight    2 - brake     3 - accelerate

    [SerializeField]
    private float _switchSpeed;

    private int _actualPosition;

    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        //check what the current position is
        for (int i = 0; i < _objectPositions.Length; i++)
        {
            if (transform.position == _objectPositions[i].transform.position)
            {
                _actualPosition = i;
            }
        }

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
            int pos = _actualPosition +(int)Mathf.Sign(Input.GetAxis("Horizontal"));
            pos %= 3;
            if (pos < 0) { pos = 0; }
            
            transform.position = Vector3.Lerp(transform.position, _objectPositions[pos].transform.position, _switchSpeed * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") != 0 && Input.GetAxis("Horizontal") == 0)
        {
            int pos = _actualPosition + ((int)Mathf.Sign(Input.GetAxis("Horizontal"))*2);
            pos %= 3;
            if (pos < 0) { pos = 0; }
            transform.position = Vector3.Lerp(transform.position, _objectPositions[pos].transform.position, _switchSpeed * Time.deltaTime);
        }
    }
}
