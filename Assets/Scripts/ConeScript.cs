using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeScript : MonoBehaviour {

    private Rigidbody _rb;

	void Start ()
    {
        _rb = GetComponent<Rigidbody>();
	}
	
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer== LayerMask.NameToLayer("Car"))
        _rb.AddForce(Vector3.Scale(collision.transform.forward, new Vector3(0, 30, 0)));
    }
}
