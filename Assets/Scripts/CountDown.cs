using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    public bool ShowCountDown { get; set; }
    // countDown
    public Text CountdownText;

    private string _countDownText;
    

    // Use this for initialization
    void Start () {
        ShowCountDown = true;
        StartCoroutine("StartCountdown");
    }

    void Update()
    {
        CountdownText.text = _countDownText;
    }

    IEnumerator StartCountdown()
    {
        _countDownText = "3";
        yield return new WaitForSeconds(1);

        _countDownText = "2";
        yield return new WaitForSeconds(1);

        _countDownText = "1";
        yield return new WaitForSeconds(1);

        _countDownText = "GO";
        yield return new WaitForSeconds(1);

        ShowCountDown = false;
        _countDownText = "";
        CountdownText.enabled = false;
    }
}
