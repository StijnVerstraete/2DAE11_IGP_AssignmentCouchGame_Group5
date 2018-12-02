using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour {

    [SerializeField] CarControls _carControls;
    [SerializeField] AudioSource _audioSource;

    [SerializeField] AudioClip[] _carClips; //0 = idle 1 = accelrate 2 = brake
    private int _clipPlaying;
	void Start ()
    {
    }
	
	// Update is called once per frame
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
            }
            if (_carControls.AccelerationLevel > 0)
            {
                _audioSource.clip = _carClips[1];
                _clipPlaying = _carControls.AccelerationLevel;
                _audioSource.volume = 0.5f;
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
                if (_carControls.AccelerationLevel == -2)
                {
                    _audioSource.volume = 0.8f;
                }
            }
            _audioSource.Play();
        }

    }
}
