using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerSettings : MonoBehaviour
{
    //[SerializeField] int health;
    //private int maxHealth;
    //[SerializeField] int playerDamage;
    private float weaponCooldown;

    [SerializeField] Transform attackPosition;
    private Vector3 attackPositionVector;
    [SerializeField] Transform weaponPivot;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsFlyingEnemies;
    public float attackRange;
    private bool isAttacking = false;
    private float attackDuration = 0.2f, attackTimer = 0f;
    private HashSet<EnemyController> damagedEnemies = new HashSet<EnemyController>();

    [SerializeField] Animator axe_animator;

    [SerializeField] Transform interactPosition;
    [SerializeField] float interactRange = 0.5f;
    public LayerMask interactable;
    private bool canInteract = true;

    [SerializeField] ParticleSystem damagedParticleSystem;

    private Vector2 _moveDirection;

    [SerializeField] MusicController musicController;
    [SerializeField] HealthbarBehaviour healthbar;

    // Start is called before the first frame update
    void Start()
    {
        // igualar health al que tingui guardat en el playerprefs, canviar el serializefield
        // same per weaponDamage
        //maxHealth = RuntimeGameSettings.Instance.GetActualHealth();
        weaponCooldown = 0;
        if (healthbar != null)
            healthbar.SetHealth(RuntimeGameSettings.Instance.GetActualHealth(), RuntimeGameSettings.Instance.GetMaxHealth());
    }

    private void OnEnable()
    {
        InputController.OnAttackInput += Attack;
        InputController.OnMoveInput += SetMoveDirection;
        InputController.OnOverworldInteract += Interact;
    }
    void OnDisable()
    {
        InputController.OnAttackInput -= Attack;
        InputController.OnMoveInput -= SetMoveDirection;
        InputController.OnOverworldInteract -= Interact;
    }
    void SetMoveDirection(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
        //Debug.Log("Dirección de movimiento recibida: " + _moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer > 0)
            {
                PerformAttack();
            }
            else
            {
                isAttacking = false; // Finaliza el ataque cuando el temporizador llega a 0
            }
        }

        if (weaponCooldown > 0)
        {
            weaponCooldown -= Time.deltaTime;
        }

        if (weaponPivot != null)
        {
            TurnWeaponPivot();
        }

        if (interactPosition != null)
        {
            TurnInteractPosition();
        }
    }

    private void TurnWeaponPivot()
    {
        if (!isAttacking)
        {
            if (_moveDirection.x < 0)
            {
                weaponPivot.localScale = new Vector3(Mathf.Abs(weaponPivot.localScale.x) * -1, weaponPivot.localScale.y, weaponPivot.localScale.z);
            }
            else if (_moveDirection.x > 0)
            {
                weaponPivot.localScale = new Vector3(Mathf.Abs(weaponPivot.localScale.x), weaponPivot.localScale.y, weaponPivot.localScale.z);
            }
        }
    }

    private void TurnInteractPosition()
    {
        if (_moveDirection.x < 0 && _moveDirection.y == 0) // mira a la izq
        {
            interactPosition.localPosition = new Vector3(-1f, 0f, interactPosition.localScale.z);
        }
        else if ( _moveDirection.x > 0 && _moveDirection.y == 0) // mira a la dere
        {
            interactPosition.localPosition = new Vector3(1f, 0f, interactPosition.localScale.z);
        }
        else if (_moveDirection.y < 0  && _moveDirection.x == 0) // mira abajo
        {
            interactPosition.localPosition = new Vector3(0f, -1f, interactPosition.localScale.z);
        }
        else if (_moveDirection.y > 0 && _moveDirection.x == 0) // mira arriba
        {
            interactPosition.localPosition = new Vector3(0f, 1f, interactPosition.localScale.z);
        }
    }

    public void TakeDamage(int dmg)
    {
        RuntimeGameSettings.Instance.SetActualHealth(RuntimeGameSettings.Instance.GetActualHealth() - dmg);
        //this.health -= dmg;
        damagedParticleSystem.Play();
        musicController.PlayHurt();
        healthbar.SetHealth(RuntimeGameSettings.Instance.GetActualHealth(), RuntimeGameSettings.Instance.GetMaxHealth());
        if (RuntimeGameSettings.Instance.GetActualHealth() <= 0)
        {
            musicController.PlayDeath();
            SceneController.LoadLoseScene();
        }
        //Debug.Log("*player says* Ouch");
    }

    public void Attack()
    {
        if (SceneController.GetActualSceneIndex() == SceneController.GetBattleIndex()) // es poden juntar els dos IF, pero per mantenir ordre no ho faig
        {
            if (weaponCooldown <= 0)
            {
                musicController.PlaySlash();
                weaponCooldown = 1f;
                axe_animator.SetTrigger("Attack");

                isAttacking = true; // ACTIVA AIXÒ
                attackTimer = attackDuration;
                attackPositionVector = attackPosition.position;
                damagedEnemies.Clear(); // Esborra els enemics que hi havia en el HashSet
            }
        }
    }

    private void PerformAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whatIsFlyingEnemies);
        Collider2D[] flyingEnemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whatIsEnemies);

        Collider2D[] allEnemiesToDamage = enemiesToDamage.Concat(flyingEnemiesToDamage).ToArray();

        // tmb es pot fer amb foreach: foreach (Collider2D collider in enemiesToDamage)
        for (int i = 0; i < allEnemiesToDamage.Length; i++)
        {
            EnemyController enemy = allEnemiesToDamage[i].GetComponent<EnemyController>();
            if (!damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(RuntimeGameSettings.Instance.GetPlayerDamage());

                if (enemy.getActualHealth() < enemy.getMaxHealth())
                    musicController.PlayDeath();
                else
                    musicController.PlayHurt();

                damagedEnemies.Add(enemy);
            }
        }
    }

    public void Interact()
    {
        if (SceneController.GetActualSceneIndex() != SceneController.GetBattleIndex() && canInteract) // tot menys l'escena de batalla
        {
            Collider2D interactedObject = Physics2D.OverlapCircle(interactPosition.position, interactRange, interactable); // Sense el ALL (OverlapCircle) perquè només vull una interacció
            if (interactedObject != null)
            {
                if (interactedObject.GetComponent<NPCController>() != null)
                {
                    NPCController npcController = interactedObject.GetComponent<NPCController>();
                    npcController.Interacted();
                }
                else if (interactedObject.GetComponent <ItemController>() != null)
                {
                    Debug.Log("Objeto detectado: " + interactedObject.name);
                    ItemController itemController = interactedObject.GetComponent<ItemController>();
                    itemController.Interacted();
                }
                else
                {
                    Debug.Log("No hay nada con que interactuar");
                }
            }
        }
    }

    public void SetCanInteract(bool canInteract)
    {
        this.canInteract = canInteract;
    }

    public bool GetCanInteract()
    {
        return canInteract;
    }

    private void OnDrawGizmosSelected() // Això serveix per poder veure el rang d'atac desde l'escena (no en el joc)
    {
        if (attackPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
        }

        if (interactPosition != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(interactPosition.position, interactRange);
        }
    }
}
