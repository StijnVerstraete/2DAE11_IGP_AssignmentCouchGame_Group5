using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessage : MonoBehaviour {

    [SerializeField] private string _instructionText;

    [SerializeField] private Text _textObject;
    [SerializeField] private GameObject _panel;

    public bool ShowText = false;

    private void Update()
    {
        if (ShowText == true && (_textObject.gameObject.activeSelf == false || _panel.activeSelf == false))
        {
            _textObject.gameObject.SetActive(true);
            _panel.SetActive(true);

            _textObject.text = _instructionText;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerCar")
        {
            ShowText = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TutorialText")
        {
            _textObject.gameObject.SetActive(false);
            _textObject.gameObject.SetActive(false);
            ShowText = false;
        }
    }

}
