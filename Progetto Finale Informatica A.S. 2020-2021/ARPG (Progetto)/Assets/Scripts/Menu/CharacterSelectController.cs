using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    private int selectedCharacterIndex; //indice nella lista del personaggio selezionato
    public GameObject gamemodeSelection; //riferimento al gameObject del pannello in cui si seleziona la modalità

    [Header("List of characters")]
    [SerializeField] private List<CharacterSelectObject> characterList = new List<CharacterSelectObject>(); //contiene i personaggi disponibili
    
    [Header("UI References")]
    [SerializeField] private Image characterSplash; //immagine del personaggio

    [Header("Sounds")]
    [SerializeField] private AudioClip arrowClickSFX; //suono riprodotto al clic di alcuni bottoni

    private void Start()
    {
        UpdateCharacterSelect();
    }

    //si aggiornano le informazioni mostrate in base al personaggio selezionato
    public void UpdateCharacterSelect()
    {
        characterSplash.sprite = characterList[selectedCharacterIndex].splash;
    }

    //soluzione non delle migliori ma per ora funziona, in base a quale bottone si preme l'indice cambia
    public void BanditSelected() 
    {
        selectedCharacterIndex = 0;
        UpdateCharacterSelect();
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }
    public void ArcherSelected()
    {
        selectedCharacterIndex = 1;
        UpdateCharacterSelect();
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }
    public void MageSelected()
    {
        selectedCharacterIndex = 2;
        UpdateCharacterSelect();
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }

    //se si preme il bottone di conferma si passa alla modalità di gioco scelta
    public void ConfirmSelection()
    {
        //dato che alcuni dei personaggi non sono ancora stati implementati non si potrà proseguire se si sceglie uno di quelli
        if (selectedCharacterIndex == 1 || selectedCharacterIndex == 2) 
        {
            Debug.Log("Il personaggio selezionato non è ancora stato implementato.");
        }
        else
        {
            SceneManager.LoadScene(gamemodeSelection.GetComponent<GamemodeSelection>().selectedGamemodeIndex);
        }
    }

    [System.Serializable] //si fa in modo che l'immagine dei personaggi si possa inserire direttamente dall'inspector di unity
    public class CharacterSelectObject
    {
        public Sprite splash;
    }
}