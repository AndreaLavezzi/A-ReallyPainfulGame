using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfiniteModeController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int killedEnemies = 0;
    // Update is called once per frame
    void Update()
    {
        text.text = "Uccisioni: " + killedEnemies;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    //public void EnemyKilled()
    //{
    //    killedEnemies++;
    //}
}
