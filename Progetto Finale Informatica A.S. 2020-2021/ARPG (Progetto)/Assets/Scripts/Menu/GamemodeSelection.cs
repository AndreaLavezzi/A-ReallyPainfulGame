using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GamemodeSelection : MonoBehaviour
{
    public int selectedGamemodeIndex = 0; //indice della modalità selezionata

    [Header("Sounds")]
    [SerializeField] private AudioClip ClickSFX; //effetto sonoro riprodotto alla pressione di alcuni bottoni

    //evento le cui azioni vengono decise nell'inspector di Unity, attiverà il pannello di selezione del personaggio e disattiverà quello delle modalità di gioco
    public UnityEvent ValidChoice; 

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        if (ValidChoice == null)
            ValidChoice = new UnityEvent();
    }

    //in base al bottone cliccato l'indice assumerà il valore corrispondente all'indice della scena a cui si vuole passare
    public void InvasionModeSelected()
    {
        selectedGamemodeIndex = 1;
        AudioManager.Instance.PlaySFX(ClickSFX);
        ValidChoice.Invoke();
    }
    public void AdventureModeSelected()
    {
        selectedGamemodeIndex = 2;
        AudioManager.Instance.PlaySFX(ClickSFX);
        ValidChoice.Invoke();
    }
}
