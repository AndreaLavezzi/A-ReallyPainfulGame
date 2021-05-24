using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanyonColossusController : MonoBehaviour
{
    #region Public Variables
    public GameObject colossalCore;
    public GameObject playerPrefab;
    public GameObject menuPanel;
    #endregion

    #region Private Variables
    GameObject player;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(colossalCore, new Vector3(-27, -2), Quaternion.identity);
        player = Instantiate(playerPrefab, new Vector3(-21, -4.3f), Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < -10)
        {
            player.transform.position = new Vector3(-21, -4);
            player.GetComponent<Animator>().SetBool("IsJumping", false);
            player.GetComponent<PlayerCombat>().TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(true);
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
