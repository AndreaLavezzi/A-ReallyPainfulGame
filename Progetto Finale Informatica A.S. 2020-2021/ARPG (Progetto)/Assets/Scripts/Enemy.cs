using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    InfiniteModeController infiniteModeController;
    public Animator animator;

    public int maxHealth = 100;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        infiniteModeController = GetComponent<InfiniteModeController>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Il nemico è stato sconfitto.");
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        this.enabled = false;
        Invoke("SpriteDisable", 15);
    }

    void SpriteDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    
}
