using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    public Transform RespawnPoint;

    public GameObject Car;
	// Use this for initialization
	void Start ()
    {
        Car.transform.position = RespawnPoint.position;
        Car.transform.rotation = new Quaternion(RespawnPoint.rotation.x, RespawnPoint.rotation.y, RespawnPoint.rotation.z, RespawnPoint.rotation.w);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("B1_Axis"))
        {
            Car.transform.position = RespawnPoint.position;
            Car.transform.rotation = new Quaternion(RespawnPoint.rotation.x, RespawnPoint.rotation.y, RespawnPoint.rotation.z,RespawnPoint.rotation.w);
            Car.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Debug.Log("Respawn");
        }
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RespawnPoint")
        {
            RespawnPoint = other.transform;
        }
    }
}
