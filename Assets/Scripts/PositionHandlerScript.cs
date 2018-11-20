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

public class PositionHandlerScript : MonoBehaviour {

    [SerializeField]
    private float _switchSpeed;

    [SerializeField]
    private List<PlayerPosition> _defaultPositions;

    [SerializeField]
    private List<PlayerPosition> _playerPositions;

    void Start ()
    {
        //set players to right positions
        for (int i = 0; i < _playerPositions.Count; i++)
        {
            if(_playerPositions[i]._transform!=null)
            _playerPositions[i]._transform.position = _defaultPositions[i]._transform.position;
        }
    }


    // Update is called once per frame
    void Update ()
    {
        //for every player
        for (int i = 0; i < _playerPositions.Count; i++)
        {
            //if there is a player playing
            if(_playerPositions[i]._transform != null)
            {
                //if player is on his position, he can switch
                if (CheckIfPlayerIsOnRightPosition(i))
                {
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
            if (angle >= 0 && angle < 90f)
            {
                _playerPositions[playerNumber]._position = Position.SteerRight;
                Debug.Log("go steerright");
            }
            else
            {
                if (angle >= 90 && angle < 180f)
                {
                    Debug.Log("go steerleft");
                    _playerPositions[playerNumber]._position = Position.SteerLeft;
                }
                else
                {
                    if (angle >= 180 && angle < 270f)
                    {
                        Debug.Log("go brake");
                        _playerPositions[playerNumber]._position = Position.Brake;
                    }
                    else
                    {
                        if (angle >= 270 && angle < 360f)
                        {
                            Debug.Log("go gas");
                            _playerPositions[playerNumber]._position = Position.Gas;
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
