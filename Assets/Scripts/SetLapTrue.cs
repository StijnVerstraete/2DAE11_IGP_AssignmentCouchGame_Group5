using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLapTrue : MonoBehaviour {

    public bool IsSetLapTrue { get; set; }
    //public bool IsSetLapTrue = false;

    private void Start()
    {
        IsSetLapTrue = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IsSetLapTrue = true;
    }
}
