using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour {

    [SerializeField] CarControls _carControls;
    [SerializeField] AudioSource _audioSource;

    [SerializeField] AudioClip[] _carClips; //0 = idle 1 = accelrate 2 = brake
    private int _clipPlaying;

    private int _stereoChannel = 0;

    [SerializeField] private GameMode _gameMode;

    [SerializeField] private int _sourceID;

    private void Start()
    {
        _gameMode = (GameMode)System.Enum.Parse(typeof(GameMode), PlayerPrefs.GetString("GameMode", "Teams"));
        _carControls = GetComponentInParent<CarControls>();
        if (_gameMode == GameMode.Teams)
        {
            if (_sourceID == 0)
            {
                _stereoChannel = -1;
            }
            if (_sourceID == 1)
            {
                _stereoChannel = 1;
            }
        }
        else
        {
            _stereoChannel = 0;
        }
    }
    void Update ()
    {
        CarAudio();

    }
    private void CarAudio()
    {
        if (_clipPlaying != _carControls.AccelerationLevel)
        {
            if (_carControls.AccelerationLevel == 0)
            {
                _audioSource.clip = _carClips[0];
                _clipPlaying = _carControls.AccelerationLevel;
                _audioSource.volume = 0.5f;
                _audioSource.panStereo = _stereoChannel;
            }
            if (_carControls.AccelerationLevel > 0)
            {
                _audioSource.clip = _carClips[1];
                _clipPlaying = _carControls.AccelerationLevel;
                _audioSource.volume = 0.5f;
                _audioSource.panStereo = _stereoChannel;
                if (_carControls.AccelerationLevel == 2)
                {
                    _audioSource.volume = 0.8f;
                }
            }
            if (_carControls.AccelerationLevel < 0)
            {
                _audioSource.clip = _carClips[2];
                _clipPlaying = _carControls.AccelerationLevel;
                _audioSource.volume = 0.5f;
                _audioSource.panStereo = _stereoChannel;
                if (_carControls.AccelerationLevel == -2)
                {
                    _audioSource.volume = 0.8f;
                }
            }
            _audioSource.Play();
        }

    }
}
