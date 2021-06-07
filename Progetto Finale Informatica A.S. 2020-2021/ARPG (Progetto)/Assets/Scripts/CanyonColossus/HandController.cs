using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HandController : MonoBehaviour
{
    #region Public Variables
    //riferimenti agli oggetti di gioco
    public GameObject colossalCorePrefab; 
    public GameObject projectilePrefab;
    public GameObject pushSpellPrefab;
    public GameObject explosionPrefab;  
    public GameObject endScreen;    //schermata che apparirà alla sconfitta del boss
    public GameObject coreWarning;  //sritta che avverte il giocatore riguardo l'apparizione del nucleo

    public ScoreController scoreController; //riferimento allo script che controlla la classifica
    #endregion

    #region Private Variables
    GameObject[] player;    //riferimento al giocatore
    float nextAttackTime;   //quantità di tempo che manca al prossimo tentativo di attacco del boss
    float cooldown; //quantità di tempo che intercorre tra due tentativi di attacco del boss
    bool coreSpawned = false;   //indica se il nucleo sia stato spawnato o meno
    bool enraged = false; //se il boss è a meno di 1/3 della sua vita esso guadagna due attacchi aggiuntivi
    #endregion

    void Start()
    {
        cooldown = 2;
        nextAttackTime = cooldown;
    }
   
    void Update()
    {
        if (nextAttackTime > 0)
        {
            nextAttackTime -= Time.deltaTime; //Viene diminuita nel tempo la quantità di tempo che manca al prossimo tentativo di attacco del boss
        }
        else
        {
            Attack();   //se la quantità di tempo mancante arriva a 0, si ha un tentativo di attacco
        }

        //vengono ottenuti la vita massima e corrente del boss, e se quest'ultima è la metà di quella massima allora viene istanziato il nucleo
        if (this.GetComponentInParent<Enemy>().maxHealth / 2 >= this.GetComponentInParent<Enemy>().currentHealth && !coreSpawned)
        {
            Instantiate(colossalCorePrefab, new Vector3(-27, -2), Quaternion.identity);
            coreSpawned = true;
            coreWarning.SetActive(true);
            Invoke("DestroyObj", 5); //richiama la funzione DestroyObj che distruggerà il messaggio di avvertimento
        }
    }

    //controlla il pattern di attacchi del boss
    void Attack()
    {
        player = GameObject.FindGameObjectsWithTag("Player");   //popola l'array di giocatori in base al tag assegnato nell'inspector di Unity
        float distance = Mathf.Abs(player[0].transform.position.x - this.gameObject.transform.position.x);  //viene calcolata la distanza tra giocatore e boss
        int prob = Random.Range(0, 10); //viene estratto un numero random

        //se il boss è a 1/3 della sua vita o meno esso si "arrabbia"
        if (this.GetComponentInParent<Enemy>().maxHealth/3 >= this.GetComponentInParent<Enemy>().currentHealth && enraged == false)
        {
            enraged = true;
        }
        if (distance < 7 && prob <= 3)  //se la distanza è minore di 7 e il numero è minore o uguale a 3 (36%)
        {
            Push(); //il giocatore viene respinto
        }
        if (distance < 7 && prob > 7) //se la distanza è minore di 7 e il numero è maggiore a 7 (27%)
        {
            Explode();  //il boss esegue un attacco esplosivo
        }
        if (distance < 12 && distance >= 7 && prob < 3) //se la distanza è minore di 12 e maggiore o uguale a 7 e il è numero minore a 3 (27%)
        {
            Fire(); //il boss spara un proiettile
        }
        if(distance < 20 && distance > 12 && enraged)   //se il boss è arrabbiato e la distanza è tra 20 e 12...
        {
            if(Random.Range(0,1) == 0)  //viene estratto un nuovo numero random, che attiverà con la stessa probabilità uno dei due attacchi aggiuntivi
            {
                Pull_Fire();    //il boss attira a sè il giocatore e spara dei proiettili
            }
            else
            {
                Pull_Explode(); //il boss attira a sè il giocatore e crea un'esplosione
            }
        }
        nextAttackTime = cooldown;  //si reimposta il tempo per il prossimo tentativo di attacco a quello iniziale
    }

    //viene istanziato un oggetto che contiene uno script che attira il giocatore alla posizione del boss
    void Pull()
    {
        GameObject pullSpellCast = Instantiate(pushSpellPrefab, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Quaternion.identity) as GameObject;
    }

    //come sopra, ma viene istanziato lontano dal boss, e crea un effetto che sembra come se il giocatore venisse respinto via
    void Push()
    {
        GameObject pushSpellCast = Instantiate(pushSpellPrefab, new Vector3(this.gameObject.transform.position.x - 30, this.gameObject.transform.position.y), Quaternion.identity) as GameObject;
    }

    //viene istanziato un proiettile che avrà uno script assegnato
    void Fire()
    {
        Instantiate(projectilePrefab, new Vector3(transform.position.x - 3, transform.position.y, -6f), Quaternion.identity);
    }

    //viene istanziata un'esplosione che avrà uno script assegnato
    void Explode()
    {
        GameObject explosionSpellCast = Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, -6f), Quaternion.identity) as GameObject;
    }

    //viene decisa randomicamente la quantità di proiettili da sparare, quindi vengono invocate in successione le funzioni Pull (una volta) e Fire (per una quantità di volte decisa dal random) 
    void Pull_Fire()
    {
        int projAmount = Random.Range(1, 4);
        Invoke("Pull", 0);
        for(float i = 0.2f; i < projAmount * 0.2f; i = i + 0.2f) //ogni proiettile viene istanziato con 0.2 secondi di delay l'uno dall'altro
        {
            Invoke("Fire", i);
        }       
    }

    //viene invocata la funzione Pull e, dopo un leggero delay, la funzione Explode
    void Pull_Explode()
    {
        Invoke("Pull", 0);
        Invoke("Explode", 0.3f);
    }

    //alla distruzione del boss
    private void OnDestroy()
    {
        Time.timeScale = 0; //viene fermato lo scorrere del tempo, in modo che il giocatore non si possa muovere o attaccare
        if(this.gameObject.GetComponent<Enemy>().currentHealth <= 0)    //se la distruzione è causata dalla sconfitta del boss (quindi la sua vita è arrivata a 0)
        {
            string loggedPlayerPath = Application.persistentDataPath + "/Logged_Player.txt";    //viene acquisito il file che contiene il giocatore loggato
            var data = File.ReadAllLines(loggedPlayerPath); //e viene immagazzinato il suo contenuto in una variabile

            //si prova a modificare il punteggio del giocatore nella classifica, ma nel caso si dovesse avere un errore non verrà modificata
            try
            {
                scoreController.AddScore(data[0], 1, true); 
            }
            catch   
            {
                endScreen.SetActive(true); //si visualizza la schermata finale nel caso si dovesse incorrere in un errore
            }
            endScreen.SetActive(true); //e anche nel caso tutto vada bene
        }
    }

    void DestroyObj() //viene distrutto il messaggio di avvertimento
    {
        Destroy(coreWarning);
    }

}
