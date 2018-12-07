using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    CoOp, Teams
}

public class GameModeHandler : MonoBehaviour {

    public Transform PlayerCars;
    public Transform OpponentCars;

    private GameMode _gameMode;

	// Use this for initialization
	void Start () {
        _gameMode = (GameMode)System.Enum.Parse(typeof(GameMode), PlayerPrefs.GetString("GameMode", "CoOp"));

        switch (_gameMode)
        {
            case GameMode.CoOp:
                {

                }break;
            case GameMode.Teams:
                {

                }break;
        }
		//set respawn points
        //enable/disable gameobjects
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
