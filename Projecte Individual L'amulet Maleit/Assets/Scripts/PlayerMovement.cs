using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float _speed = 6f;
    private Vector2 _moveDirection;

    private Rigidbody2D _rb;

    private float spawnCooldown;
    private float spawnCooldownMax = 10f;

    // Start is called before the first frame update
    void Start()
    {
        InputController.OnMoveInput += SetMoveDirection;

        _rb = GetComponent<Rigidbody2D>();

        spawnCooldown = spawnCooldownMax;

        if (SceneController.GetActualSceneIndex() == 1)
        {
            _rb.position = new Vector2(-6, 2); // aprox de on vull que faci spawn
        }
        else if (RuntimeGameSettings.Instance.playerLastPosition != Vector2.zero) // CANVIAR AMB EL GETTER / SETTER
        {
            _rb.position = RuntimeGameSettings.Instance.playerLastPosition;
        }
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

        if (spawnCooldown > 0)
        {
            spawnCooldown -= Time.deltaTime;
        }
        else
        {
            spawnCooldown = 0;
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
        if (collision.CompareTag("SpawnableEnemies") && spawnCooldown == 0)
        {
            Debug.Log("¡HA APARECIDO UN ENEMIGO!");
            RuntimeGameSettings.Instance.playerLastPosition = _rb.position; // CANVIAR AMB EL SETTER
            SceneController.LoadScene(1);
        }
    }
}
