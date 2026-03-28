using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private Transform _enemyParent;
    [SerializeField]
    private float _spawnDelay;
    [SerializeField]
    private bool _isSpawning = true;
    [SerializeField]
    private float _xSpawnLeftBind;
    [SerializeField]
    private float _xSpawnRightBind;
    [SerializeField]
    private float _ySpawnPosition;

    private float _xSpawnPosition;
    private Vector3 _spawnPosition = new Vector3();

    void Start()
    {
        // set upper spawn point for enemies
        _spawnPosition.y = _ySpawnPosition;

        // start spawning
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (_isSpawning)
        {
            // set position parameters
            _xSpawnPosition = Random.Range(_xSpawnLeftBind, _xSpawnRightBind);
            _spawnPosition.x = _xSpawnPosition;

            // spawn
            Instantiate(_enemyPrefab, _spawnPosition, Quaternion.identity, _enemyParent);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    public void StopSpawning()
    {
        _isSpawning = false;
    }
}
