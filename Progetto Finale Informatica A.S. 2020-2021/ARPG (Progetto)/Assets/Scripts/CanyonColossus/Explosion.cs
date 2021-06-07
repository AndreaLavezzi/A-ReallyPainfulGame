using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    bool alreadyHit; //evita che il giocatore venga danneggiato più volte    

    //viene riprodotta un'animazione e viene invocata funzione Destroy dopo che l'animazione è finita
    void Start()
    {
        GetComponentInChildren<Animator>().SetTrigger("Explode");
        Invoke("Destroy", GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);
        alreadyHit = false;
    }

    //se qualcosa collide con l'esplosione viene controllato che sia un giocatore, e se lo è viene applicato del danno
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.GetComponentInParent<PlayerCombat>() != null && alreadyHit == false)
        {
            hitInfo.GetComponentInParent<PlayerCombat>().TakeDamage(3);
            alreadyHit = true;
        }
    }

    //distrugge il gameObject padre di questo script
    void Destroy()
    {
        Destroy(gameObject);
    }
}
