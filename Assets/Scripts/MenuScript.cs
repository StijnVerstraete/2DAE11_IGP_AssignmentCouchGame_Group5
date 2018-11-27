using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    [SerializeField]
    private List<int> _assignedControllers = new List<int>();

    private PlayerPanel[] _panels;
    

	// Use this for initialization
	void Start () {
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
	}

    private void AddPlayerController(int controller)
    {
        _assignedControllers.Add(controller);

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

    }
}
