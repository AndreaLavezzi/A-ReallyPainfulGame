using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossalCoreController : MonoBehaviour
{
    //il nucleo viene distrutto automaticamente dopo 15 secondi
    void Start()
    {
        Invoke("Destroy", 15);
    }

    void Destroy()
    {
        GetComponentInParent<Enemy>().Die(); //viene richiamato dallo script Enemy il metodo Die che distrugge il nucleo
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //vengono trovati tutti i giocatori
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(10); //e viene applicato loro del danno
        }
    }
}
