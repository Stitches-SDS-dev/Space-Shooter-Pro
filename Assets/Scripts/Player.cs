using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startPosition = new Vector3(0, -3, 0);
    [SerializeField]
    private float _playerSpeed = 2.5f;
    [SerializeField]
    Vector3 _direction = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        // assign starting position
        transform.position = _startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float hMove;
        float vMove;

        hMove = Input.GetAxis("Horizontal");
        vMove = Input.GetAxis("Vertical");

        _direction.x = hMove;
        _direction.y = vMove;

        transform.Translate(_direction * _playerSpeed * Time.deltaTime);
    }
}
