using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Dateils")]
    [SerializeField]
    private Vector3 _startPosition = new Vector3(0, -3, 0);
    [SerializeField]
    private float _playerSpeed = 2.5f;
    [SerializeField]
    Vector3 _playerDirection = new Vector3();
    [SerializeField]
    private Vector3 _bindPosition = new Vector3();

    [Header("Player Binds")]
    [SerializeField]
    private float _leftPlayerBind;
    [SerializeField]
    private float _rightPlayerBind;
    [SerializeField]
    private float _upperPlayerBind;
    [SerializeField]
    private float _lowerPlayerBind;

    void Start()
    {
        // assign starting position
        transform.position = _startPosition;
    }

    void Update()
    {
        // get inputs
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");

        // apply inputs to Vector3
        _playerDirection.x = hMove;
        _playerDirection.y = vMove;

        // move player
        transform.Translate(_playerDirection * _playerSpeed * Time.deltaTime);

        // check player binds and restrain
        _bindPosition = transform.position;

        if (transform.position.x <= _leftPlayerBind)
        {
            _bindPosition.x = _leftPlayerBind;
        }
        else if (transform.position.x >= _rightPlayerBind)
        {
            _bindPosition.x = _rightPlayerBind;
        }

        if (transform.position.y >= _upperPlayerBind)
        {
            _bindPosition.y = _upperPlayerBind;
        }
        else if (transform.position.y <= _lowerPlayerBind)
        {
            _bindPosition.y = _lowerPlayerBind;
        }

        // apply binding restrictions
        transform.position = _bindPosition;
    }
}
