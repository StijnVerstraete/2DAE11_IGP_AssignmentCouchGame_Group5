using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Positions
{
    SteerLeft, SteerRight, Brake, Gas
}

public abstract class Position
{
    public Positions _position;
    public Transform _transform;
}

[System.Serializable]
public class PlayerPosition:Position
{
    public bool _isInPosition;
}

[System.Serializable]
public class DefaultPosition:Position
{
    public int _amountOfPlayers;
}

public class PositionHandlerScript : MonoBehaviour {

    [SerializeField] private float _switchSpeed;

    [SerializeField] private int _maxAmountOfPlayersOnPosition;

    [SerializeField] public List<DefaultPosition> DefaultPositions;

    [SerializeField] public List<PlayerPosition> PlayerPositions;

    void Start ()
    {
        //set players to right positions
        for (int i = 0; i < PlayerPrefs.GetInt("AmountOfPlayers",0); i++)
        {
                PlayerPositions[i]._transform.position = DefaultPositions[i]._transform.position;

                //set how many players are on position
                ++DefaultPositions[i]._amountOfPlayers;
        }

    }


    // Update is called once per frame
    void Update ()
    {
        //for every player
        for (int i = 0; i < PlayerPrefs.GetInt("AmountOfPlayers",0); i++)
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
                    for (int j = 0; j < DefaultPositions.Count; j++)
                    {

                        if (PlayerPositions[i]._position == DefaultPositions[j]._position)
                        {
                            PlayerPositions[i]._transform.position = Vector3.MoveTowards(PlayerPositions[i]._transform.position, DefaultPositions[j]._transform.position, _switchSpeed * Time.deltaTime);
                        }
                            
                    }
                    
                }
        }
        
    }

    private bool CheckIfPlayerIsOnRightPosition(int playerNumber)
    {
        for(int i=0; i < DefaultPositions.Count; i++)
        {
            if (PlayerPositions[playerNumber]._position == DefaultPositions[i]._position&& PlayerPositions[playerNumber]._transform.position == DefaultPositions[i]._transform.position)
                return true;
        }
        return false;
    }

    private void SetDestination(int playerNumber)
    {
        float xAxisInput = Input.GetAxis("J" + (playerNumber + 1) + "_XboxHorizontal");
        float yAxisInput = Input.GetAxis("J" + (playerNumber + 1) + "_XboxVertical");

        //only if player uses joystick
        if (yAxisInput !=0|| xAxisInput != 0)
        {
            //get the angle from the joystick
            float angle;
            if (yAxisInput >= 0)
                angle = Vector2.Angle(Vector2.right, new Vector2(xAxisInput, yAxisInput));
            else
                angle = Vector2.Angle(Vector2.left, new Vector2(xAxisInput, yAxisInput)) + 180;
            //Debug.Log(angle);


            //tell the player where togo
            Positions pos = PlayerPositions[playerNumber]._position;

            if (angle >= 315 || angle < 45f) //go right
            {
                if (pos == Positions.SteerLeft)
                    SwitchPosition(playerNumber, pos, Positions.SteerRight);
                if(pos == Positions.Brake)
                    SwitchPosition(playerNumber, pos, Positions.Gas);
                Debug.Log("player " + playerNumber + "go right");
            }
            else
            {
                if (angle >= 135 && angle < 225f) //go left
                {
                    Debug.Log("player " + playerNumber + "go left");
                    if (pos == Positions.SteerRight)
                        SwitchPosition(playerNumber, pos, Positions.SteerLeft);
                    if (pos == Positions.Gas)
                        SwitchPosition(playerNumber, pos, Positions.Brake); 
                }
                else
                {
                    if (angle >= 225 && angle < 315f) //go down
                    {
                        Debug.Log("player " + playerNumber + "go down");
                        if (pos == Positions.SteerLeft)
                            SwitchPosition(playerNumber, pos, Positions.Brake);
                        if (pos == Positions.SteerRight)
                            SwitchPosition(playerNumber, pos, Positions.Gas);
                    }
                    else
                    {
                        if (angle >= 45 && angle < 135f) //go up
                        {
                            Debug.Log("player " + playerNumber + "go up");
                            if (pos == Positions.Brake)
                                SwitchPosition(playerNumber, pos, Positions.SteerLeft);
                            if (pos == Positions.Gas)
                                SwitchPosition(playerNumber, pos, Positions.SteerRight);
                        }
                    }
                }
            }
        }
       
        //8-directional code

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

    private void SwitchPosition(int playerNumber, Positions currentPosition, Positions targetPosition)
    {
        //for each position
        for (int i = 0; i < DefaultPositions.Count; i++)
        {
            //get the target position
            if (DefaultPositions[i]._position == targetPosition)
            {
                //check if there is space for the player
                if (DefaultPositions[i]._amountOfPlayers < _maxAmountOfPlayersOnPosition)
                {
                    //set the position of the player
                    PlayerPositions[playerNumber]._position = DefaultPositions[i]._position;

                    //subtract 1 player from the old position
                    for (int j = 0; j < DefaultPositions.Count; j++)
                    {
                        if (DefaultPositions[j]._position == currentPosition)
                        {
                            if (DefaultPositions[j]._amountOfPlayers != 0)
                                --DefaultPositions[j]._amountOfPlayers;
                        }
                    }

                    //add 1 player to the new position
                    ++DefaultPositions[i]._amountOfPlayers;
                }
            }
        }
    }

    private void SetPosition(int playerNumber)
    {
        for (int i = 0; i < DefaultPositions.Count; i++)
        {
            if (PlayerPositions[playerNumber]._position == DefaultPositions[i]._position)
            {
                PlayerPositions[playerNumber]._transform.position = DefaultPositions[i]._transform.position;
            }
        }
    }

    public void ScramblePlayers()
    {
        //for each player
        for(int i= 0; i < PlayerPrefs.GetInt("AmountOfPlayers");i++)
        {
            int rand = UnityEngine.Random.Range(0, DefaultPositions.Count);
            SwitchPosition(i, PlayerPositions[i]._position, DefaultPositions[rand]._position);
        }
    }
}
