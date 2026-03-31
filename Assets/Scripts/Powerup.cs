using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _fallSpeed;
    [SerializeField]
    private float _yDespawnPosition;
    [SerializeField]
    private Vector2 _powerupDirection = Vector3.down;

    void Start()
    {

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
            if (player != null) player.ActivateTripleShot();

            Destroy(this.gameObject, 0.1f);
        }
    }
}
