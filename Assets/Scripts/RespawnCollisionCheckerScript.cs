using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollisionCheckerScript : MonoBehaviour {

    private float _invincibilitytimer = 0;
    [SerializeField] float _invincibilityTime = 2;

    private bool _isColliding;
	
	// Update is called once per frame
	void Update () {
        if (!_isColliding)
        {
            _invincibilitytimer += Time.deltaTime;
            if (_invincibilitytimer > _invincibilityTime)
            {
                transform.parent.GetComponent<RespawnScript>().ChangeLayer(9);
                GameObject.Destroy(gameObject);
            }

        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==9)
        _isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.GetComponent<RespawnScript>().ChangeLayer(9);
        GameObject.Destroy(gameObject);
    }
}
