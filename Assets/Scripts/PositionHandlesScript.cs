using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PositionX
{
    Left, Right
}
enum PositionY
{
    Top, Bottom
}

class PlayerPosition
{
    PositionX _positionX;
    PositionY _positionY;

    public PlayerPosition(PositionX xpos, PositionY ypos)
    {
        _positionX = xpos;
        _positionY = ypos;
    }
}

public class PositionHandlesScript : MonoBehaviour {
    [SerializeField]
    private Transform[] _objectPositions; //0 - steerLeft  1 - steerRight    2 - brake     3 - accelerate

    [SerializeField]
    private GameObject[] _players;

    [SerializeField]
    private float _switchSpeed;


    private int _actualPosition;
    private int _destinationPosition;

    //private List<PlayerPosition> _playersPositions;
    private PlayerPosition[] _playersPositions;

    [SerializeField]
    private bool _diagonalMovement = false;

    void Start ()
    {
        _playersPositions = new PlayerPosition[]{ new PlayerPosition(PositionX.Left, PositionY.Top), new PlayerPosition(PositionX.Right, PositionY.Top),
        new PlayerPosition(PositionX.Left, PositionY.Bottom),new PlayerPosition(PositionX.Right, PositionY.Bottom)};

        for(int i=0; i < _players.Length; i++)
        {

        }
	}
	
	// Update is called once per frame
	void Update ()
    {

        //check what the current position is
        for (int i = 0; i < _objectPositions.Length; i++)
        {
            if (transform.position == _objectPositions[i].transform.position)
            {
                _actualPosition = i;
            }
        }
        if (_actualPosition == _destinationPosition)
        {
            SetDestination();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _objectPositions[_destinationPosition].transform.position, _switchSpeed * Time.deltaTime);
        }
        
    }
    private void SetDestination()
    {
        #region tryout code lucas
        //TEMP CODE
        float angle;
        //angle = Mathf.Rad2Deg * Mathf.Atan(Input.GetAxis("Vertical") / Input.GetAxis("Horizontal"));
        //angle = Vector2.Angle(Vector2.right, new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) * (Input.GetAxis("Vertical") > 0 ? 1 : -1);
        if (Input.GetAxis("Vertical")>=0)
        angle = Vector2.Angle(Vector2.right, new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        else
            angle = Vector2.Angle(Vector2.left, new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) +180;
        Debug.Log(angle);
        #endregion)
    }

}
