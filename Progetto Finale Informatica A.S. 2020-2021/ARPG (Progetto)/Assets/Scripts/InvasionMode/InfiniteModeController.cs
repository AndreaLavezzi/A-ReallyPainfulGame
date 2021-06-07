using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfiniteModeController : MonoBehaviour
{
    #region Public Variables
    public GameObject playerPrefab; //prefab del giocatore
    public GameObject menuPanel;    //pannello del menu
    public Transform enemyPrefab;   //prefab del nemico
    public Transform rightWall;
    public Transform leftWall;
    public float spawnRange = 12f;  //distanza di spawn dei nemici dal giocatore
    [HideInInspector] public int killedEnemies = 0; //contatore delle uccisioni
    #endregion

    #region Private Variables
    GameObject player;  //variabile del giocatore
    TextMeshProUGUI killedEnemiesTXT; //oggetto che mostra le uccisioni
    #endregion

    //viene istanziato il giocatore e ridotto il suo campo visivo
    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(25, 2), Quaternion.identity) as GameObject;
        player.GetComponentInChildren<Camera>().fieldOfView = 0.8f;
        killedEnemiesTXT = player.GetComponentInChildren<TextMeshProUGUI>();
    }

    
    void Update()
    {
        killedEnemiesTXT.text = "Uccisioni: " + killedEnemies;

        //se non ci sono nemici ne vengono spawnati
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            EnemySpawn();
        }

        //se si preme ESC si ferma lo scorrere del tempo e si attiva un pannello che chiede se si vuole andare al menu principale
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0;
        }
        //se il pannello non è attivo il tempo torna alla normalità
        if (!menuPanel.activeSelf)
        {
            Time.timeScale = 1;
        }
    }

    //spawna i nemici
    void EnemySpawn()
    {
        //il ciclo viene ripetuto per un numero di volte random
        for (int i = 0; i < Random.Range(1, 6); i++)
        {
            Vector3 playerPos = player.transform.position; //si acquisisce la posizione del giocatore
            Vector3 playerDirection = player.transform.forward; //la sua direzione
            Quaternion playerRotation = player.transform.rotation; //la sua rotazione
            Vector3 spawnDistance;
            spawnDistance.x = Random.Range(spawnRange, -spawnRange); //posizione è compresa tra i due valori, positivo e negativo, del raggio di spawn dal giocatore
            spawnDistance.y = 0;
            spawnDistance.z = 0;

            //se il nemico sarebbe dovuto spawnare fuori dai bordi della mappa allora il segno della sua posizione viene invertito
            if (playerPos.x + spawnDistance.x > rightWall.position.x || playerPos.x + spawnDistance.x < leftWall.position.x)
            {
                spawnDistance.x = spawnDistance.x * -1;
            }            

            Vector3 spawnPos = playerPos + playerDirection + spawnDistance; //sommando le posizioni ottenute si ottiene la posizione di spawn finale
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity); //si istanzia il nemico in quella posizione
        }
    }

    //porta alla scena del menu
    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
