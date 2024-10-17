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

    [SerializeField] Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        InputController.OnMoveInput += SetMoveDirection;

        _rb = GetComponent<Rigidbody2D>();

        spawnCooldown = spawnCooldownMax;

        if (SceneController.GetActualSceneIndex() == 2)
        {
            _rb.position = new Vector2(-6, 2); // aprox de on vull que faci spawn
        }
        else if (RuntimeGameSettings.Instance.GetPlayerLastPostion() != Vector2.zero) // CANVIAR AMB EL GETTER / SETTER
        {
            _rb.position = RuntimeGameSettings.Instance.GetPlayerLastPostion();
        }
        //else
        //{
        //    _rb.position = new Vector2(-5,1);
        //}
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero)
        {
            MovePlayer();
            _animator.SetBool("isMoving", true);
            _animator.SetFloat("x_direction", _moveDirection.x);
            _animator.SetFloat("y_direction", _moveDirection.y);
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _animator.SetBool("isMoving", false);
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
        //Debug.Log("Direcci�n de movimiento recibida: " + _moveDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnableEnemies") && spawnCooldown == 0)
        {
            Debug.Log("�HA APARECIDO UN ENEMIGO!");
            RuntimeGameSettings.Instance.SetPlayerLastPosition(_rb.position); // CANVIAR AMB EL SETTER
            SceneController.LoadScene(2);
        }
    }

    void PlayerSpriteChange(float y_direction)
    {
        if (y_direction < 0)
        {

        }
        else if (y_direction > 1)
        {

        }
    }
}
