using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour {

    public Transform RespawnPoint;

    [SerializeField] private bool _isOpponent;

	// Use this for initialization
	void Start ()
    {
        //transform.position = RespawnPoint.position;
        //transform.rotation = RespawnPoint.localRotation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("B1_Axis") && !_isOpponent)
        {
            Respawn();
        }
	}

    public void Respawn()
    {
        transform.position = RespawnPoint.position;
        transform.rotation = RespawnPoint.localRotation;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Debug.Log("Respawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RespawnPoint")
        {
            if (_isOpponent && RespawnPoint == other.transform)
                Respawn();

            RespawnPoint = other.transform;
        }
    }
}
