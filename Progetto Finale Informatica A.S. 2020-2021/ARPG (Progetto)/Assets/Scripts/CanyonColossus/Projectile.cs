using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f; //velocità del proiettile
    public int damage = 1;  //danno provocato
    public Rigidbody2D rigidBody; //componente assegnata al proiettile a cui vengono applicate delle forze

    //si assegna al proiettile una velocità e si distrugge il proiettile 0.4 secondi dopo che viene istanziato
    void Start()
    {
        rigidBody.velocity = transform.right * -speed;
        Invoke("Destroy", 0.4f);
    }

    //se collide con un collider si controlla che sia un giocatore controllando se al suo genitore è assegnato lo script PlayerCombat
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.GetComponentInParent<PlayerCombat>() != null)
        {
            hitInfo.GetComponentInParent<PlayerCombat>().TakeDamage(damage); //si applica danno attraverso un metodo dello script PlayerCombat
            Destroy(gameObject); //il proiettile viene distrutto
        }
    }

    void Destroy()
    {
        Destroy(gameObject); //distrugge il proiettile
    }
}
