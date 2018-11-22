using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum Position
{
    SteerLeft, SteerRight, Brake, Gas
}

[System.Serializable]
public class PlayerPosition
{
    public Position _position;
    public Transform _transform;
}

[System.Serializable]
public class DefaultPosition:PlayerPosition
{
    public int _amountOfPLayers;
}

public class PositionHandlerScript : MonoBehaviour {

    [SerializeField]
    private float _switchSpeed;

    [SerializeField]
    private int _maxAmountOfPlayersOnPosition;

    [SerializeField]
    private List<DefaultPosition> _defaultPositions;

    [SerializeField]
    private List<PlayerPosition> _playerPositions;

    void Start ()
    {
        //set players to right positions
        for (int i = 0; i < _playerPositions.Count; i++)
        {
            if (_playerPositions[i]._transform != null)
            {
                _playerPositions[i]._transform.position = _defaultPositions[i]._transform.position;

                //set how many players are on position
                ++_defaultPositions[i]._amountOfPLayers;
            }
        }

    }


    // Update is called once per frame
    void Update ()
    {
        //string[] temp = Input.GetJoystickNames();
        //for (int i = 0; i < temp.Length; i++)
        //{
        //    Debug.Log("player " + i+ ": " +temp[i].ToString());
        //}

        //for every player
        for (int i = 0; i < _playerPositions.Count; i++)
        {
            //if there is a player playing
            if(_playerPositions[i]._transform != null)
            {
                //if player is on his position, he can switch
                if (CheckIfPlayerIsOnRightPosition(i))
                {
                    //Debug.Log("player " + i + " set dest");
                    SetDestination(i);
                    //SetPosition(i);
                }
                else
                {
                    //else move to the right position
                    for (int j = 0; j < _defaultPositions.Count; j++)
                    {

                        if (_playerPositions[i]._position == _defaultPositions[j]._position)
                        {
                            _playerPositions[i]._transform.position = Vector3.MoveTowards(_playerPositions[i]._transform.position, _defaultPositions[j]._transform.position, _switchSpeed * Time.deltaTime);
                        }
                            
                    }
                    
                }
            }
        }
        
    }

    private bool CheckIfPlayerIsOnRightPosition(int playerNumber)
    {
        for(int i=0; i < _defaultPositions.Count; i++)
        {
            if (_playerPositions[playerNumber]._position == _defaultPositions[i]._position&& _playerPositions[playerNumber]._transform.position == _defaultPositions[i]._transform.position)
                return true;
        }
        return false;
    }

