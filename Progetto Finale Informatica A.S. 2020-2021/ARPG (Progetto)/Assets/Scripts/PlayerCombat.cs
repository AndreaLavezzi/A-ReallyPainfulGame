using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator healthBarAnimator;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 1f;
    public float nextAttackTime = 0f;

    public int maxHealth = 20;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            TakeDamage(1);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            TakeDamage(5);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            TakeDamage(-1);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            TakeDamage(-5);
        }
    }

    // Riproduce un'animazione d'attacco, individua i nemici nel suo raggio e applica loro del danno
    void Attack()
    {
        playerAnimator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = maxHealth;
        }
        healthBarAnimator.SetInteger("Health", currentHealth);
        healthBarAnimator.SetTrigger("HealthChange");
    }
}
