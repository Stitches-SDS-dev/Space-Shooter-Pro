using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Dateils")]
    [SerializeField]
    private bool _isAlive = true;
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
    [SerializeField]
    private GameObject[] _damageSprites;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private int _maxAmmo = 15;
    [SerializeField]
    private int _currentAmmo;
    [SerializeField]
    private float _baseFireRate;

    [Header("Player Binds")]
    [SerializeField]
    private bool _shouldWrap = false;
    [SerializeField]
    private float _playerWrap, _xPlayerBind, _upperPlayerBind, _lowerPlayerBind;

    [Header("Genade Settings")]
    [SerializeField]
    private bool _isGrenadeActive = false;
    [SerializeField]
    private GameObject _grenadePrefab;
    [SerializeField]
    private float _yGrenadeOffset;
    [SerializeField]
    private AudioClip _grenadeAudio;
    [SerializeField]
    private float _grenadeFireRate = 1f;

    [Header("Laser Settings")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private Transform _weaponParent;
    [SerializeField]
    private float _yLaserOffset;
    [SerializeField]
    private AudioClip _laserAudio;

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
    [SerializeField]
    private AudioClip _powerupAudio;

    [Header("Thruster Options")]
    [SerializeField]
    private bool _isThrusterActive = false;
    [SerializeField]
    private float _thrusterBoost;
    [SerializeField]
    private SpriteRenderer _thrusterSprite;
    [SerializeField]
    private Color _baseColor;
    [SerializeField]
    private Color _boostColor = Color.white;

    [Header("Camera Shake Options")]
    [SerializeField]
    private float _shakeDelay;
    [SerializeField]
    private float _maxShake;

    private Camera _mainCamera;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Vector3 _laserOffset = new Vector3();
    private Vector3 _grenadeOffset = new Vector3();
    private float _canFire = 0f;
    private bool _isSpeedBoostActive = false;
    private float _fireRate;

    void Start()
    {
        // assign starting position
        transform.position = _startPosition;

        _currentAmmo = _maxAmmo;

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("Spawn Manager not found!");

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (_uiManager == null) Debug.LogError("UI Manager not found!");

        _mainCamera = Camera.main;

        _fireRate = _baseFireRate;
    }

    void Update()
    {
        if (_isAlive)
        {
            ThrusterStatus();
            PlayerMovement();

            // check for input and rate of fire delay
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && Time.time >= _canFire) Shoot();
        }
    }

    private void ThrusterStatus()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isThrusterActive)
        {
            if (!_uiManager.QueryFullDrain())
            {
                _uiManager.StartDrainingGauge();
                _playerSpeed += _thrusterBoost;
                _thrusterSprite.color = _boostColor;
                _isThrusterActive = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _isThrusterActive)
        {
            _uiManager.StopDrainingGauge();
            _playerSpeed -= _thrusterBoost;
            _thrusterSprite.color = _baseColor;
            _isThrusterActive = false;
        }
    }

    private void Shoot()
    {
        // fire laser with an offset above the Player
        _canFire = Time.time + _fireRate;
        if (!_isGrenadeActive)
        {
            if (!_isTripleShotActive)
            {
                if (_currentAmmo > 0)
                {
                    // fire standard laser
                    _laserOffset = transform.position;
                    _laserOffset.y += _yLaserOffset;
                    Instantiate(_laserPrefab, _laserOffset, Quaternion.identity, _weaponParent);
                    AudioSource.PlayClipAtPoint(_laserAudio, _mainCamera.transform.position);

                    _currentAmmo--;
                    _uiManager.UpdateAmmo(_currentAmmo, _maxAmmo);
                }
            }
            else
            {
                // fire triple shot if active
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity, _weaponParent);
                AudioSource.PlayClipAtPoint(_laserAudio, _mainCamera.transform.position);
            }
        }
        else
        {
            // fire grenade
            _grenadeOffset = transform.position;
            _grenadeOffset.y += _yGrenadeOffset;
            Instantiate(_grenadePrefab, _grenadeOffset, Quaternion.identity, _weaponParent);
            //play audio
            AudioSource.PlayClipAtPoint(_grenadeAudio, _mainCamera.transform.position);
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
            CameraShake(0.5f);
            _uiManager.UpdateLives(_playerLives);
            switch (_playerLives) {
                case 0:
                    _isAlive = false;
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                    _uiManager.GameOver();
                    _spawnManager.StopSpawning();
                    Destroy(this.gameObject, 0.5f);
                    break;
                case 1:
                    for (int i = 0; i < _damageSprites.Length; i++)
                    {
                        if (!_damageSprites[i].activeInHierarchy)
                        {
                            _damageSprites[i].SetActive(true);
                        }                        
                    }
                    break;
                case 2:
                    int randomDamage = Random.Range(0, _damageSprites.Length);
                    _damageSprites[randomDamage].SetActive(true);
                    break;
            }
        }
    }

    public void IncreaseScore(int value)
    {
        _score += value;
        _uiManager.UpdateScore(_score);
    }

    #region Powerups
    public void SelectPowerup(GameObject powerup, PowerupType powerupType, WaitForSeconds timer, float bonus)
    {
        switch (powerupType)
        {
            case PowerupType.TripleShot:
                ActivateTripleShot(timer, powerup);
                break;
            case PowerupType.SpeedBoost:
                ActivateSpeedBoost(timer, bonus, powerup);
                break;
            case PowerupType.ShieldBonus:
                ActivateShieldBonus(timer, (int) bonus, powerup);
                break;
            case PowerupType.AmmoPickup:
                AmmoPickedUp((int)bonus, powerup);
                break;
            case PowerupType.ExtraLife:
                AddExtraLife((int) bonus, powerup);
                break;
            case PowerupType.Grenade:
                ActivateGrenade(timer, powerup);
                break;
        }
    }

    private void ActivateGrenade(WaitForSeconds timer, GameObject powerup)
    {
        _isGrenadeActive = true;
        _fireRate = _grenadeFireRate;
        StartCoroutine(GrenadeCooldown(timer));
        AudioSource.PlayClipAtPoint(_powerupAudio, _mainCamera.transform.position);
        Destroy(powerup, 0.1f);
    }

    private void AddExtraLife(int bonus, GameObject powerup)
    {
        if (_playerLives < 3)
        {
            _playerLives++;
            switch (_playerLives)
            {
                case 2:
                    int randomDamage = Random.Range(0, _damageSprites.Length);
                    _damageSprites[randomDamage].SetActive(false);
                    break;
                case 3:
                    for (int i = 0; i < _damageSprites.Length; i++)
                    {
                        if (!_damageSprites[i].activeInHierarchy)
                        {
                            _damageSprites[i].SetActive(false);
                        }
                    }
                    break;
            }
            AudioSource.PlayClipAtPoint(_powerupAudio, _mainCamera.transform.position);
            Destroy(powerup, 0.1f);
        }
    }

    private void AmmoPickedUp(int addAmmo, GameObject powerup)
    {
        if (_currentAmmo != _maxAmmo)
        {
            _currentAmmo += addAmmo;
            if (_currentAmmo > _maxAmmo) _currentAmmo = _maxAmmo;
            _uiManager.UpdateAmmo(_currentAmmo, _maxAmmo);
            AudioSource.PlayClipAtPoint(_powerupAudio, _mainCamera.transform.position);
            Destroy(powerup, 0.1f);
        }
    }

    private void ActivateShieldBonus(WaitForSeconds timer, int bonus, GameObject powerup)
    {
        if (!_isShieldActive)
        {
            _isShieldActive = true;
            _shieldStrength = bonus;
            _shieldBonusVisual.SetActive(true);
            StartCoroutine(ShieldBonusCooldown(timer));
            AudioSource.PlayClipAtPoint(_powerupAudio, _mainCamera.transform.position);
            Destroy(powerup, 0.1f);
        }
    }

    private void ActivateSpeedBoost(WaitForSeconds timer, float bonus, GameObject powerup)
    {
        if (!_isSpeedBoostActive)
        {
            _isSpeedBoostActive = true;
            _playerSpeed += bonus;
            _thrusterSprite.color = _boostColor;
            StartCoroutine(SpeedBoostCooldown(timer, bonus));
            AudioSource.PlayClipAtPoint(_powerupAudio, _mainCamera.transform.position);
            Destroy(powerup, 0.1f);
        }
    }

    private void ActivateTripleShot(WaitForSeconds timer, GameObject powerup)
    {
        if (!_isTripleShotActive)
        {
            _isTripleShotActive = true;
            StartCoroutine(TripleShotCooldown(timer));
            AudioSource.PlayClipAtPoint(_powerupAudio, _mainCamera.transform.position);
            Destroy(powerup, 0.1f);
        }
    }

    private IEnumerator ShieldBonusCooldown (WaitForSeconds timer)
    {
        yield return timer;
        _isShieldActive = false;
        _shieldBonusVisual.SetActive(false);
    }
    private IEnumerator SpeedBoostCooldown(WaitForSeconds timer, float bonus)
    {
        yield return timer;
        _isSpeedBoostActive = false;
        _playerSpeed -= bonus;
        _thrusterSprite.color = _baseColor;
    }

    private IEnumerator TripleShotCooldown(WaitForSeconds timer)
    {
        yield return timer;
        _isTripleShotActive = false;
    }

    private IEnumerator GrenadeCooldown(WaitForSeconds timer)
    {
        yield return timer;
        _isGrenadeActive = false;
        _fireRate = _baseFireRate;
    }
    #endregion

    #region --- Camera Shake ---

    void CameraShake(float duration)
    {
        StartCoroutine(CameraShakeRoutine(duration));
    }

    IEnumerator CameraShakeRoutine(float duration)
    {

        Transform main = Camera.main.transform;
        WaitForSeconds wait = new WaitForSeconds(_shakeDelay);
        Vector3 rotation = main.localEulerAngles;
        rotation.z = _maxShake;

        while (duration >= 0)
        {

            main.eulerAngles = rotation;
            rotation.z -= 0.1f;
            yield return wait;

            main.eulerAngles = -rotation;
            yield return wait;
            duration -= 0.1f;

            Debug.Log(duration);
        }

        main.eulerAngles = Vector3.zero;
    }

    #endregion
}
