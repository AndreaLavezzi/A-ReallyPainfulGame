using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip buttonClickSFX;

    public void PlayClickSFX()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX);
    }

    public void ChiudiGioco()
    {
        Debug.Log("Hai chiuso il gioco!");
        Application.Quit();
    }
}
