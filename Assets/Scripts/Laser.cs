using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed;
    [SerializeField]
    private float _despawnPosition;
    private Vector3 _direction = Vector3.up;

    void Update()
    {
        // move laser
        transform.Translate(_direction * _laserSpeed * Time.deltaTime);

        // destroy after leaving screen
        if (transform.position.y > _despawnPosition)
        {
            Destroy(this.gameObject, 1f);
        }
    }
}
