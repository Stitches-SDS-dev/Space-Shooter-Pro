using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed;
    [SerializeField]
    private Vector3 _laserDirection = Vector3.down;
    [SerializeField]
    private float _despawnPosition;
    
    void Update()
    {
        transform.Translate(_laserDirection * _laserSpeed * Time.deltaTime);

        // destroy after leaving screen
        if (transform.position.y < _despawnPosition) Destroy(this.gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.DamagePlayer();
            }
            Destroy(this.gameObject);
        }
    }
}
