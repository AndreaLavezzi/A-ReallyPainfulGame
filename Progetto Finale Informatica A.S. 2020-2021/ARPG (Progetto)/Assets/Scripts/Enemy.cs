using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public GameObject healthBarPrefab;
    public float maxHealth = 100;
    public float currentHealth;
    public float destructionTime = 15;
    private bool isDead = false;
    GameObject childHealthBar;
    Image healthBarFill;

    // Start is called before the first frame update
    private void Start()
    {
        childHealthBar = Instantiate(healthBarPrefab, gameObject.transform) as GameObject;
        childHealthBar.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, -6f);
        childHealthBar.SetActive(false);
        healthBarFill = childHealthBar.GetComponentInChildren<Image>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth == maxHealth)
        {
            childHealthBar.SetActive(true);
        }

        currentHealth -= damage;
        healthBarFill.fillAmount = currentHealth / maxHealth;
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        if(currentHealth <= 0)
        {
            childHealthBar.SetActive(false);
            Die();
        }
    }

    public void Die()
    {
        if(isDead == false)
        {
            isDead = true;
            Debug.Log("Il nemico è stato sconfitto.");
            //GameObject.Find("InfiniteModeControllerOBJ").GetComponent<InfiniteModeController>().killedEnemies++;
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            float animDuration = 0;
            if (animator != null)
            {
                animator.SetBool("IsDead", true);
                animDuration = animator.GetCurrentAnimatorStateInfo(0).length;
            }

            //GetComponentInChildren<Collider2D>().enabled = false;
            //GetComponentInChildren<CircleCollider2D>().enabled = false;
            //this.enabled = false;
            Invoke("Destroy", destructionTime + Convert.ToInt32(animDuration));
        }
        
    }

    public void Destroy()
    {
        Destroy(transform.root.GetComponentInParent<Enemy>().gameObject);
    }

    
    
}
