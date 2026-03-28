using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Dateils")]
    [SerializeField]
    private int _playerLives = 3;
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
    private bool _shouldWrap = false;
    [SerializeField]
    private float _playerWrap, _xPlayerBind, _upperPlayerBind, _lowerPlayerBind;

    [Header("Laser Settings")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private Transform _laserParent;
    [SerializeField]
    private float _yLaserOffset;
    [SerializeField]
    private float _fireRate = 0.5f;

    private SpawnManager _spawnManager;
    private Vector3 _laserOffset = new Vector3();
    private float _canFire = 0f;

    void Start()
    {
        // assign starting position
        transform.position = _startPosition;

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found!");
        }
    }

    void Update()
    {
        PlayerMovement();

        // check for input and rate of fire delay
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && Time.time >= _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        // fire laser with an offset above the Player
        _canFire = Time.time + _fireRate;
        _laserOffset = transform.position;
        _laserOffset.y += _yLaserOffset;
        Instantiate(_laserPrefab, _laserOffset, Quaternion.identity, _laserParent);
    }

    private void PlayerMovement()
    {
        // apply inputs to Vector3
        _playerDirection.x = Input.GetAxis("Horizontal");
        _playerDirection.y = Input.GetAxis("Vertical");

        // move player
        transform.Translate(_playerDirection * _playerSpeed * Time.deltaTime);

        // check player binds and set restraints or wraps        
        _bindPosition = transform.position;

        _bindPosition.y = Mathf.Clamp(_bindPosition.y, _lowerPlayerBind, _upperPlayerBind);

        if (!_shouldWrap)
        {
            // clamp on the x position
            _bindPosition.x = Mathf.Clamp(_bindPosition.x, -_xPlayerBind, _xPlayerBind);
        }
        else
        {
            // or wrap to the opposite side
            if (transform.position.x <= -_playerWrap)
            {
                _bindPosition.x = _playerWrap;
            }
            if (transform.position.x >= _playerWrap)
            {
                _bindPosition.x = -_playerWrap;
            }
        }

        // apply binding restrictions
        transform.position = _bindPosition;
        
    }
    
    public void DamagePlayer()
    {
        _playerLives--;

        if (_playerLives <= 0)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }
}
