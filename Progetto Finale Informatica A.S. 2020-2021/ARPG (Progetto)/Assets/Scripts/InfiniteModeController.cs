using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfiniteModeController : MonoBehaviour
{
    #region Public Variables
    public Transform enemyPrefab;
    public GameObject playerPrefab;
    public Transform rightWall;
    public Transform leftWall;
    public TextMeshProUGUI killedEnemiesTXT;
    public float spawnRange = 12f;
    [HideInInspector] public int killedEnemies = 0;
    #endregion

    #region Private Variables
    GameObject player;
    #endregion

    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(25, 2), Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponentInChildren<TextMeshProUGUI>().text = "Uccisioni: " + killedEnemies;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            EnemySpawn();
        }
    }

    void EnemySpawn()
    {
        for(int i = 0; i < Random.Range(1, 6); i++)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 playerDirection = player.transform.forward;
            Quaternion playerRotation = player.transform.rotation;
            Vector3 spawnDistance;
            spawnDistance.x = Random.Range(spawnRange, -spawnRange);
            spawnDistance.y = 0;
            spawnDistance.z = 0;
            if(playerPos.x + spawnDistance.x > rightWall.position.x || playerPos.x + spawnDistance.x < leftWall.position.x)
            {
                spawnDistance.x = spawnDistance.x * -1;
            }            
            Vector3 spawnPos = playerPos + playerDirection + spawnDistance;
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}
