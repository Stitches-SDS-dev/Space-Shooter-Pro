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
    private GameObject[] _powerups;

    private Vector3 _spawnPosition = new Vector3();
    private WaitForSeconds _enemySpawnTimer;
    private WaitForSeconds _powerupSpawnTimer;

    void Start()
    {
        // consider making a random timer... can this be done without using the new keyword elsewhere?
        _enemySpawnTimer = new WaitForSeconds(_enemySpawnDelay);
        _powerupSpawnTimer = new WaitForSeconds(_powerupSpawnDelay);

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
            yield return _powerupSpawnTimer;
            // set position parameters
            _spawnPosition.x = Random.Range(-_xSpawnBind, _xSpawnBind);

            int powerupSelector = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[powerupSelector], _spawnPosition, Quaternion.identity, _powerupParent);
        }
    }

    public void StopSpawning()
    {
        _isSpawning = false;
    }
}
