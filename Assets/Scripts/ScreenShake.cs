using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]  private Transform _camTransform;

    [SerializeField] private float _shakeDuration;

    private float _shakeAmount = 0.3f;
    private float _decreaseFactor = 1.0f;

    private Vector3 _originalPos;

    void OnEnable()
    {
        _originalPos = _camTransform.localPosition;
    }

    void Update()
    {
        if (_shakeDuration > 0)
        {
            _camTransform.localPosition = _originalPos + Random.insideUnitSphere * _shakeAmount;

            _shakeDuration -= Time.deltaTime * _decreaseFactor;
        }
        else
        {
            _shakeDuration = 0f;
            _camTransform.localPosition = _originalPos;
        }
    }
}
