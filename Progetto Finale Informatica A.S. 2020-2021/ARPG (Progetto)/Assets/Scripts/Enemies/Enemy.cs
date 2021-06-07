using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Public Variables
    public Animator animator; //riferimento alla componente che dà un'animazione al nemico
    public GameObject healthBarPrefab; //riferimento al prefab della barra della vita

    //i primi due valori sono facilmente modificabili dall'inspector di Unity
    public float maxHealth = 100; //vita massima
    public float destructionTime = 15; //tempo di distruzione dell'oggetto dopo la morte
    public float currentHealth; //vita attuale
    #endregion

    #region Private Variables
    GameObject childHealthBar; //variabile contenente la barra della vita figlia dell'oggetto contenente questo script
    Image healthBarFill;    //riempimento della barra della vita

    bool isDead; //indica se il nemico è morto o no
    #endregion

    //viene istanziata e nascosta la barra della vita, viene assegnata il valore della vita massima alla vita attuale
    private void Start()
    {
        isDead = false;
        childHealthBar = Instantiate(healthBarPrefab, gameObject.transform) as GameObject;
        childHealthBar.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, -6f);
        childHealthBar.SetActive(false);
        healthBarFill = childHealthBar.GetComponentInChildren<Image>();
        currentHealth = maxHealth;
    }

    //applica danno al nemico
    public void TakeDamage(int damage)
    {
        if(isDead == false)
        {
            if (currentHealth == maxHealth) //prima di applicare danno per la prima volta attiva la barra della vita
            {
                childHealthBar.SetActive(true);
            }

            currentHealth -= damage; //diminuisce la vita attuale
            healthBarFill.fillAmount = currentHealth / maxHealth; //il riempimento della barra della vita corrisponde al rapporto tra vita attuale e vita massima
            if (animator != null) //se esiste una componente animator viene riprodotta un'animazione
            {
                animator.SetTrigger("Hurt");
            }

            if (currentHealth <= 0) //se la vita attuale è minore o uguale a 0, il nemico muore
            {
                childHealthBar.SetActive(false);
                Die();
            }
        }
    }

    public void Die()
    {
        if(isDead == false)
        {
            isDead = true;

            float animDuration = 0;
            if (animator != null)
            {
                animator.SetBool("IsDead", true);
                animDuration = animator.GetCurrentAnimatorStateInfo(0).length;
            }

            Invoke("Destroy", destructionTime + Convert.ToInt32(animDuration)); /*il gameObject del nemico viene distrutto in base a quanto tempo è deciso 
                                                                                  dalla variabile destructionTime, dopo aver finito di riprodurre l'animazione
                                                                                  di morte*/
        }
        
    }

    public void Destroy()
    {
        Destroy(transform.root.GetComponentInParent<Enemy>().gameObject);
    }

    
    
}
