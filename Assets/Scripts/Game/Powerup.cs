using UnityEngine;

public enum PowerupType
{
    TripleShot,
    SpeedBoost,
    ShieldBonus,
    AmmoPickup,
    ExtraLife
}

public class Powerup : MonoBehaviour
{   
    [SerializeField]
    private PowerupType _powerupType;
    [SerializeField]
    private float _fallSpeed;
    [SerializeField]
    private float _yDespawnPosition;
    [SerializeField]
    private Vector2 _powerupDirection = Vector3.down;
    [SerializeField]
    [Tooltip("If Applicable")]
    private float _duration;
    [SerializeField]
    [Tooltip("If Applicable")]
    private float _bonusApplied;

    private WaitForSeconds _timer;

    void Start()
    {
        _timer = new WaitForSeconds(_duration);
    }

    void Update()
    {
        transform.Translate(_powerupDirection * _fallSpeed * Time.deltaTime);

        if (transform.position.y <= _yDespawnPosition) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null) player.SelectPowerup(this.gameObject, _powerupType, _timer, _bonusApplied);
        }
    }
}
