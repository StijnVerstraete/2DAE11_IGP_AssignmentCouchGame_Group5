using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {

    [SerializeField] private GameObject _pauseMenu;

    [SerializeField] private RectTransform _levelCursor;
    [SerializeField] private float _cursorSpeed;

    private Canvas _canvas;
    private RectTransform _canvasRect;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private List<int> _players = new List<int>();

    public bool IsPaused
    {
        get
        {
            return _pauseMenu.activeSelf;
        }
    }
    // Use this for initialization
    void Start () {
        _pauseMenu.SetActive(false);
        for (int i = 1; i <= PlayerPrefs.GetInt("MaxPlayers", 4); i++)
        {
            if (PlayerPrefs.GetInt("Player" + i + "Console", 0) != 0)
            {
                _players.Add(PlayerPrefs.GetInt("Player" + i + "Console"));
            }
        }
        _canvas = GetComponent<Canvas>();
        _canvasRect = GetComponent<RectTransform>();
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Start_XboxButton"))
        {
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);

            if (_pauseMenu.activeSelf)
            {
                _levelCursor.anchoredPosition = _canvas.pixelRect.center;
            }

        }

        if (!_pauseMenu.activeSelf) return;


        //move cursor
        float xAxisInput = Input.GetAxis("J_XboxHorizontal");
        float yAxisInput = Input.GetAxis("J_XboxVertical");
        MoveCursor(_levelCursor, xAxisInput, yAxisInput);

        //check if a player presses the cursor
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i] != 0)
            {
                if (Input.GetButtonDown("A" + _players[i] + "_XboxButton"))
                {
                    PressCursor(_levelCursor, i + 1);
                }
            }

        }
    }

    void MoveCursor(RectTransform cursor, float xAxisInput, float yAxisInput)
    {
        Vector2 temp = cursor.anchoredPosition + new Vector2(xAxisInput, yAxisInput) *Time.deltaTime* _cursorSpeed;
        //cursor.anchoredPosition = new Vector2(Mathf.Clamp(temp.x, 0,_canvas.pixelRect.width-cursor.rect.width / 2), Mathf.Clamp(temp.y, cursor.rect.height / 2, _canvas.pixelRect.height)) ;
        cursor.anchoredPosition = new Vector2(Mathf.Clamp(temp.x, 0, _canvasRect.rect.width - cursor.rect.width / 2), Mathf.Clamp(temp.y, cursor.rect.height / 2, _canvasRect.rect.height));
    }

    void PressCursor(RectTransform cursor, int player)
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
            Debug.Log("Hit " + result.gameObject.name);

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

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
