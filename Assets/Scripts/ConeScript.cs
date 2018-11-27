using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeScript : MonoBehaviour {

    private Rigidbody _rb;
    [SerializeField] private GameObject _car;
	void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        //_car = GameObject.FindGameObjectWithTag("Car");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        _rb.AddForce(Vector3.Scale(_car.transform.forward, new Vector3(0, 30, 0)));
    }
}
