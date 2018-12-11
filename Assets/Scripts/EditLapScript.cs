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
    private int _lap = 0;
    private bool _stopTimer = false;


    // Use this for initialization
    void Start () {
        
        _currentLap = int.Parse(LapsText.text);
        FinishText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        _isSetUpdateLapTrue = SetUpdateLap.GetComponent<SetLapTrue>().IsSetLapTrue;

        if (!Countdown.GetComponent<CountDown>()._showCountdown && !_stopTimer) // start the timer if GO is gone
        {
            LapsTimer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isSetUpdateLapTrue)
        {
            _currentLap++;

            if (_currentLap > _endLap)
            {
                _stopTimer = true;
                //Debug.Log("Show finish screen ");
                FinishText.text = "You Have Finished !!!";
                /*
                 * todo
                 *      show laps times
                 *      total time
                 *      position place car 
                 */
        
            }
            else
            {
                LapsText.text = _currentLap.ToString();
            }

            

            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        _isSetUpdateLapTrue = false;
        SetUpdateLap.GetComponent<SetLapTrue>().IsSetLapTrue = false;
    }

    private void LapsTimer()
    {
        _timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(_timer / 60F);
        int seconds = Mathf.FloorToInt(_timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        LapTimerText.text = niceTime;
    }
}
