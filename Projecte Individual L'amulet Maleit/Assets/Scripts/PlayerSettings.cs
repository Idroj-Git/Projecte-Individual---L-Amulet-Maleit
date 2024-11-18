using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] int health;
    private int maxHealth;
    [SerializeField] int weaponDamage;
    private float weaponCooldown;

    [SerializeField] Transform attackPosition;
    [SerializeField] Transform weaponPivot;
    public LayerMask whatIsEnemies;
    public float attackRange;

    [SerializeField] Animator axe_animator;

    [SerializeField] Transform interactPosition;
    [SerializeField] float interactRange = 0.5f;
    public LayerMask interactable;
    private bool canInteract = true;

    [SerializeField] ParticleSystem damagedParticleSystem;

    private Vector2 _moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        // igualar health al que tingui guardat en el playerprefs, canviar el serializefield
        // same per weaponDamage
        maxHealth = health;
        weaponCooldown = 0;
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
        if (_moveDirection.x < 0)
        {
            weaponPivot.localScale = new Vector3(Mathf.Abs(weaponPivot.localScale.x) * -1, weaponPivot.localScale.y, weaponPivot.localScale.z);
        }
        else if (_moveDirection.x > 0)
        {
            weaponPivot.localScale = new Vector3(Mathf.Abs(weaponPivot.localScale.x), weaponPivot.localScale.y, weaponPivot.localScale.z);
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
        this.health -= dmg;
        damagedParticleSystem.Play();
        if (this.health <= 0)
        {
            SceneController.LoadLoseScene();
        }
        Debug.Log("*player says* Ouch");
    }

    public void Attack()
    {
        if (SceneController.GetActualSceneIndex() == 2) // es poden juntar els dos IF, pero per mantenir ordre no ho faig
        {
            if (weaponCooldown <= 0)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whatIsEnemies); // Mirar com es fa per que quedi una estela de dmg (coroutine o algo)
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(weaponDamage);
                }
                weaponCooldown = 1f;
                axe_animator.SetTrigger("Attack");
            }
        }
    }

    public void Interact()
    {
        if (SceneController.GetActualSceneIndex() != 2 && canInteract) // tot menys l'escena de batalla
        {
            Collider2D interactedObject = Physics2D.OverlapCircle(interactPosition.position, interactRange, interactable); // Sense el ALL (OverlapCircle) perquè només vull una interacció
            if (interactedObject != null)
            {
                if (interactedObject.GetComponent<NPCController>() != null)
                {
                    NPCController nPCController = interactedObject.GetComponent< NPCController>();
                    nPCController.Interacted();
                }
                else if (interactedObject.GetComponent <ItemController>() != null)
                {
                    // item thing
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactPosition.position, interactRange);
    }
}
