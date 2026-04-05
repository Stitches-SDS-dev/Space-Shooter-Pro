using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startPosition;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _destructionDelay;
    [SerializeField]
    private GameObject _explosion;

    private SpawnManager _spawnManager;
    private CircleCollider2D _collider;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("Spawn Manager not found!");

        _collider = GetComponent<CircleCollider2D>();
        if (_collider == null) Debug.LogError("CircleCollider2D not found!");

        transform.position = _startPosition;
    }

    void Update()
    {
        transform.Rotate(0, 0, 1 * _rotateSpeed *  Time.deltaTime);    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            _collider.enabled = false;
            _spawnManager.StartSpawning();
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject, _destructionDelay);
        }
    }
}
