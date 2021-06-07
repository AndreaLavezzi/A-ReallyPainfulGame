using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClickSFX; //effetto sonoro riprodotto alla pressione di alcuni tasti

    //ogni funzione è assegnata a un bottone dall'inspector di unity
    public void PlayClickSFX()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX);
    }

    public void CloseGame()
    {
        Debug.Log("Hai chiuso il gioco!");
        Application.Quit();
    }

    public void ToLeaderbord()
    {
        SceneManager.LoadScene(3);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
