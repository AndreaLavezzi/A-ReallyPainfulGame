using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    #region Public Variables
    public Animator playerAnimator; //riferimento alla componente che anima lo sprite del giocatore
    public Animator healthBarAnimator;  //riferimento alla componente che anima la barra della vita del giocatore

    public Transform attackPoint;   //punto di attacco
    public LayerMask enemyLayers;   //layer dei nemici

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 1f;
    public float nextAttackTime = 0f;

    public int maxHealth = 20;
    #endregion

    #region Private Variables
    private int currentHealth;
    CharacterController2D characterController2D;    //riferimento allo script che controlla il movimento del personaggio
    #endregion

    void Start()
    {
        currentHealth = maxHealth; 
    }

    void Update()
    {
        if(Time.time >= nextAttackTime) //se può attaccare
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) //se viene cliccato il tasto sinistro
            {
                playerAnimator.SetTrigger("Attack"); //viene riprodotta l'animazione di attacco
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(-1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }
    }

    //durante l'animazione d'attacco, individua i nemici nel suo raggio e applica loro del danno
    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponentInParent<Enemy>().TakeDamage(attackDamage);
        }
    }

    //applica danno al giocatore
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) //se il giocatore muore resetta la scena
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //cambia l'animazione della barra della vita in base alla vita attuale
        healthBarAnimator.SetInteger("Health", currentHealth); 
        healthBarAnimator.SetTrigger("HealthChange");
    }
}
