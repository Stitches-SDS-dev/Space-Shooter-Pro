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

    [Header("Additional Settings")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _yLaserOffset;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private int _playerLives = 3;


    private Vector3 _laserOffset = new Vector3();
    private float _canFire = 0f;

    void Start()
    {
        // assign starting position
        transform.position = _startPosition;
    }

    void Update()
    {
        PlayerMovement();

        // check for input and rate of fire delay
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        _laserOffset = transform.position;
        _laserOffset.y += _yLaserOffset;
        Instantiate(_laserPrefab, _laserOffset, Quaternion.identity);
    }

    private void PlayerMovement()
    {
        // get inputs
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");

        // apply inputs to Vector3
        _playerDirection.x = hMove;
        _playerDirection.y = vMove;

        // move player
        transform.Translate(_playerDirection * _playerSpeed * Time.deltaTime);

        // check player binds and set restraints
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
    
    public void DamagePlayer()
    {
        _playerLives--;

        if (_playerLives <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
