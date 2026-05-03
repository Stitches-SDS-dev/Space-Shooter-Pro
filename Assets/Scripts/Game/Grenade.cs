using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Vector3 _originPos;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _travelDistance;
    [SerializeField]
    private GameObject _grenadeExplosion;
    [SerializeField]
    private float _destroyDelay;

    private WaitForSeconds _destroyTimer;

    void Start()
    {
        _originPos = transform.position;
        _destroyTimer = new WaitForSeconds(_destroyDelay);
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime);
        if (transform.position.y > _originPos.y + _travelDistance)
        {
            InitiateExplosion();
        }
    }

    private void InitiateExplosion()
    {
        Instantiate(_grenadeExplosion, transform.position, Quaternion.identity);
        StartCoroutine(CleanupRoutine());
    }

    private IEnumerator CleanupRoutine()
    {
        yield return _destroyTimer;
        Destroy(this.gameObject);
    }
}
