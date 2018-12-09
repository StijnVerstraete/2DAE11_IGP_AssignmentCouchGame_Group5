using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour {

    public Transform RespawnPoint;
    [SerializeField] private GameObject _respawnCollisionChecker;
    [SerializeField] private bool _isOpponent;

    private GameObject _respawnCollision;

    public void Respawn()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        transform.position = RespawnPoint.position;
        transform.rotation = RespawnPoint.localRotation;

        gameObject.layer = 14;
        _respawnCollision = GameObject.Instantiate(_respawnCollisionChecker,transform,false);
        _respawnCollision.transform.rotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RespawnPoint" && gameObject.tag!="RespawnCollision")
        {
            if (_isOpponent && RespawnPoint == other.transform)
                Respawn();

            RespawnPoint = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(gameObject.tag == "RespawnCollision")
        {
            //object nog verwijderen als er gerespawned wordt
        }
    }
}
