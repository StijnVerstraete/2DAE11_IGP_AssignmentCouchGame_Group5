using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    FreeForAll, Teams
}

public class GameModeHandler : MonoBehaviour {

    public Transform PlayerCars;
    public Transform OpponentCars;

	// Use this for initialization
	void Start () {
		//set respawn points
        //enable/disable gameobjects
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
