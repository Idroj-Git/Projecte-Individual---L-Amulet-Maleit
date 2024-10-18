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

    public Transform attackPosition;
    public LayerMask whatIsEnemies;
    public float attackRange;

    [SerializeField] Animator axe_animator;

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
                if (_moveDirection.x < 0)
                {
                    attackPosition.localPosition = new Vector3(attackPosition.position.x * -1, attackPosition.position.y, attackPosition.position.z);
                }
                else if (_moveDirection.x > 0)
                {
                    attackPosition.localPosition = new Vector3(Mathf.Abs(attackPosition.position.x), attackPosition.position.y, attackPosition.position.z);
                }

                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(weaponDamage);
                }
                weaponCooldown = 1.5f;
                axe_animator.SetTrigger("Attack");
            }
        }
    }

    private void OnDrawGizmosSelected() // Això serveix per poder veure el rang d'atac desde l'escena (no en el joc)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
