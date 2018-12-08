﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Phase
{
    StartScreen, PlayerSelectionScreen, LevelSelectionScreen
}

[System.Serializable]
public class ScreenPhase
{
    public Phase _phase;
    public GameObject _screen;
}

public class MainMenuScript : MonoBehaviour {

    [Header("Global Variables")]
    [SerializeField] private List<ScreenPhase> _screens;
    private Phase _currentPhase;
    
    [Header("Player Selection Screen Variables")]
    private int[] _players = new int[] { 0, 0, 0, 0 };
    private bool[] _hasControllerJoined = new bool[] { false, false, false, false };

    [SerializeField] private GameObject[] _hasJoinedPanels;
    //[SerializeField] private GameObject[] _hasNotJoinedPanels;

    [SerializeField] private int _maxPlayers=4;
    [SerializeField] private int _minimumConnectedPlayers;
    private int _connectedPlayers = 0;
    [SerializeField] private Button _goButton;

    private GameMode _currentGameMode= GameMode.CoOp;
    [SerializeField] private Text _gameModeButtonText;

    [SerializeField] int _maxAmountOfBots=4;
    private int _amountOfBots=0;
    [SerializeField] private Text _amountOfBotsText;

    [Header("Cursor Variables")]
    [SerializeField] private Canvas _canvas;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    [SerializeField] private RectTransform[] _cursors;
    [SerializeField] private RectTransform _levelCursor;

    [SerializeField] private float _cursorSpeed;


    // Use this for initialization
    void Start () {
        PlayerPrefs.SetInt("MaxPlayers", _maxPlayers);

        //player selection screen
        for (int i = 0; i < _hasJoinedPanels.Length; i++)
            _hasJoinedPanels[i].SetActive(false);

        _goButton.interactable = false;
        _amountOfBotsText.text = "Bots: " + _amountOfBots;
        _gameModeButtonText.text= _currentGameMode.ToString();


        //set phase
        _currentPhase = Phase.StartScreen;
        ChangePhase(_currentPhase);


        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        switch (_currentPhase)
        {
            case Phase.StartScreen:
                {
                    for (int i = 1; i <= _players.Length; i++)
                    {
                        if(Input.GetButtonDown("A" + i + "_XboxButton"))
                        {
                            ChangePhase(Phase.PlayerSelectionScreen);
                            break;
                        }
                    }
                }break;
            case Phase.PlayerSelectionScreen:
                {
                    CheckIfPlayerJoinsOrLeaves();

                    //for each player, if there is a console connected
                    for (int i = 0; i < _players.Length; i++)
                    {
                        if (_players[i] != 0)
                        {
                            //move cursor
                            float xAxisInput = Input.GetAxis("J" + _players[i] + "_XboxHorizontal");
                            float yAxisInput = Input.GetAxis("J" + _players[i] + "_XboxVertical");
                            MoveCursor(_cursors[i], xAxisInput, yAxisInput);


                            if (Input.GetButtonDown("A" + _players[i] + "_XboxButton"))
                            {
                                PressCursor(_cursors[i]);
                            }                              
                        }

                    }

                    if (_connectedPlayers >= _minimumConnectedPlayers)
                    {
                        _goButton.interactable = true;
                    }
                    else
                    {
                        _goButton.interactable = false;
                    }

                    //end of PlayerSelectionScreenPhase
                }
                break;
            case Phase.LevelSelectionScreen:
                {
                    //move cursor
                    float xAxisInput = Input.GetAxis("J_XboxHorizontal");
                    float yAxisInput = Input.GetAxis("J_XboxVertical");
                    MoveCursor(_levelCursor, xAxisInput, yAxisInput);

                    //check if a player presses the cursor
                    for (int i = 0; i < _players.Length; i++)
                    {
                        if (_players[i] != 0)
                        {
                            if (Input.GetButtonDown("A" + _players[i] + "_XboxButton"))
                            {
                                PressCursor(_levelCursor);
                            }
                        }

                    }
                }
                break;
        }
	}

    void PressCursor(RectTransform cursor)
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the cursor position
        m_PointerEventData.position = cursor.anchoredPosition;

        Debug.Log(cursor.anchoredPosition);

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
        Debug.Log(results.Count);

