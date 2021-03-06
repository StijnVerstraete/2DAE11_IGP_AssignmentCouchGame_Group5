﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    CoOp, Teams
}

public class GameModeHandler : MonoBehaviour {

    [SerializeField] private Transform _playerCars;
    [SerializeField] private Transform _botCars;
    [SerializeField] private Transform _startPositions;
    [SerializeField] private Camera[] _camerasCars;

    private GameMode _gameMode;

    private List<int> _players = new List<int>();
    private List<int> _team0Controllers= new List<int>();
    private List<int> _team1Controllers = new List<int>();
    private List<int> _team0Players = new List<int>();
    private List<int> _team1Players = new List<int>();

    // Use this for initialization
    void Start () {
        _gameMode = (GameMode)System.Enum.Parse(typeof(GameMode), PlayerPrefs.GetString("GameMode", "CoOp"));

        switch (_gameMode)
        {
            case GameMode.CoOp:
                {
                    SpawnCars(1, PlayerPrefs.GetInt("AmountOfBots", 0));

                    for (int i = 1; i <= PlayerPrefs.GetInt("MaxPlayers", 4); i++)
                    {
                        if (PlayerPrefs.GetInt("Player" + i + "Console", 0) != 0)
                        {
                            _players.Add(PlayerPrefs.GetInt("Player" + i + "Console"));
                        }
                    }
                    _playerCars.GetChild(0).GetComponent<PositionHandlerScript>().AssignControllersToPlayers(_players.ToArray());
                }
                break;
            case GameMode.Teams:
                {
                    //spawn 2 cars and a given amount of bots
                    SpawnCars(2, PlayerPrefs.GetInt("AmountOfBots", 0));
                    //for each possible player
                    for (int i = 1; i <= PlayerPrefs.GetInt("MaxPlayers"); i++)
                    {
                        //if player has a controller connected
                        int controller = PlayerPrefs.GetInt("Player" + (i) + "Console");
                        if (controller != 0)
                        {
                            int team = PlayerPrefs.GetInt("Player" + (i) + "Team");

                            if (team == 0)
                            {
                                _team0Controllers.Add(controller);
                                _team0Players.Add(i);
                            }

                            if (team == 1)
                            {
                                _team1Controllers.Add(controller);
                                _team1Players.Add(i);
                            }
                            
                        }
                    }
                    _playerCars.GetChild(0).GetComponent<PositionHandlerScript>().AssignControllersToPlayers(_team0Controllers.ToArray(), _team0Players.ToArray());
                    _playerCars.GetChild(1).GetComponent<PositionHandlerScript>().AssignControllersToPlayers(_team1Controllers.ToArray(),_team1Players.ToArray());

                    _camerasCars[0].rect= new Rect(0,0,.5f,1);
                    _camerasCars[1].rect = new Rect(0.5f, 0, .5f, 1);
                }
                break;
        }
	}

    void SpawnPlayerCars(int amountOfPlayerCars)
    {
        for (int i = 0; i < amountOfPlayerCars; i++)
        {
            //set position
            _playerCars.GetChild(i).position = _startPositions.GetChild(i).position;
            //set respawnpoint
            _playerCars.GetChild(i).GetComponent<RespawnScript>().RespawnPoint = _startPositions.GetChild(i);
            //set car active
            _playerCars.GetChild(i).gameObject.SetActive(true);
        }
    }

    void SpawnBotCars(int amountOfPlayerCars, int amountOfBots)
    {
        for (int i = amountOfPlayerCars; i < amountOfBots+amountOfPlayerCars; i++)
        {
            //set position
            _botCars.GetChild(i-amountOfPlayerCars).position = _startPositions.GetChild(i).position;
            //set respawnpoint
            _botCars.GetChild(i - amountOfPlayerCars).GetComponent<RespawnScript>().RespawnPoint = _startPositions.GetChild(i);
            //set car active
            _botCars.GetChild(i-amountOfPlayerCars).gameObject.SetActive(true);
        }
    }

    void SpawnCars(int amountOfPlayerCars, int amountOfBots)
    {
        SpawnPlayerCars(amountOfPlayerCars);
        SpawnBotCars(amountOfPlayerCars,amountOfBots);
    }
	
}
