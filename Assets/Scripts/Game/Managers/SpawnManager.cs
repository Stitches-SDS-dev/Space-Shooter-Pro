using System.Collections;
using UnityEditor.Animations;
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
    [SerializeField]
    private GameObject _weaponParent;
    [SerializeField]
    private Transform[] _objectParents; // 0 = Weapons, 1 = Enemies, 2 = Powerups 

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
            Instantiate(_enemyPrefab, _spawnPosition, Quaternion.identity, _objectParents[1]);
            yield return _enemySpawnTimer;
        }
    }

    private IEnumerator SpawnPowerups()
    {
        yield return _powerupSpawnTimer;
        while (_isSpawning)
        {            
            // set position parameters
            _spawnPosition.x = Random.Range(-_xSpawnBind, _xSpawnBind);

            int powerupSelector = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[powerupSelector], _spawnPosition, Quaternion.identity, _objectParents[2]);
            yield return _powerupSpawnTimer;
        }
    }

    public void StopSpawning()
    {
        _isSpawning = false;
        CleanupPrefabs(); //somehow continues to spawn powerups
    }

    private void CleanupPrefabs()
    {
        for (int i = 0;  i < _objectParents.Length; i++)
        {
            foreach (Transform child in _objectParents[i]) Destroy(child.gameObject);
        }
    }
}
