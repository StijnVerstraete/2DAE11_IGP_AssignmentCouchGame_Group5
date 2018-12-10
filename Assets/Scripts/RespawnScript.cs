using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour {

    public Transform RespawnPoint;
    [SerializeField] private GameObject[] _wheels;
    [SerializeField] private GameObject _respawnCollisionChecker;
    [SerializeField] private bool _isOpponent;

    private GameObject _respawnCollision;

    public void Respawn()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        transform.position = RespawnPoint.position;
        transform.rotation = RespawnPoint.localRotation;

        ChangeLayer(14);
        if (_respawnCollision != null)
            GameObject.Destroy(_respawnCollision);
        _respawnCollision = GameObject.Instantiate(_respawnCollisionChecker,transform,false);
        _respawnCollision.transform.rotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RespawnPoint")
        {
            //if (_isOpponent && RespawnPoint == other.transform)
            //    Respawn();

            RespawnPoint = other.transform;
        }
    }

    public void ChangeLayer(int layer)
    {
        gameObject.layer = layer;
        for (int i = 0; i < _wheels.Length; i++)
        {
            _wheels[i].layer = layer;
        }
    }
}
