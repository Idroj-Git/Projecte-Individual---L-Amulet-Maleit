using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float speed;
    [SerializeField] int strength;
    private BattleManager battleManager;
    [SerializeField] Transform target;

    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        battleManager = FindObjectOfType<BattleManager>();
        battleManager.SetEnemiesAlive(battleManager.GetEnemiesAlive() + 1);
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    public void TakeDamage(int dmg)
    {
        this.health -= dmg;
        Debug.Log("*enemy says* Ouch");
        if (health <= 0)
        {
            battleManager.SetEnemiesAlive(battleManager.GetEnemiesAlive() - 1);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerSettings player = collision.collider.GetComponent<PlayerSettings>();
            if (player != null)
            {
                player.TakeDamage(strength);
            }
        }
    }
}
