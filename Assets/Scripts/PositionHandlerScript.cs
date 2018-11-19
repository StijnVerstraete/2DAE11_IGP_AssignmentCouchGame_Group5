using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PositionX
{
    Left, Right
}
enum PositionY
{
    Top, Bottom
}

class PlayerPosition
{
    public PositionX _positionX;
    public PositionY _positionY;

    public PlayerPosition(PositionX xpos, PositionY ypos)
    {
        _positionX = xpos;
        _positionY = ypos;
    }
}

public class PositionHandlerScript : MonoBehaviour {
    [SerializeField]
    private Transform[] _objectPositions; //0 - steerLeft  1 - steerRight    2 - brake     3 - accelerate

    [SerializeField]
    private GameObject[] _players;

    [SerializeField]
    private float _switchSpeed;


    private PlayerPosition _actualPosition;
    private int _destinationPosition;

    //private List<PlayerPosition> _playersPositions;
    private PlayerPosition[] _targetPositions;
    private PlayerPosition[] _playersPositions;

    [SerializeField]
    private bool _diagonalMovement = false;

    void Start ()
    {
        _targetPositions = new PlayerPosition[]{ new PlayerPosition(PositionX.Left, PositionY.Top), new PlayerPosition(PositionX.Right, PositionY.Top),
        new PlayerPosition(PositionX.Left, PositionY.Bottom),new PlayerPosition(PositionX.Right, PositionY.Bottom)};

        _playersPositions = _targetPositions;

        //set start position for each player
        for(int i=0; i < _players.Length; i++)
        {
            _players[i].transform.position = _objectPositions[i].position;
        }
	}


    // Update is called once per frame
    void Update ()
    {

        for (int i = 0; i < _players.Length; i++)
        {
            if (Input.GetAxis("J1_XboxHorizontal") > 0 || Input.GetAxis("J1_XboxVertical") > 0 || Input.GetAxis("J1_XboxHorizontal") < 0 || Input.GetAxis("J1_XboxVertical") < 0)
            {
                SetDestination(i);
                SetPosition(i);
            }
            
            ////check what the current position is
            //for (int j = 0; j < _objectPositions.Length; j++)
            //{
            //    if (_players[i].transform.position == _objectPositions[j].transform.position)
            //    {
            //        _actualPosition = _playersPositions[j];
            //    }
            //}

            //if (_actualPosition._positionX == _destinationPosition)
            //{
            //    SetDestination(i);
            //}
            //else
            //{
            //    transform.position = Vector3.MoveTowards(transform.position, _objectPositions[_destinationPosition].transform.position, _switchSpeed * Time.deltaTime);
            //}
        }


        
    }

    private void SetPosition(int playerNumber)
    {
        for(int i=0; i < _targetPositions.Length; i++)
        {
            if(_playersPositions[playerNumber]._positionX==_targetPositions[i]._positionX && _playersPositions[playerNumber]._positionY == _targetPositions[i]._positionY)
            {
                _players[playerNumber].transform.position = _objectPositions[i].position;
            }
        }
    }

    private void SetDestination(int playerNumber)
    {
        //get the angle from the joystick
        float angle;
        if (Input.GetAxis("J1_XboxVertical") >=0)
        angle = Vector2.Angle(Vector2.right, new Vector2(Input.GetAxis("J1_XboxHorizontal"), Input.GetAxis("J1_XboxVertical")));
        else
            angle = Vector2.Angle(Vector2.left, new Vector2(Input.GetAxis("J1_XboxHorizontal"), Input.GetAxis("J1_XboxVertical"))) +180;
        Debug.Log(angle);

        //set the position the player has to go to
        if(angle>337.5f || angle <= 22.5f)
        {
            _playersPositions[playerNumber]._positionX = PositionX.Right;
        }
        else
        {
            if (angle > 22.5f && angle <= 67.5f)
            {
                _playersPositions[playerNumber]._positionX = PositionX.Right;
                _playersPositions[playerNumber]._positionY = PositionY.Top;
            }
            else
            {
                if (angle > 67.5f && angle <= 112.5f)
                {
                    _playersPositions[playerNumber]._positionY = PositionY.Top;
                }
                else
                {
                    if (angle > 112.5f && angle <= 157.5f)
                    {
                        _playersPositions[playerNumber]._positionX = PositionX.Left;
                        _playersPositions[playerNumber]._positionY = PositionY.Top;
                    }
                    else
                    {
                        if (angle > 157.5f && angle <= 202.5f)
                        {
                            _playersPositions[playerNumber]._positionX = PositionX.Left;
                        }
                        else
                        {
                            if (angle > 202.5f && angle <= 247.5f)
                            {
                                _playersPositions[playerNumber]._positionX = PositionX.Left;
                                _playersPositions[playerNumber]._positionY = PositionY.Bottom;
                            }
                            else
                            {
                                if (angle > 247.5f && angle <= 292.5f)
                                {
                                    _playersPositions[playerNumber]._positionY = PositionY.Bottom;
                                }
                                else
                                {
                                    if (angle > 292.5f && angle <= 337.5f)
                                    {
                                        _playersPositions[playerNumber]._positionX = PositionX.Right;
                                        _playersPositions[playerNumber]._positionY = PositionY.Top;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
