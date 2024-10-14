using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float _speed = 6f;
    private Vector2 _moveDirection;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        InputController.OnMoveInput += SetMoveDirection;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero)
        {
            MovePlayer();
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    void MovePlayer()
    {
        _rb.velocity = _moveDirection * _speed;
        //Debug.Log("Movimiento hacia " + _moveDirection);
    }

    void SetMoveDirection(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
        //Debug.Log("Dirección de movimiento recibida: " + _moveDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnableEnemies"))
        {
            Debug.Log("HA APARECIDO UN ENEMIGO!");
        }
    }
}
