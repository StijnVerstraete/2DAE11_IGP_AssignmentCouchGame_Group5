using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    [SerializeField]
    private Image _startImage;

    private List<int> _assignedControllers = new List<int>();

    private PlayerPanel[] _panels;


    private float _timer = 0;
    private bool _showStartImage = false;

	// Use this for initialization
	void Start () {
        _startImage.enabled = false;
        PlayerPrefs.SetInt("AmountOfPlayers", 0);

        _panels = FindObjectsOfType<PlayerPanel>().OrderBy(t=> t.PlayerNumber).ToArray();  
    }
	
	// Update is called once per frame
	void Update () {
        JoinTheGame();
        ShowStartImageAfter5Seconds();
	}

    private void JoinTheGame()
    {
        // get the length of the controller connected to this pc
        for (int i = 1; i < Input.GetJoystickNames().Length + 1; i++)
        {
            if (_assignedControllers.Contains(i))
                continue;

            //control which controller is used and if they click on the A button
            if (Input.GetButtonDown("A" + i + "_XboxButton"))
            {
                AddPlayerController(i);
                //Debug.Log("A" + i + "_XboxButton");
                break;
            }
        }
    }

    private void ShowStartImageAfter5Seconds()
    {
        if (_assignedControllers.Count() >= 1)
        {
            _timer += Time.deltaTime;

            if (_timer >= 5.0f)
            {
                Debug.Log("over 5 sec show start button");
                _timer = 0;
                _showStartImage = true;
            }
        }

        if (_showStartImage)
        {
            _startImage.enabled = true;

            if (Input.GetButtonDown("Start_XboxButton"))
            {
                SceneManager.LoadScene("Level1");
                Debug.Log("Go to level 01");
            }
        }
    }

    private void AddPlayerController(int controller)
    {
        //add the controller to the list
        _assignedControllers.Add(controller);
        PlayerPrefs.SetInt("AmountOfPlayers", PlayerPrefs.GetInt("AmountOfPlayers", 0) + 1);

        //for (int i = 0; i < _panels.Length; i++)
        //{
        //    if (!_panels[i].HasControllerAssigned)
        //    {
        //        _panels[i].UpdatePanels(controller);
        //    }
        //}
        
        //update the panels when a controller is joined
        _panels[controller - 1].UpdatePanels(controller);
    }
}
