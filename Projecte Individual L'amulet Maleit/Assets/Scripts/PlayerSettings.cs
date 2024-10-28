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
    public LayerMask interactable;

    private Vector2 _moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        // igualar health al que tingui guardat en el playerprefs, canviar el serializefield
        // same per weaponDamage
        maxHealth = health;
        weaponCooldown = 0;
        InputController.OnAttackInput += Attack;
        InputController.OnMoveInput += SetMoveDirection;
    }
    void OnDestroy()
    {
        InputController.OnAttackInput -= Attack;
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
            interactPosition.localScale = new Vector3(Mathf.Abs(interactPosition.localScale.x) * -1, interactPosition.localScale.y, interactPosition.localScale.z);
        }
        else if ( _moveDirection.x > 0 && _moveDirection.y == 0) // mira a la dere
        {
            interactPosition.localScale = new Vector3(Mathf.Abs(interactPosition.localScale.x), interactPosition.localScale.y, interactPosition.localScale.z);
        }
        else if (_moveDirection.y < 0  && _moveDirection.x == 0) // mira abajo
        {
            interactPosition.localScale = new Vector3(interactPosition.localScale.x, Mathf.Abs(interactPosition.localScale.y) * -1, interactPosition.localScale.z);
        }
        else if (_moveDirection.y > 0 && _moveDirection.x == 0) // mira arriba
        {
            interactPosition.localScale = new Vector3(interactPosition.localScale.x, Mathf.Abs(interactPosition.localScale.y), interactPosition.localScale.z);
        }
    }

    public void TakeDamage(int dmg)
    {
        this.health -= dmg;
        if (this.health <= 0)
        {
            SceneController.LoadScene(4);
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
        if (SceneController.GetActualSceneIndex() != 2)
        {
            Collider2D interactedObjects = Physics2D.OverlapCircle(interactPosition.position, 0.5f, interactable); // Sense el ALL perquè només vull una interacció
            // como podria hacer que esto sea una colisión que active el OnCollisionEnter de otros objetos?
        }
    }

    private void OnDrawGizmosSelected() // Això serveix per poder veure el rang d'atac desde l'escena (no en el joc)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
