using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public RowUI rowUi; //riferimento alle righe

    string scoreboardFilePath;  //percorso del che contiene i nomi dei giocatori e i rispettivi punteggi

    private void Start()
    {
        scoreboardFilePath = Application.persistentDataPath + "/Scoreboard.txt"; //si assegna il percorso alla variabile

        //si controlla che il file esista
        if (!File.Exists(scoreboardFilePath))
        {
            //se non esiste viene creato
            FileStream fs = File.Create(scoreboardFilePath);
            fs.Close();
        }

        SortScores(); //si riordina la classifica in ordine decrescente
        ShowScores(); //vengono mostrati i valori
        
    }

    void ShowScores()
    {
        var scores = File.ReadAllLines(scoreboardFilePath); //vengono immagazzinate le righe del file in una variabile

        int scoreboardLength = scores.Length;   //viene immagazzinata la lunghezza
        if (scoreboardLength > 10)  //se la lunghezza è maggiore di 10 si imposta a 10, in modo da far ripetere il for successivo per massimo 10 volte
        {
            scoreboardLength = 10;
        }

        //si istanziano le righe della classifica a partire dalla prima
        for (int i = 0; i < scoreboardLength; i++)
        {
            //ogni riga è composta da giocatore e punteggio, divisi da una virgola
            string[] player = scores.ToArray()[i].Split(',');

            var row = Instantiate(rowUi, transform);

            //si impostano la posizione, il nome e il punteggio dei giocatori
            row.rank.text = (i + 1).ToString();
            row.name.text = player[0];
            row.score.text = player[1];
        }
    }

    //al metodo si passa il nome del giocatore, il punteggio e se quest'ultimo debba essere aggiunto a quello precedente o se debba sostituirlo in caso sia più alto
    public void AddScore(string playerName, int score, bool additive)
    {
        var scores = File.ReadAllLines(scoreboardFilePath); //si acquisisce il contenuto della classifica
        bool playerFound = false;   //indica se il giocatore è contenuto nel file

        //viene comparato il nome del giocatore con ogni riga del file
        for (int i = 0; i < scores.Length; i++)
        {
            string[] player = scores.ToArray()[i].Split(',');
            if (playerName == player[0]) //se il nome è nella lista
            {
                playerFound = true; //il giocatore è stato trovato

                if (additive)//se il punteggio va aggiunto
                {
                    int newHighScore = Int32.Parse(player[1]) + score; //si calcola il nuovo punteggio
                    player[1] = newHighScore.ToString();    //e si assegna all'elemento dopo la virgola
                }
                if (!additive && score > Int32.Parse(player[1])) //se il punteggio non va aggiunto e il nuovo punteggio è maggiore del precedente
                {
                    player[1] = score.ToString(); //si sostituisce il punteggio nuovo a quello vecchio
                }
            }
            scores[i] = player[0] + "," + player[1]; //si riscrive la riga
        }
        if (!playerFound) //se il giocatore non è stato trovato nella lista
        {
            Array.Resize(ref scores, scores.Length + 1); //si incrementa la lunghezza dell'array
            scores[scores.Length - 1] = playerName + "," + score.ToString(); //si inserisce il nome e il punteggio separati dalla virgola
            SortScores(); //si riordinano i punteggi
        }
        File.WriteAllLines(scoreboardFilePath, scores); //il file viene aggiornato
    }

    //riordina i punteggi
    void SortScores()
    {
        var scores = File.ReadAllLines(scoreboardFilePath); //viene acquisito il contenuto del file

        //viene riordinato con un bubble sort
        for (int i = 0; i < scores.Length - 1; i++)
        {
            for(int j = 0; j < scores.Length - 1; j++)
            {
                string[] player1 = scores.ToArray()[j].Split(',');
                string[] player2 = scores.ToArray()[j + 1].Split(',');
                if(Int32.Parse(player1[1]) < Int32.Parse(player2[1]))
                {
                    var t = scores[j];
                    scores[j] = scores[j + 1];
                    scores[j + 1] = t;
                }
            }
            
        }
        File.WriteAllLines(scoreboardFilePath, scores); //si aggiorna il file
    }



}
