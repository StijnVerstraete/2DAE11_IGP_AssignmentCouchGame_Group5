using System.Collections;
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

    private GameMode _gameMode;

	// Use this for initialization
	void Start () {
        _gameMode = (GameMode)System.Enum.Parse(typeof(GameMode), PlayerPrefs.GetString("GameMode", "CoOp"));

        switch (_gameMode)
        {
            case GameMode.CoOp:
                {
                    SpawnCars(1, PlayerPrefs.GetInt("AmountOfBots", 0));
                }
                break;
            case GameMode.Teams:
                {

                }break;
        }
		//set respawn points
        //enable/disable gameobjects
	}

    void SpawnPlayerCars(int amountOfPlayerCars)
    {
        for (int i = 0; i < amountOfPlayerCars; i++)
        {
            _playerCars.GetChild(i).position = _startPositions.GetChild(i).position;
            _playerCars.GetChild(i).gameObject.SetActive(true);
        }
    }

    void SpawnBotCars(int amountOfPlayerCars, int amountOfBots)
    {
        for (int i = amountOfPlayerCars; i < amountOfBots; i++)
        {
            _botCars.GetChild(i-amountOfPlayerCars).position = _startPositions.GetChild(i).position;
            _botCars.GetChild(i-amountOfPlayerCars).gameObject.SetActive(true);
        }
    }

    void SpawnCars(int amountOfPlayerCars, int amountOfBots)
    {
        SpawnPlayerCars(amountOfPlayerCars);
        SpawnBotCars(amountOfPlayerCars,amountOfBots);
    }

    //void SpawnCars()
    //{
    //    int arrayIndex=0;
    //    for (int i = 0; i < _startPositions.Length; i++)
    //    {
    //        for (int j = 0; j < length; j++)
    //        {

    //        }
    //    }
    //}
	
	// Update is called once per frame
	void Update () {
		
	}
}
