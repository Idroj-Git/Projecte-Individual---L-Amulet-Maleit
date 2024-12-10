using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    protected float maxSpeed;
    protected int maxHealth;
    [SerializeField] protected int strength;
    private BattleManager battleManager;
    [SerializeField] protected Transform target;
    protected bool canMove = true;

    protected Rigidbody2D rb;
    [SerializeField] protected HealthbarBehaviour healthbar;
    [SerializeField] ParticleSystem damagedParticleSystem;
    protected float stunTimer = 0f, baseKnockback = 4f;
    protected bool isKnockbacked = false;
    protected Vector2 lastDirection;
    protected bool isStunned
    {
        get { return stunTimer > 0f; }
    }


    // Start is called before the first frame update
    protected virtual void Start()
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
        //SpriteRenderer slimeRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log($"Slime position: {transform.position}, Scale: {transform.localScale}, Sorting Layer: {slimeRenderer.sortingLayerName}");

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isStunned)
        {
            MoveEnemy();
        }


        if (isKnockbacked && speed < 0)
        {
            speed += Time.deltaTime * 5f;
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

    protected virtual void MoveEnemy()
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
        damagedParticleSystem.Play();
        GetKnockbacked(dmg);
        //Debug.Log("*enemy says* Ouch");
        //Debug.Log("With speed" + speed + " resulting in " + -speed + " speed");
        if (health <= 0)
        {
            battleManager.SetEnemiesAlive(battleManager.GetEnemiesAlive() - 1);
            Destroy(gameObject);
        }
    }

    protected virtual void GetKnockbacked(float knockbackForce)
    {
        if (!isKnockbacked)
        {
            isKnockbacked = true;
            stunTimer = 0f;
            speed=maxSpeed;
            if (gameObject.CompareTag("VampireBoss"))
                speed = -maxSpeed * (knockbackForce / 100);
            else
                speed = -maxSpeed * baseKnockback * (knockbackForce / 100); // mini augment depenent de la força
            //Debug.Log("Knockback with " + knockbackForce + " With speed  " + speed +  " resulting in " + knockbackForce * -speed + " speed");
        }
    }

    protected virtual void StartStun()
    {
        stunTimer = 0.2f;
        isKnockbacked = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
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

    public int getMaxHealth()
    {
        return this.maxHealth;
    }
}
