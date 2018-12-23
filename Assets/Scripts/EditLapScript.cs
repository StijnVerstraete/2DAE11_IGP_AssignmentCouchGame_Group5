using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditLapScript : MonoBehaviour {

    
    public Text LapsText;
    public Text FinishText;
    public Text GoToMenuText;
    public GameObject SetUpdateLap;
    public GameObject Countdown;
    public GameObject PanelSplit;

    private int _currentLap;
    private bool _isSetUpdateLapTrue = false;
    [SerializeField]
    private int _endLap = 3;

    [SerializeField]
    private string _carName;

    private bool _isFinished = false;

    //lapstimer
    public Text LapTimerText;

    private float _timer;
    private float _lapsTimer;
    //private int _lap = 0;
    private bool _stopTimer = false;


    private string[] _timeLapsArray = new string[3];
    private int _timeLapsIndex = 0;

    private List<string> _carsList = new List<string>();

    private GameMode _gameMode;
   

    // Use this for initialization
    void Start () {
        
        _currentLap = int.Parse(LapsText.text);
        FinishText.text = "";
        GoToMenuText.enabled = false;

        _gameMode = (GameMode)System.Enum.Parse(typeof(GameMode), PlayerPrefs.GetString("GameMode", "Team"));
        
        if (_gameMode.Equals(GameMode.Teams))
        {
            PanelSplit.SetActive(true);
        }
        else
        {
            PanelSplit.SetActive(false);
        }

        
    }
	
	// Update is called once per frame
	void Update () {
        _isSetUpdateLapTrue = SetUpdateLap.GetComponent<SetLapTrue>().IsSetLapTrue;

        if (!Countdown.GetComponent<CountDown>().ShowCountDown && !_stopTimer) // start the timer if GO is gone
        {
            
            TotalLapsTimer();

            _lapsTimer += Time.deltaTime;
        }

        if (_isFinished && Input.GetButtonDown("Start_XboxButton"))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRAADASFASDSASADFA");
        Debug.Log(other.name);
        Debug.Log(_carName);
        Debug.Log("TRAADASFASDSASADFA");

        string finishedText = "";
        if (other.name.Contains(_carName) && _isSetUpdateLapTrue)
        {
            _currentLap++;

            _timeLapsArray[_timeLapsIndex] = LapsTimer();

            _timeLapsIndex++;

            _lapsTimer = 0;

            if (_currentLap > _endLap) //you are finished
            {
                _stopTimer = true;
                //Debug.Log("Show finish screen ");
                finishedText = "You Have Finished !!!";
                //FinishText.text = "You Have Finished !!!";
                /*
                 * todo
                 *      show laps times
                 *      total time
                 *      position place car 
                 */
                
                for (int i = 0; i < _timeLapsArray.Length; i++)
                {
                    finishedText += "\nlap " + (i + 1) + ": " + _timeLapsArray[i];
                    //FinishText.text += "/nlap " + (i++) + ": " + _timeLapsArray[i];
                    //Debug.Log(_timeLapsArray[i]);
                }

                finishedText += "\nTotal Time: " + LapTimerText.text;
                //FinishText.text += "/nTotal Time: " + LapTimerText.text;
                //Debug.Log("total: " + LapTimerText.text);

                GoToMenuText.enabled = true;
                _isFinished = true;
        
            }
            else
            {
                LapsText.text = _currentLap.ToString();
            }

            // Controls at which position the car is
            //code can be better;
            if (_currentLap > _endLap)
            {
                _carsList.Add(other.name);

                for (int i = 0; i < _carsList.Count; i++)
                {
                    if (_carsList[i].Contains(_carName))
                    {
                        finishedText += "\n You have finished at position " + (i+1);
                        //Debug.Log("found player car at position" + (i+1));
                    }
                }

                FinishText.text = finishedText;
            }
            
        }
        
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains(_carName))
        {
            _isSetUpdateLapTrue = false;
            SetUpdateLap.GetComponent<SetLapTrue>().IsSetLapTrue = false;
            Debug.Log("trigger exit");
        }
    }

    private void TotalLapsTimer()
    {
        _timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(_timer / 60F);
        int seconds = Mathf.FloorToInt(_timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        LapTimerText.text = niceTime;
    }

    private string LapsTimer()
    {
        int minutes = Mathf.FloorToInt(_lapsTimer / 60F);
        int seconds = Mathf.FloorToInt(_lapsTimer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        return niceTime;
    }
}
