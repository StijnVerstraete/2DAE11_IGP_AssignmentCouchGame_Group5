using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    StartScreen, SelectionScreen, LevelSelectionScreen
}

[System.Serializable]
public class ScreenPhase
{
    public Phase _phase;
    public GameObject _screen;
}

public class MainMenuScript : MonoBehaviour {

    private Transform _transform;

    [SerializeField] private List<ScreenPhase> _screens;

    private Phase _currentPhase;

    // Use this for initialization
    void Start () {
        _currentPhase = Phase.StartScreen;
        _transform = GetComponent<Transform>();

        ChangePhase(_currentPhase);
	}
	
	// Update is called once per frame
	void Update () {
        switch (_currentPhase)
        {
            case Phase.StartScreen:
                {
                    for (int i = 1; i < 5; i++)
                    {
                        if(Input.GetButtonDown("A" + i + "_XboxButton"))
                        {
                            ChangePhase(Phase.SelectionScreen);
                            break;
                        }
                    }
                }break;
            case Phase.SelectionScreen:
                {

                }
                break;
            case Phase.LevelSelectionScreen:
                {

                }
                break;
        }
	}

    void ChangePhase(Phase target)
    {
        foreach (ScreenPhase phase in _screens)
        {
            phase._screen.SetActive(false);
            if(phase._phase==target)
                phase._screen.SetActive(true);
        }

        _currentPhase = target;
    }
}
