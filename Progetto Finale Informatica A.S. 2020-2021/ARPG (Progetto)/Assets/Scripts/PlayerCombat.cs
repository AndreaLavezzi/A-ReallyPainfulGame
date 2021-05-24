using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    #region Public Variables
    public Animator playerAnimator;
    public Animator healthBarAnimator;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 1f;
    public float nextAttackTime = 0f;

    public int maxHealth = 20;
    #endregion

    #region Private Variables
    private int currentHealth;
    CharacterController2D characterController2D;
    #endregion

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
                playerAnimator.SetTrigger("Attack");
                //Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            TakeDamage(1);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            TakeDamage(5);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            TakeDamage(-1);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            TakeDamage(-5);
        }
    }

    // Riproduce un'animazione d'attacco, individua i nemici nel suo raggio e applica loro del danno
    public void Attack()
    {
        //playerAnimator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        Debug.Log(hitEnemies);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponentInParent<Enemy>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        healthBarAnimator.SetInteger("Health", currentHealth);
        healthBarAnimator.SetTrigger("HealthChange");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
