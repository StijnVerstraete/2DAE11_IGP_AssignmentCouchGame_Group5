using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditLapScript : MonoBehaviour {

    
    public Text LapsText;
    public Text FinishText;
    public GameObject SetUpdateLap;
    public GameObject Countdown;

    private int _currentLap;
    private bool _isSetUpdateLapTrue = false;
    private int _endLap = 3;

    //lapstimer
    public Text LapTimerText;

    private float _timer;
    private float _lapsTimer;
    //private int _lap = 0;
    private bool _stopTimer = false;


    private string[] _timeLapsArray = new string[3];
    private int _timeLapsIndex = 0;

    private List<string> _carsList = new List<string>();

    // Use this for initialization
    void Start () {
        
        _currentLap = int.Parse(LapsText.text);
        FinishText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        _isSetUpdateLapTrue = SetUpdateLap.GetComponent<SetLapTrue>().IsSetLapTrue;

        if (!Countdown.GetComponent<CountDown>().ShowCountDown && !_stopTimer) // start the timer if GO is gone
        {
            
            TotalLapsTimer();

            _lapsTimer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string finishedText = "";
        if (other.tag == "PlayerCar" && _isSetUpdateLapTrue)
        {
            _currentLap++;

            _timeLapsArray[_timeLapsIndex] = LapsTimer();

            _timeLapsIndex++;

            _lapsTimer = 0;

            if (_currentLap > _endLap)
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
                Debug.Log(_timeLapsArray.Length);
                for (int i = 0; i < _timeLapsArray.Length; i++)
                {
                    finishedText += "\nlap " + (i + 1) + ": " + _timeLapsArray[i];
                    //FinishText.text += "/nlap " + (i++) + ": " + _timeLapsArray[i];
                    //Debug.Log(_timeLapsArray[i]);
                }

                finishedText += "\nTotal Time: " + LapTimerText.text;
                //FinishText.text += "/nTotal Time: " + LapTimerText.text;
                //Debug.Log("total: " + LapTimerText.text);
                
                
        
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
                    if (_carsList[i].Contains("PREF_Car"))
                    {
                        finishedText += "\n You have finished at position " + (i+1);
                        //Debug.Log("found player car at position" + (i+1));
                    }

                    if (_carsList[i].Contains("PREF_Car (1)")) //when you play 1 vs 1
                    {
                        finishedText += "\n You have finished at position " + (i + 1);
                       // Debug.Log("found player second car at position" + (i + 1));
                    }
                }

                FinishText.text = finishedText;
            }


        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        _isSetUpdateLapTrue = false;
        SetUpdateLap.GetComponent<SetLapTrue>().IsSetLapTrue = false;
        Debug.Log("trigger exit");
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
