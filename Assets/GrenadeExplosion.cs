using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField]
    private float _colliderActivationDelay;

    private WaitForSeconds _colliderActivationTimer;
    private WaitForSeconds _explosionDeathTimer;
    private CircleCollider2D _collider;

    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        if (_collider == null) Debug.LogError("CircleCollider2D not found!");
        
        _colliderActivationTimer = new WaitForSeconds(_colliderActivationDelay);
        _explosionDeathTimer = new WaitForSeconds(2.7f - _colliderActivationDelay);

        StartCoroutine(AcivateCollider());
    }

    private IEnumerator AcivateCollider()
    {
        yield return _colliderActivationTimer;
        _collider.enabled = true;
        yield return _explosionDeathTimer;
        Destroy(this.gameObject);
    }
}
