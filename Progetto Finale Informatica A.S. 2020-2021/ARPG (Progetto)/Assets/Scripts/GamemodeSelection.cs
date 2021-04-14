using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeSelection : MonoBehaviour
{
    private int selectedGamemodeIndex;
    
    [Header("List of gamemodes")]
    [SerializeField] private List<GamemodeSelectObject> gamemodeList = new List<GamemodeSelectObject>();

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI gamemodeName;
    [SerializeField] private Image gamemodeSplash;

    [Header("Sounds")]
    [SerializeField] private AudioClip arrowClickSFX;

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
        Debug.Log(string.Format($"E' stata scelta la modalità {gamemodeList[selectedGamemodeIndex].gamemodeName}"));
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
