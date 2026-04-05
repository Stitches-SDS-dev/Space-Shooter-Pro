using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed;
    [SerializeField]
    private int _scoreValue;
    [SerializeField]
    private Vector3 _enemyDirection = Vector3.down;
    [SerializeField]
    private float _yRespawnPosition;
    [SerializeField]
    private float _yDroppedOffPosition;
    [SerializeField]
    private float _xRespawnBind;
    [SerializeField]
    private float _destroyDelay;

    private Vector3 _respawnPosition = Vector3.zero;
    private float _xRespawnPosition;
    private bool _isDying = false;

    private Player _player;
    private Animator _deathAnim;
    private BoxCollider2D _collider;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.LogError("Player not found!");

        _deathAnim = GetComponent<Animator>();
        if (_deathAnim == null) Debug.LogError("Animator not found!");

        _collider = GetComponent<BoxCollider2D>();
        if (_collider == null) Debug.LogError("BoxCollider2D not found!");
    }

    private void Update()
    {
        // move down at appropriate speed
        transform.Translate(_enemyDirection * _enemySpeed * Time.deltaTime);

        // at bottom respawn at random x position
        if (transform.position.y < _yDroppedOffPosition && !_isDying) Respawn();
    }

    private void Respawn()
    {
        // create random x pos within binds
        _xRespawnPosition = Random.Range(-_xRespawnBind, _xRespawnBind);

        // set respawn location
        _respawnPosition.x = _xRespawnPosition;
        _respawnPosition.y = _yRespawnPosition;

        // respawn at new location
        transform.position = _respawnPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if hit by Laser, destroy this Enemy
        if (other.CompareTag("Laser"))
        {
            _isDying = true;
            _player.IncreaseScore(_scoreValue);
            _deathAnim.SetTrigger("OnEnemyDeath");
            _collider.enabled = false;
            Destroy(other.gameObject);
            Destroy(this.gameObject, _destroyDelay);
        } 
        // if colliding with Player, damage Player and destroy this Enemy
        else if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _isDying = true;
                _player.DamagePlayer();
                _deathAnim.SetTrigger("OnEnemyDeath");
                _collider.enabled = false;
                Destroy(this.gameObject, _destroyDelay);
            }
        }       
    }
}
