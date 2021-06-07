using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CanyonColossusController : MonoBehaviour
{
    #region Public Variables
    public GameObject playerPrefab; //riferimento al giocatore
    public GameObject menuPanel;    //pannello per tornare al menu
    #endregion

    #region Private Variables
    GameObject player; //variabile del giocatore
    #endregion

    //viene istanziato il giocatore e viene disattivata la componente che mostra le uccisioni (inutile in questa modalità)
    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(-21, -4.3f), Quaternion.identity) as GameObject;
        player.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
    }

    void Update()
    {
        //se il giocatore cade viene applicato del danno e viene riportato su una delle piattaforme
        if (player.transform.position.y < -10)
        {
            player.transform.position = new Vector3(-21, -4);
            player.GetComponent<Animator>().SetBool("IsJumping", false);
            player.GetComponent<PlayerCombat>().TakeDamage(1);
        }

        //se viene cliccato il pulsante ESC viene fermato lo scorrere del tempo e attivato il pannello che riporta al menu principale
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0;
        }
        //se il pannello non è attivo il tempo viene riportato alla normalità
        if (!menuPanel.activeSelf)
        {
            Time.timeScale = 1;
        }
    }

    //metodo che riporta al menu
    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
