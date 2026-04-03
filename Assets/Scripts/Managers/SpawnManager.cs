using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Global Spawn Settings")]
    [SerializeField]
    private bool _isSpawning = true;
    [SerializeField]
    private float _xSpawnBind;
    [SerializeField]
    private float _ySpawnPosition;

    [Header("Enemy Spawn Settings")]
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private Transform _enemyParent;
    [SerializeField]
    private float _enemySpawnDelay;

    [Header("Powerup Spawn Settings")]
    [SerializeField]
    private Transform _powerupParent;
    [SerializeField]
    private float _powerupSpawnDelay;
    [SerializeField]
    private GameObject _tripleShotPowerupPrefab;
    [SerializeField]
    private GameObject _speedBoostPowerupPrefab;

    private Vector3 _spawnPosition = new Vector3();
    private WaitForSeconds _enemySpawnTimer;
    private WaitForSeconds _poweruptSpawnTimer;

    void Start()
    {
        _enemySpawnTimer = new WaitForSeconds(_enemySpawnDelay);
        _poweruptSpawnTimer = new WaitForSeconds(_powerupSpawnDelay);

        // set upper spawn point for enemies
        _spawnPosition.y = _ySpawnPosition;

        // start spawning
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }

    private IEnumerator SpawnEnemies()
    {
        while (_isSpawning)
        {
            // set position parameters
            _spawnPosition.x = Random.Range(-_xSpawnBind, _xSpawnBind);

            // spawn
            Instantiate(_enemyPrefab, _spawnPosition, Quaternion.identity, _enemyParent);
            yield return _enemySpawnTimer;
        }
    }

    private IEnumerator SpawnPowerups()
    {
        while (_isSpawning)
        {            
            yield return _poweruptSpawnTimer;
            // set position parameters
            _spawnPosition.x = Random.Range(-_xSpawnBind, _xSpawnBind);

            int powerupSelector = Random.Range(0, 2);
            GameObject powerup = null;
            switch (powerupSelector)
            {
                case 0:
                    powerup = _tripleShotPowerupPrefab;
                    break;
                case 1:
                    powerup = _speedBoostPowerupPrefab;
                    break;
            }

            Instantiate(powerup, _spawnPosition, Quaternion.identity, _powerupParent);
        }
    }

    public void StopSpawning()
    {
        _isSpawning = false;
    }
}
