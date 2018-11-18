using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxControllers : MonoBehaviour {

    [SerializeField]
    private string _horizontalInputPlayer;
    [SerializeField]
    private string _verticalInputPlayer;
    [SerializeField]
    private string _aButtonInputPlayer;


    // Use this for initialization
    void Start () {
        Debug.Log(_horizontalInputPlayer);
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis(_horizontalInputPlayer);
        float v = Input.GetAxis(_verticalInputPlayer);

        this.transform.Translate(new Vector3(h,0,v) * Time.deltaTime);
        

        if (Input.GetButtonDown(_aButtonInputPlayer))
        {
            Debug.Log("A button is down");
        }
    }
}
