using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float speed;
    private float maxSpeed, maxHealth;
    [SerializeField] int strength;
    private BattleManager battleManager;
    [SerializeField] Transform target;
    private bool canMove = true;

    private Rigidbody2D rb;
    [SerializeField] HealthbarBehaviour healthbar;

    private float stunTimer = 0f, baseKnockback = 2;
    private bool isKnockbacked = false;
    private Vector2 lastDirection;
    private bool isStunned
    {
        get { return stunTimer > 0f; }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        battleManager = FindObjectOfType<BattleManager>();
        battleManager.SetEnemiesAlive(battleManager.GetEnemiesAlive() + 1);
        if (target == null)
        {
            target = FindAnyObjectByType<PlayerMovement>().transform;
        }
        maxSpeed = speed;
        maxHealth = health;
        healthbar.SetHealth(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned)
        {
            MoveEnemy();
        }


        if (isKnockbacked && speed < 0)
        {
            speed += Time.deltaTime * 2f;
            if (speed >= 0)
            {
                speed = 0;
                StartStun();
            }
        }
        else
        {
            speed = maxSpeed;
        }

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        }
    }

    private void MoveEnemy()
    {
        if (target != null && canMove)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            if (!isKnockbacked)
                lastDirection = direction;

            if (isKnockbacked)
                rb.velocity = lastDirection * speed; // es tira endarrera amb la direcció anterior.
            else
                rb.velocity = direction * speed;
        }
    }

    public void TakeDamage(int dmg)
    {
        this.health -= dmg;
        healthbar.SetHealth(health, maxHealth);
        GetKnockbacked(dmg);
        //Debug.Log("*enemy says* Ouch");
        Debug.Log("With speed" + speed + " resulting in " + -speed + " speed");
        if (health <= 0)
        {
            battleManager.SetEnemiesAlive(battleManager.GetEnemiesAlive() - 1);
            Destroy(gameObject);
        }
    }

    private void GetKnockbacked(float knockbackForce)
    {
        if (!isKnockbacked)
        {
            isKnockbacked = true;
            stunTimer = 0f;
            speed = -speed * baseKnockback * (knockbackForce / 100); // mini augment depenent de la força
            Debug.Log("Knockback with " + knockbackForce + " With speed  " + speed +  " resulting in " + knockbackForce * -speed + " speed");
        }
    }

    private void StartStun()
    {
        stunTimer = 0.2f;
        isKnockbacked = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerSettings player = collision.collider.GetComponent<PlayerSettings>();
            if (player != null)
            {
                player.TakeDamage(strength);
                GetKnockbacked(strength);
            }
        }
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public float getActualHealth()
    {
        return this.health;
    }

    public float getMaxHealth()
    {
        return this.maxHealth;
    }
}
