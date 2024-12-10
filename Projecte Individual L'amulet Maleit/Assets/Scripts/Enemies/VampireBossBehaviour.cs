using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class VampireBossBehaviour : EnemyController
{
    // Start is called before the first frame update
    private float rushCooldown = 10f, rushTimer;
    private Vector3 rushTargetPosition;
    private bool canUseRushAttack
    {
        get { return rushTimer < 0 && !isUsingRushAttack; } // Si el rush timer ha acabat es pot utilitzar
    }
    private bool isUsingRushAttack = false;

    protected override void Start()
    {
        base.Start();
        rushTimer = rushCooldown;
        rushTargetPosition = target.position; // per si de cas.
        strength = Mathf.FloorToInt(RuntimeGameSettings.Instance.GetMaxHealth() * 0.15f); // Cada cop és el 15% de vida del jugador arrodonit cap a baix.
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        rushTimer -= Time.deltaTime;
    }

    protected override void MoveEnemy() // Es crida sempre i quan no estigui stunned.
    {
        if (target != null && canMove)
        {
            /*if (speed > 0 && canUseRushAttack)
            {
                StartCoroutine(RushAttack());
            }
            else 
            if (isUsingRushAttack)
            {
                Vector2 direction = (rushTargetPosition - transform.position).normalized;
                if (!isKnockbacked)
                    lastDirection = direction;

                if (isKnockbacked)
                    rb.velocity = lastDirection * speed; // es tira endarrera amb la direcció anterior.
                else
                    rb.velocity = direction * speed;
            }
            else
            {*/
            Vector2 direction = (target.position - transform.position).normalized;
                if (!isKnockbacked)
                    lastDirection = direction;

                if (isKnockbacked)
                    rb.velocity = lastDirection * speed; // es tira endarrera amb la direcció anterior.
                else
                    rb.velocity = direction * speed;
            //}
        }
    }

    private IEnumerator RushAttack() // No funciona el atac especial ;-;
    {
        if (isUsingRushAttack) yield break; // Evita començar multiples corroutine.

        isUsingRushAttack = true;
        rushTimer = rushCooldown;
        stunTimer = 0.6f;
        rushTargetPosition = target.position;
        Debug.Log(rushTargetPosition);
        //yield return new WaitForSeconds(0.5f); // Espera per deixar que el jugador s'aparti.
        speed *= 100;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, rushTargetPosition) < 0.1f);

        rushTimer = rushCooldown;
        isUsingRushAttack = false;
        speed = maxSpeed;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerSettings player = collision.collider.GetComponent<PlayerSettings>();
            if (player != null)
            {
                player.TakeDamage(strength);
                health = Mathf.Min(health + strength, maxHealth); // ES CURA EL MAL QUE FA
                //if (health + strength > maxHealth)    És el mateix però sense utilitzar Matf
                //    health = maxHealth;
                //else
                //    health += strength;
                healthbar.SetHealth(health, maxHealth);
                GetKnockbacked(strength);
            }
        }
    }

    protected override void StartStun()
    {
        stunTimer = 0.1f;
        isKnockbacked = false;
    }
}
