using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterLoginSystem : MonoBehaviour
{
    #region Public Variables
    //riferimenti a vari oggetti dell'user interface
    public TMP_InputField loginUsernameIPT, loginPasswordIPT, registerUsernameIPT, registerPasswordIPT, registerConfirmPasswordIPT;
    public TextMeshProUGUI errorMSG, loggedPlayerText;
    public Button backFromLogin, backFromRegister;
    public GameObject login, register, logout;
    #endregion

    #region Private Varibales
    bool loggedIn = false; //indica se un giocatore è loggato o meno
    string loggedPlayerPath, leaderboardPath, accountsPath; //percorsi dei vari file
    #endregion

    //appena viene avviato il gioco controlla se esistono i file e nel caso in cui non esistano, crea i file seguendo il percorso specificato dalle stringhe
    void Start()
    {
        loginUsernameIPT.characterLimit = 12;
        loginPasswordIPT.characterLimit = 12;
        registerUsernameIPT.characterLimit = 12;
        registerPasswordIPT.characterLimit = 12;
        registerConfirmPasswordIPT.characterLimit = 12;

        loggedPlayerPath = Application.persistentDataPath + "/Logged_Player.txt";
        leaderboardPath = Application.persistentDataPath + "/Leaderboard.txt";
        accountsPath = Application.persistentDataPath + "/Accounts.txt";

        if (!File.Exists(accountsPath))
        {
            FileStream fs = File.Create(accountsPath);
            fs.Close();
        }
        else if (!File.Exists(leaderboardPath))
        {
            FileStream fs = File.Create(leaderboardPath);
            fs.Close();
        }
        else if (!File.Exists(loggedPlayerPath))
        {
            FileStream fs = File.Create(loggedPlayerPath);
            fs.Close();
        }

        //controlla nel file se un giocatore è già loggato
        var data = File.ReadAllLines(loggedPlayerPath);
        try
        {
            loggedPlayerText.text = "Accesso effettuato come " + data[0];

            //si disattivano i bottoni per login e register e si attiva quello per il logout
            login.gameObject.SetActive(false);
            register.gameObject.SetActive(false);
            logout.gameObject.SetActive(true);
        }
        catch
        {

        }
    }

    //cripta il dato che gli viene assegnato
    static string Encrypt(string value)
    {
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
            byte[] data = md5.ComputeHash(utf8.GetBytes(value));
            return Convert.ToBase64String(data);
        }
    }

    //eseguita alla pressione del bottone per eseguire l'accesso
    public void Login()
    {
        if (loginUsernameIPT.text == "" || loginPasswordIPT.text == "")//controlla se siano stati inseriti dei dati nei campi
        {
            errorMSG.text = "Compilare i campi per effettuare il login.";
            errorMSG.gameObject.SetActive(true);
        }
        else
        {
            var data = File.ReadAllLines(accountsPath); //memorizzo in data tutte le righe           

            for (int i = 0; i < data.ToArray().Length; i++)
            {
                string[] credentials = data.ToArray()[i].Split(',');
                string checkUsername = credentials[0];
                string checkPassword = credentials[1];

                if (checkUsername == loginUsernameIPT.text && checkPassword == Encrypt(loginPasswordIPT.text))//controlla che le credenziali che vengono inserite
                {
                    loggedIn = true;
                }
            }

            //se non sono state trovate
            if (!loggedIn)
            {
                errorMSG.text = "L'username o la password inseriti sono errati.";
                errorMSG.gameObject.SetActive(true);
            }
            else //se sono state trovate
            {
                StreamWriter player = new StreamWriter(loggedPlayerPath, false); //viene scritto nel file del giocatore loggato l'username
                player.Write(loginUsernameIPT.text);
                player.Close();

                loggedPlayerText.text = "Accesso effettuato come " + loginUsernameIPT.text; //viene modificato il testo che comunica con quale account si è loggati

                backFromLogin.onClick.Invoke(); //si torna al menu principale

                //si disattivano i bottoni per login e register e si attiva quello per il logout
                login.gameObject.SetActive(false);
                register.gameObject.SetActive(false);
                logout.gameObject.SetActive(true);
            }
        }

    }
    public void Register()
    {
        //viene controllato cosa viene inserito nei campi
        if (registerUsernameIPT.text == "" || registerPasswordIPT.text == "" || registerConfirmPasswordIPT.text == "") //se uno dei campi è vuoto non si effettua la registrazione
        {
            errorMSG.text = "Compilare i campi per effettuare la registrazione.";
            errorMSG.gameObject.SetActive(true);
        }
        else if(registerPasswordIPT.text != registerConfirmPasswordIPT.text) //se la password e la conferma non corrispondono non si effettua la registrazione
        {
            errorMSG.text = "Le password non corrispondono. Riprova.";
            errorMSG.gameObject.SetActive(true);
        }else
        {
            bool registrationCompleted = true; 

            var data = File.ReadAllLines(accountsPath);

            //viene confrontato il nome inserito con tutti i nomi nel file
            for (int i = 0; i < data.ToArray().Length; i++)
            {
                string[] player = data.ToArray()[i].Split(',');
                string checkUsername = player[0];

                if (checkUsername == registerUsernameIPT.text)//se è presente la registrazione non è effettuata
                {
                    registrationCompleted = false;
                    errorMSG.text = "Il nome inserito è già registrato"; //!! <- messaggio di errore aggiunto dopo la versione Beta 1.0.2
                    errorMSG.gameObject.SetActive(true);
                }
            }

            if (registrationCompleted) //se la registrazione viene effettuata
            {
                //viene salvato su file nome e password criptata
                StreamWriter saveAccount = new StreamWriter(accountsPath, true);
                saveAccount.WriteLine(registerUsernameIPT.text + "," + Encrypt(registerPasswordIPT.text));
                saveAccount.Close();

                //viene salvato il nome del giocatore loggato
                StreamWriter loggedPlayer = new StreamWriter(loggedPlayerPath, false);
                loggedPlayer.Write(registerUsernameIPT.text);
                loggedPlayer.Close();
                loggedPlayerText.text = "Accesso effettuato come " + registerUsernameIPT.text;

                backFromRegister.onClick.Invoke();

                //si disattivano i bottoni per login e register e si attiva quello per il logout
                login.gameObject.SetActive(false);
                register.gameObject.SetActive(false);
                logout.gameObject.SetActive(true);
            }
        }
    }

    //controlla che il messaggio di errore sia attivo, nel caso in cui lo sia, quando l'utente modifica gli input field, li resetta ed elimina il messaggio di errore
    public void Clear()
    {
        if (errorMSG.gameObject.activeSelf == true)
        {
            loginUsernameIPT.text = "";
            loginPasswordIPT.text = "";
            registerUsernameIPT.text = "";
            registerPasswordIPT.text = "";
            registerConfirmPasswordIPT.text = "";
            errorMSG.gameObject.SetActive(false);
        }
    }

    //cancella il testo nel file del giocatore loggato e comunica che il login non è eseguito
    public void Logout()
    {
        File.WriteAllText(loggedPlayerPath, string.Empty);
        loggedPlayerText.text = "Login non eseguito";
    }

    //resetta gli inputfield nel caso vengano cliccati alcuni bottoni
    public void ClearOnReload()
    {
        loginUsernameIPT.text = "";
        loginPasswordIPT.text = "";
        registerUsernameIPT.text = "";
        registerPasswordIPT.text = "";
        registerConfirmPasswordIPT.text = "";
        errorMSG.gameObject.SetActive(false);
    }

}
