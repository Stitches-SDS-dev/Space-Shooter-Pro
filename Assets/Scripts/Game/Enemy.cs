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

    private Vector3 _respawnPosition = Vector3.zero;
    private float _xRespawnPosition;

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.LogError("Player not found!");
    }

    private void Update()
    {
        // move down at appropriate speed
        transform.Translate(_enemyDirection * _enemySpeed * Time.deltaTime);

        // at bottom respawn at random x position
        if (transform.position.y < _yDroppedOffPosition) Respawn();
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
            _player.IncreaseScore(_scoreValue);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        } 
        // if colliding with Player, damage Player and destroy this Enemy
        else if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.DamagePlayer();
                Destroy(this.gameObject);
            }
        }       
    }
}
