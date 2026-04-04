using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    [Header("Player Dateils")]
    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private int _score = 0;
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

    [Header("Power Up Options")]
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldBonusVisual;
    [SerializeField]
    private int _shieldStrength = 0;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Vector3 _laserOffset = new Vector3();
    private float _canFire = 0f;

    void Start()
    {
        // assign starting position
        transform.position = _startPosition;

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("Spawn Manager not found!");

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (_uiManager == null) Debug.LogError("UI Manager not found!");
    }

    void Update()
    {
        PlayerMovement();

        // check for input and rate of fire delay
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && Time.time >= _canFire) FireLaser();
    }

    private void FireLaser()
    {
        // fire laser with an offset above the Player
        _canFire = Time.time + _fireRate;
        if (!_isTripleShotActive)
        {
            // fire standard laser
            _laserOffset = transform.position;
            _laserOffset.y += _yLaserOffset;
            Instantiate(_laserPrefab, _laserOffset, Quaternion.identity, _laserParent);
        }
        else
        {
            // fire triple shot if active
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity, _laserParent);
        }
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
        
        // clamp on the x position
        if (!_shouldWrap) _bindPosition.x = Mathf.Clamp(_bindPosition.x, -_xPlayerBind, _xPlayerBind);
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
        if (_isShieldActive)
        {
            _shieldStrength--;
            if (_shieldStrength <= 0)
            {
                _isShieldActive = false;
                _shieldBonusVisual.SetActive(false);
            }
        }
        else
        {
            _playerLives--;
            _uiManager.UpdateLives(_playerLives);
            if (_playerLives <= 0)
            {
                _uiManager.GameOver();
                _spawnManager.StopSpawning();
                Destroy(this.gameObject);
            }
        }
    }

    public void IncreaseScore(int value)
    {
        _score += value;
        _uiManager.UpdateScore(_score);
    }

    #region Powerups
    public void SelectPowerup(Powerup.PowerupType powerupType, WaitForSeconds timer, float bonus)
    {
        switch (powerupType)
        {
            case Powerup.PowerupType.TripleShot:
                ActivateTripleShot(timer);
                break;
            case Powerup.PowerupType.SpeedBoost:
                ActivateSpeedBoost(timer, bonus);
                break;
            case Powerup.PowerupType.ShieldBonus:
                ActivateShieldBonus(timer, (int) bonus);
                break;
        }
    }

    private void ActivateShieldBonus(WaitForSeconds timer, int bonus)
    {
        _isShieldActive = true;
        _shieldStrength = bonus;
        _shieldBonusVisual.SetActive(true);
        StartCoroutine(ShieldBonusCooldown(timer));
    }

    private void ActivateSpeedBoost(WaitForSeconds timer, float bonus)
    {
        _playerSpeed += bonus;
        StartCoroutine(SpeedBoostCooldown(timer, bonus));
    }

    private void ActivateTripleShot(WaitForSeconds timer)
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCooldown(timer));
    }

    private IEnumerator ShieldBonusCooldown (WaitForSeconds timer)
    {
        yield return timer;
        _isShieldActive = false;
    }
    private IEnumerator SpeedBoostCooldown(WaitForSeconds timer, float bonus)
    {
        yield return timer;
        _playerSpeed -= bonus;
    }

    private IEnumerator TripleShotCooldown(WaitForSeconds timer)
    {
        yield return timer;
        _isTripleShotActive = false;
    }
    #endregion
}
