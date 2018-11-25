using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour {

    [SerializeField]
    private Image _imgButton;
    [SerializeField]
    private Text _txtJoin;
    [SerializeField]
    private Text _txtAssigned;

    public int PlayerNumber;

    public bool HasControllerAssigned { get; set; }
    

    public void UpdatePanels(int controller)
    {
        _imgButton.enabled = false;
        _txtJoin.enabled = false;
        _txtAssigned.enabled = true;
        _txtAssigned.text = "Controller " + controller + " has joined";

        HasControllerAssigned = true;
    }
}
