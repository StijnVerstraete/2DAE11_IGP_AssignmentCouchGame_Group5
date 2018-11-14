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

    private int _destinationPosition;

    [SerializeField]
    private bool _diagonalMovement = false;

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
        if (_actualPosition == _destinationPosition)
        {
            SetDestination();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _objectPositions[_destinationPosition].transform.position, _switchSpeed * Time.deltaTime);
        }
        
    }
    private void SetDestination()
    {
        #region diagonalmovement
        //diagionals
        if (_diagonalMovement == true)
        {
            if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
            {
                _destinationPosition = 0;
            }
            if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
            {
                _destinationPosition = 1;
            }
            if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
            {
                _destinationPosition = 2;
            }
            if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
            {
                _destinationPosition = 3;
            }
        }

        #endregion diagonalmovement
        #region verticalhorizontamovement
        if (_diagonalMovement == false)
        {
            //horizontal
            if ((_actualPosition == 0 || _actualPosition == 2) && Input.GetAxis("Horizontal") > 0)
            {
                _destinationPosition = _actualPosition + 1;
            }
            if ((_actualPosition == 1 || _actualPosition == 3) && Input.GetAxis("Horizontal") < 0)
            {
                _destinationPosition = _actualPosition - 1;
            }
            //vertical
            if ((_actualPosition == 0 || _actualPosition == 1) && Input.GetAxis("Vertical") > 0)
            {
                _destinationPosition = _actualPosition + 2;
            }
            if ((_actualPosition == 2 || _actualPosition == 3) && Input.GetAxis("Vertical") < 0)
            {
                _destinationPosition = _actualPosition - 2;
            }
        }

        #endregion verticalhorizontamovement
    }
}