        //For do specific action every result returned
        foreach (RaycastResult result in results)
        {
            //Debug.Log("Hit " + result.gameObject.name);

            //in case of button: click button
            if (result.gameObject.GetComponent<Button>())
            {
                Button but = result.gameObject.GetComponent<Button>();
                if (but.interactable)
                {
                    but.onClick.Invoke();
                    break;
                }

            }
        }
    }

    void MoveCursor(RectTransform cursor, float xAxisInput, float yAxisInput)
    {
        Vector2 temp = cursor.anchoredPosition + new Vector2(xAxisInput, yAxisInput) * Time.deltaTime * _cursorSpeed;
        cursor.anchoredPosition = new Vector2(Mathf.Clamp(temp.x, 0,_canvas.pixelRect.width-cursor.rect.width / 2), Mathf.Clamp(temp.y, cursor.rect.height / 2, _canvas.pixelRect.height)) ;
    }

    void ChangePhase(Phase target)
    {
        //hide all panels except target panel
        foreach (ScreenPhase phase in _screens)
        {
            phase._screen.SetActive(false);
            if(phase._phase==target)
                phase._screen.SetActive(true);
        }

        _currentPhase = target;
    }

    public void GotBackToStartScreen()
    {
        //undo all changes made on selectionscreen
        for (int i = 0; i < _players.Length; i++)
        {
            _cursors[i].gameObject.SetActive(false);
            _players[i] = 0;
            _hasJoinedPanels[i].SetActive(false);
            _hasControllerJoined[i] = false;
            _connectedPlayers=0;
        }
        ChangePhase(Phase.StartScreen);
    }

    public void GotToLevelSelector()
    {
        ChangePhase(Phase.LevelSelectionScreen);
        _levelCursor.anchoredPosition = _canvas.pixelRect.center;
    }

    public void GoBackToPlayerSelectionScreen()
    {
        ChangePhase(Phase.PlayerSelectionScreen);

        //set cursors back to starting positions
        for (int i = 0; i < _cursors.Length; i++)
        {
            _cursors[i].position = _hasJoinedPanels[i].transform.position;
        }
    }

    public void PlayLevel(string levelName)
    {
        //save players to prefs
        int _amountOfPLayers=0;
        for (int i = 0; i < _players.Length; i++)
        {
            PlayerPrefs.SetInt("Player" + i + "Console", _players[i]);
            if (_players[i] != 0)
                _amountOfPLayers++;
        }
        PlayerPrefs.SetInt("AmountOfPlayers", _amountOfPLayers);
        PlayerPrefs.SetInt("AmountOfBots", _amountOfBots);

        PlayerPrefs.SetString("GameMode",_currentGameMode.ToString());
        SceneManager.LoadScene(levelName);
    }
    
    public void ChangeGameMode()
    {
        if (_currentGameMode == GameMode.CoOp)
            _currentGameMode = GameMode.Teams;
        else _currentGameMode = GameMode.CoOp;

        _gameModeButtonText.text = _currentGameMode.ToString();
    }

    void CheckIfPlayerJoinsOrLeaves()
    {
        for (int i = 1; i <= _players.Length; i++)
        {

            if (Input.GetButtonDown("A" + i + "_XboxButton"))
            {
                if (!_hasControllerJoined[i])
                {
                    for (int j = 0; j < _players.Length; j++)
                    {
                        if (_players[j] == 0)
                        {
                            _players[j] = i;
                            _hasJoinedPanels[j].SetActive(true);
                            //PlayerPrefs.SetInt("Player"+ j+"Controller",i);

                            _cursors[j].gameObject.SetActive(true);
                            _cursors[j].position = _hasJoinedPanels[j].transform.position;
                            _hasControllerJoined[i] = true;
                            _connectedPlayers++;
                            break;
                        }
                    }
                }
            }

            if (Input.GetButtonDown("B" + i + "_XboxButton"))
            {
                if (_hasControllerJoined[i])
                {
                    for (int j = 0; j < _players.Length; j++)
                    {
                        if (_players[j] == i)
                        {
                            _players[j] = 0;
                            _hasJoinedPanels[j].SetActive(false);
                            _cursors[j].gameObject.SetActive(false);
                            _hasControllerJoined[i] = false;
                            _connectedPlayers--;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void AddBot()
    {
        if (_amountOfBots < _maxAmountOfBots)
            _amountOfBots++;
        _amountOfBotsText.text = "Bots: " + _amountOfBots;
    }
    public void RemoveBot()
    {
        if (_amountOfBots > 0)
            _amountOfBots--;
        _amountOfBotsText.text = "Bots: " + _amountOfBots;
    }
}
