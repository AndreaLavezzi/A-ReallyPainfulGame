using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GamemodeSelection : MonoBehaviour
{
    public int selectedGamemodeIndex;
    
    [Header("List of gamemodes")]
    [SerializeField] private List<GamemodeSelectObject> gamemodeList = new List<GamemodeSelectObject>();

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI gamemodeName;
    [SerializeField] private Image gamemodeSplash;

    [Header("Sounds")]
    [SerializeField] private AudioClip arrowClickSFX;

    public UnityEvent ValidChoice;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        if (ValidChoice == null)
            ValidChoice = new UnityEvent();
    }

    private void Start()
    {
        UpdateGamemodeSelectionUI();
    }

    public void LeftArrow()
    {
        selectedGamemodeIndex--;
        if (selectedGamemodeIndex < 0)
        {
            selectedGamemodeIndex = gamemodeList.Count - 1;
        }

        UpdateGamemodeSelectionUI();
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }
    public void RightArrow()
    {
        selectedGamemodeIndex++;
        if (selectedGamemodeIndex == gamemodeList.Count)
        {
            selectedGamemodeIndex = 0;
        }

        UpdateGamemodeSelectionUI();
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }

    public void Confirm()
    {
        if(selectedGamemodeIndex == 1)
        {
            Debug.Log("La modalità selezionata non è ancora stata implementata.");
        }
        else
        {
            ValidChoice.Invoke();
        }
    }

    private void UpdateGamemodeSelectionUI()
    {
        //Cambierà l'immagine e il nome della modalità
        gamemodeSplash.sprite = gamemodeList[selectedGamemodeIndex].splash;
        gamemodeName.text = gamemodeList[selectedGamemodeIndex].gamemodeName;
    }

    [System.Serializable]
    public class GamemodeSelectObject
    {
        public Sprite splash;
        public string gamemodeName;
    }
}