    private void SetDestination(int playerNumber)
    {
        string xAxis = "J" + (playerNumber+1) + "_XboxHorizontal";
        string yAxis = "J" + (playerNumber+1) + "_XboxVertical";

        //only if player uses joystick
        if (Input.GetAxis(yAxis)!=0|| Input.GetAxis(xAxis) != 0)
        {
            //get the angle from the joystick
            float angle;
            if (Input.GetAxis(yAxis) >= 0)
                angle = Vector2.Angle(Vector2.right, new Vector2(Input.GetAxis(xAxis), Input.GetAxis(yAxis)));
            else
                angle = Vector2.Angle(Vector2.left, new Vector2(Input.GetAxis(xAxis), Input.GetAxis(yAxis))) + 180;
            Debug.Log(angle);


            //tell the player where togo
            Position pos = _playerPositions[playerNumber]._position;
            if (angle >= 315 || angle < 45f) //go right
            {
                if (pos == Position.SteerLeft)
                    SwitchPosition(playerNumber, pos, Position.SteerRight);
                if(pos == Position.Brake)
                    SwitchPosition(playerNumber, pos, Position.Gas);
                Debug.Log("player " + playerNumber + "go right");
            }
            else
            {
                if (angle >= 135 && angle < 225f) //go left
                {
                    Debug.Log("player " + playerNumber + "go left");
                    if (pos == Position.SteerRight)
                        SwitchPosition(playerNumber, pos, Position.SteerLeft);
                    if (pos == Position.Gas)
                        SwitchPosition(playerNumber, pos, Position.Brake); 
                }
                else
                {
                    if (angle >= 225 && angle < 315f) //go down
                    {
                        Debug.Log("player " + playerNumber + "go down");
                        if (pos == Position.SteerLeft)
                            SwitchPosition(playerNumber, pos, Position.Brake);
                        if (pos == Position.SteerRight)
                            SwitchPosition(playerNumber, pos, Position.Gas);
                    }
                    else
                    {
                        if (angle >= 45 && angle < 135f) //go up
                        {
                            Debug.Log("player " + playerNumber + "go up");
                            if (pos == Position.Brake)
                                SwitchPosition(playerNumber, pos, Position.SteerLeft);
                            if (pos == Position.Gas)
                                SwitchPosition(playerNumber, pos, Position.SteerRight);
                        }
                    }
                }
            }
        }
            
        //if(angle>337.5f || angle <= 22.5f)
        //{
        //    _playersPositions[playerNumber]._positionX = PositionX.Right;
        //}
        //else
        //{
        //    if (angle > 22.5f && angle <= 67.5f)
        //    {
        //        _playersPositions[playerNumber]._positionX = PositionX.Right;
        //        _playersPositions[playerNumber]._positionY = PositionY.Top;
        //    }
        //    else
        //    {
        //        if (angle > 67.5f && angle <= 112.5f)
        //        {
        //            _playersPositions[playerNumber]._positionY = PositionY.Top;
        //        }
        //        else
        //        {
        //            if (angle > 112.5f && angle <= 157.5f)
        //            {
        //                _playersPositions[playerNumber]._positionX = PositionX.Left;
        //                _playersPositions[playerNumber]._positionY = PositionY.Top;
        //            }
        //            else
        //            {
        //                if (angle > 157.5f && angle <= 202.5f)
        //                {
        //                    _playersPositions[playerNumber]._positionX = PositionX.Left;
        //                }
        //                else
        //                {
        //                    if (angle > 202.5f && angle <= 247.5f)
        //                    {
        //                        _playersPositions[playerNumber]._positionX = PositionX.Left;
        //                        _playersPositions[playerNumber]._positionY = PositionY.Bottom;
        //                    }
        //                    else
        //                    {
        //                        if (angle > 247.5f && angle <= 292.5f)
        //                        {
        //                            _playersPositions[playerNumber]._positionY = PositionY.Bottom;
        //                        }
        //                        else
        //                        {
        //                            if (angle > 292.5f && angle <= 337.5f)
        //                            {
        //                                _playersPositions[playerNumber]._positionX = PositionX.Right;
        //                                _playersPositions[playerNumber]._positionY = PositionY.Top;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }

    private void SwitchPosition(int playerNumber, Position currentPosition, Position targetPosition)
    {
        for (int i = 0; i < _defaultPositions.Count; i++)
        {
            //get the target position
            if (_defaultPositions[i]._position == targetPosition)
            {
                //check if there is space for the player
                if (_defaultPositions[i]._amountOfPLayers < _maxAmountOfPlayersOnPosition)
                {
                    //set the position of the player
                    _playerPositions[playerNumber]._position = _defaultPositions[i]._position;

                    //subtract 1 player from the old position
                    for (int j = 0; j < _defaultPositions.Count; j++)
                    {
                        if (_defaultPositions[j]._position == currentPosition)
                        {
                            if (_defaultPositions[j]._amountOfPLayers != 0)
                                --_defaultPositions[j]._amountOfPLayers;
                        }
                    }

                    //add 1 player to the new position
                    ++_defaultPositions[i]._amountOfPLayers;
                }
            }
        }
    }

    private void SetPosition(int playerNumber)
    {
        for (int i = 0; i < _defaultPositions.Count; i++)
        {
            if (_playerPositions[playerNumber]._position == _defaultPositions[i]._position)
            {
                _playerPositions[playerNumber]._transform.position = _defaultPositions[i]._transform.position;
            }
        }
    }
}
