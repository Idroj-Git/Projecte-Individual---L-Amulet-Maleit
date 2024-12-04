using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _speed = 6f;
    private Vector2 _moveDirection;

    private Rigidbody2D _rb;

    private float spawnCooldown;
    private float spawnCooldownMax = 3f;

    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    private bool canMove = true, isPlayerOnSpawner = false;
    private Collider2D lastCollision;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        InputController.OnMoveInput += SetMoveDirection; // Aconseguir input de moviment

        //Tot això d'abaix estava al Start, però m'interessa que s'executi abans del primer frame.
        _rb = GetComponent<Rigidbody2D>();

        spawnCooldown = spawnCooldownMax;

        if (SceneController.GetActualSceneIndex() == 2)
        {
            _rb.position = new Vector2(-6, 2); // aprox de on vull que faci spawn
        }
        else if (RuntimeGameSettings.Instance.GetPlayerLastPostion() != Vector2.zero)
        {
            _rb.position = RuntimeGameSettings.Instance.GetPlayerLastPostion();
        }
    }
    private void OnDisable()
    {
        InputController.OnMoveInput -= SetMoveDirection; // Aconseguir input de moviment
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero && canMove)
        {
            MovePlayer();
            _animator.SetBool("isMoving", true);
            if (_moveDirection.x < 0) { spriteRenderer.flipX = true; } // SpriteFlip
            else { spriteRenderer.flipX = false; }
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
            spawnCooldown = 0.1f; // Per fer el check de spawn cada 0.2
            if (lastCollision != null)
            {
                if (lastCollision.CompareTag("SpawnableEnemies") && _moveDirection != Vector2.zero) 
                {
                    if (isPlayerOnSpawner && Random.Range(0, 100) < 5) // 5% de posibilitats d'spawn d'enemics
                    {
                        RuntimeGameSettings.Instance.SetPlayerLastPosition(_rb.position); // CANVIAR AMB EL SETTER
                        SceneController.LoadBattleScene();
                    }
                    //Debug.Log("¡HA APARECIDO UN ENEMIGO en el update!");
                }

            }
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
        if (collision.CompareTag("SpawnableEnemies") && spawnCooldown == 0 && Random.Range(0, 100) < 23) // 23% de que apareixin enemics
        {
            //Debug.Log("¡HA APARECIDO UN ENEMIGO!");
            //RuntimeGameSettings.Instance.SetPlayerLastPosition(_rb.position); // CANVIAR AMB EL SETTER
            //SceneController.LoadBattleScene();
        }
        else if (collision.CompareTag("SpawnableEnemies"))
        {
            //Debug.Log("aaa");
            lastCollision = collision;
            isPlayerOnSpawner = true;
        }
        else if (collision.CompareTag("CaveEntrance"))
        {
            RuntimeGameSettings.Instance.SetPlayerLastPosition(new Vector2(0f, 0f)); // POSICIÓ DE LA ENTRADA DINS LA COVA. Això ho necessito aquí perquè al combatre amb enemics em canvia el lloc on apareix
            SceneController.LoadCaveScene();
        }
        else if (collision.CompareTag("CaveExit"))
        {
            RuntimeGameSettings.Instance.SetPlayerLastPosition(new Vector2(81.55f, 32.8f)); // POSICIÓ DE LA ENTRADA DINS LA COVA. Això ho necessito aquí perquè al combatre amb enemics em canvia el lloc on apareix
            SceneController.LoadMainWorldScene();
        }
        else if (collision.CompareTag("ForestEntrance"))
        {
            RuntimeGameSettings.Instance.SetPlayerLastPosition(new Vector2(0f, 0f)); // POSICIÓ DE LA SORTIDA DEL BOSC
            SceneController.LoadForestScene();
        }
        else if (collision.CompareTag("ForestExit"))
        {
            RuntimeGameSettings.Instance.SetPlayerLastPosition(new Vector2(11.05f, -14.6f)); // POSICIÓ DE LA SORTIDA DEL BOSC
            SceneController.LoadMainWorldScene();
        }
        else if (collision.CompareTag("StoryTrigger"))
        {
            //DialogueController.Instance.ShowStoryDialogue(PlayerPrefs.GetInt("StoryFlag"));
            //PlayerPrefs.SetInt("StoryFlag", PlayerPrefs.GetInt("StoryFlag") + 1);
            //collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnableEnemies"))
        {
            isPlayerOnSpawner = false;
        }
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public Vector2 GetPosition() { return _rb.position; }
}
