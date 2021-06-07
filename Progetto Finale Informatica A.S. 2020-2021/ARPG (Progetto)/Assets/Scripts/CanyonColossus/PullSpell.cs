using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullSpell : MonoBehaviour
{
    float velocity = 0.0f;
    float posY; //altezza del giocatore nel momento in cui viene attirato

    //viene rilevato il giocatore e si assegna qual è l'altezza a cui deve essere attirato il giocatore
    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        posY = players[0].transform.position.y;
        Invoke("Destroy", 0.4f); //l'attrazione dura 0.4 secondi
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players) //viene trascinato il giocatore verso un obiettivo in un certo quantitativo di tempo
        {
            float newPosX = Mathf.SmoothDamp(player.transform.position.x, gameObject.GetComponentInParent<Transform>().position.x, ref velocity, 0.2f);
            player.transform.position = new Vector3(newPosX, posY, player.transform.position.z);
        }
    }

    //distrugge il gameObject parente di questo script
    void Destroy()
    {
        Destroy(gameObject);
    }
}
