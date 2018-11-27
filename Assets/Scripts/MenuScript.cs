using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    private List<int> _assignedControllers = new List<int>();

    private PlayerPanel[] _panels;


    private float _timer = 0;
    private bool _showStartImage = false;

	// Use this for initialization
	void Start () {

        PlayerPrefs.SetInt("AmountOfPlayers", 0);

        _panels = FindObjectsOfType<PlayerPanel>().OrderBy(t=> t.PlayerNumber).ToArray();  
    }
	
	// Update is called once per frame
	void Update () {
        // get the length of the controller connected to this pc

        for (int i = 1; i < Input.GetJoystickNames().Length + 1; i++)
        {
            if (_assignedControllers.Contains(i))
                continue;

            if (Input.GetButtonDown("A" + i + "_XboxButton"))
            {
                AddPlayerController(i);
                Debug.Log("A" + i + "_XboxButton");
                break;
            }
        }

        if (_assignedControllers.Count() >= 1)
        {
            _timer += Time.deltaTime;

            if (_timer >= 10.0f)
            {
                Debug.Log("over 10 sec show start button");
                _timer = 0;
                _showStartImage = true;
            }
        }
	}

    private void AddPlayerController(int controller)
    {
        _assignedControllers.Add(controller);
        PlayerPrefs.SetInt("AmountOfPlayers", PlayerPrefs.GetInt("AmountOfPlayers", 0) + 1);

        //for (int i = 0; i < _panels.Length; i++)
        //{
        //    if (!_panels[i].HasControllerAssigned)
        //    {
        //        _panels[i].UpdatePanels(controller);
        //    }
        //}

        if (!_panels[controller - 1].HasControllerAssigned)
        {
            _panels[controller - 1].UpdatePanels(controller);
        }
        _timer = 0;
    }
}
