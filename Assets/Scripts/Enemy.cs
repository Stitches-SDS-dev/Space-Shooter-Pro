using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed;
    [SerializeField]
    private Vector3 _enemyDirection = Vector3.down;
    [SerializeField]
    private float _yRespawnPosition;
    [SerializeField]
    private float _yDroppedOffPosition;
    [SerializeField]
    private float _xRespawnBindLeft;
    [SerializeField]
    private float _xRespawnBindRight;

    private Vector3 _respawnPosition = Vector3.zero;
    private float _xRespawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move down at appropriate speed
        transform.Translate(_enemyDirection * _enemySpeed * Time.deltaTime);

        // at bottom respawn at random x position
        if (transform.position.y < _yDroppedOffPosition)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // create random x pos within binds
        _xRespawnPosition = Random.Range(_xRespawnBindLeft, _xRespawnBindRight);

        // set respawn location
        _respawnPosition.x = _xRespawnPosition;
        _respawnPosition.y = _yRespawnPosition;

        // respawn at new location
        transform.position = _respawnPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if hit by Player Laser, destroy this enemy
        if (other.CompareTag("Laser"))
        {
            Debug.Log(other.transform.name);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        } 
        else if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.DamagePlayer();
                Destroy(this.gameObject);
            }
        }
    }
}
