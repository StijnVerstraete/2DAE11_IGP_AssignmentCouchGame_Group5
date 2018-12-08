using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarElementsAnimation : MonoBehaviour {

    [SerializeField] private GameObject _steer;
    [SerializeField] private GameObject _gasPedal;
    [SerializeField] private GameObject _brakePedal;

    [SerializeField] private CarControls _carControls;

    private int[] _amountGasArray = new int [4];
    private int[] _amountBrakeArray = new int[4];

    [SerializeField] private int _gasLevel;
    [SerializeField] private int _brakeLevel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //amountgas
        for (int i = 0; i < _carControls.PlayerActionsAcceleration.Length; i++)
        {
            if(_carControls.PlayerActionsAcceleration[i] >=0)
            {
                _amountGasArray[i] = _carControls.PlayerActionsAcceleration[i];
            }
            else
            {
                _amountGasArray[i] = 0;
            }
        }
        _gasLevel = _amountGasArray[0] + _amountGasArray[1] + _amountGasArray[2] + _amountGasArray[3];
        //amountbrake
        for (int i = 0; i < _carControls.PlayerActionsAcceleration.Length; i++)
        {
            if (_carControls.PlayerActionsAcceleration[i] <= 0)
            {
                _amountBrakeArray[i] = _carControls.PlayerActionsAcceleration[i];
            }
            else
            {
                _amountBrakeArray[i] = 0;
            }
        }
        _brakeLevel = _amountBrakeArray[0] + _amountBrakeArray[1] + _amountBrakeArray[2] + _amountBrakeArray[3];

        _steer.transform.localEulerAngles = new Vector3(_steer.transform.localEulerAngles.x, _steer.transform.localEulerAngles.y, 90 + (_carControls.SteeringLevel * 45));
        _gasPedal.transform.localEulerAngles = new Vector3(-17 + (10 * -_gasLevel), _gasPedal.transform.localEulerAngles.y, _gasPedal.transform.localEulerAngles.z);
        _brakePedal.transform.localEulerAngles = new Vector3(-17 + (10 * _brakeLevel), _brakePedal.transform.localEulerAngles.y, _brakePedal.transform.localEulerAngles.z);
    }
}
 