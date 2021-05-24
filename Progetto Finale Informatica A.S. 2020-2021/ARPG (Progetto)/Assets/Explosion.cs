using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    bool alreadyHit;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Animator>().SetTrigger("Explode");
        Invoke("Destroy", GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);
        alreadyHit = false;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        
        if (hitInfo.GetComponentInParent<PlayerCombat>() != null && alreadyHit == false)
        {
            hitInfo.GetComponentInParent<PlayerCombat>().TakeDamage(3);
            alreadyHit = true;
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
