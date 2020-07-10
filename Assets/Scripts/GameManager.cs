using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public AudioSource theMusic;

    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    private int currentScore;
    private int scorePerNote = 100;
    private int scorePerGoodNote = 125;
    private int scorePerPerfectNote = 150;

    private int currentMulitplier;
    private int multiplierTracker;
    private int[] multiplierThresholds = { 4, 8, 16 };

    public Text scoreText;
    public Text multiText;

    private float totalNotes;
    private float normalHits;
    private float goodHits;
    private float perfectHits;
    private float missedHits;

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

    // Start is called before the first frame update
    void Start() {
        instance = this;

        currentScore = 0;
        scoreText.text = "Score: " + currentScore;
        currentMulitplier = 1;

        totalNotes = FindObjectsOfType<NoteObject>().Length;
    }

    // Update is called once per frame
    void Update() {
        if(!startPlaying) {
            if(Input.anyKeyDown) {
                startPlaying = true;
                theBS.hasStarted = true;

                theMusic.Play();
            }
        } else {
            if (!theMusic.isPlaying && !resultsScreen.activeInHierarchy) {
                resultsScreen.SetActive(true);

                normalsText.text = normalHits.ToString();
                goodsText.text = goodHits.ToString();
                perfectsText.text = perfectHits.ToString();
                missesText.text = missedHits.ToString();

                float totalHit = normalHits + goodHits + perfectHits;
                float percentHit = (totalHit / totalNotes) * 100f;

                percentHitText.text = percentHit.ToString("F1") + "%";

                string rankVal = "F";

                if (percentHit == 100) {
                    rankVal = "S+";
                } else if (percentHit >= 95) {
                    rankVal = "S";
                } else if (percentHit >= 90) {
                    rankVal = "A";
                } else if (percentHit >= 80) {
                    rankVal = "B";
                } else if (percentHit >= 70) {
                    rankVal = "C";
                } else if (percentHit >= 60) {
                    rankVal = "D";
                }

                rankText.text = rankVal;

                finalScoreText.text = currentScore.ToString();
            }
        }
    }

    public void NoteHit() {
        //UnityEngine.Debug.Log("Hit On Time");

        if (currentMulitplier - 1 < multiplierThresholds.Length) {
            multiplierTracker++;

            if (multiplierThresholds[currentMulitplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMulitplier++;
            }
        }

        multiText.text = "Multiplier: x" + currentMulitplier;

        //currentScore += scorePerNote * currentMulitplier;
        scoreText.text = "Score: " + currentScore;
    }

    public void NormalHit() {
        currentScore += scorePerNote * currentMulitplier;
        NoteHit();

        normalHits++;
    }

    public void GoodHit() {
        currentScore += scorePerGoodNote * currentMulitplier;
        NoteHit();

        goodHits++;
    }

    public void PerfectHit() {
        currentScore += scorePerPerfectNote * currentMulitplier;
        NoteHit();

        perfectHits++;
    }

    public void NoteMissed() {
        UnityEngine.Debug.Log("Missed Note");

        currentMulitplier = 1;
        multiplierTracker = 0;

        multiText.text = "Multiplier: x" + currentMulitplier;

        missedHits++;
    }
}
