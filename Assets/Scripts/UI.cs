using GoogleMobileAds.Api;
using Scripts.Ads;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : BaseUI {

    public GameObject pauseMenu;
    public GameObject winMenu;

    public GameObject freeRoll;
    public GameObject paidRoll;

    public GameObject tillFree;
    public GameObject timeLeft;

    public GameObject notEnoughCoins;
    public GameObject watchAd;
    public GameObject roll;

    public GameObject p1;
    public GameObject p2;
    public GameObject ai;

    public GameObject coinTotal;
    public GameObject highScoreTotal;
    public GameObject bestScoreTotal;

    public GameObject coinsEarnedImpact;

    public GameObject goalAnimation;
    public GameObject highScoreAnimation;
    public GameObject winnerAnimation;
    public GameObject comboAnimation;

    public Sprite[] symbolArray;

    public float goalAnimationLength = 1.5f;
    public float newRoundStartAnimationLength = 1.5f;
    public float specialAnimationLength = 1.5f;
    public float winAnimationLength = 1f;
    public float highScoreAnimationLength = 3.5f;

    public GameObject scoreLeft;
    public GameObject scoreRight;

    public GameObject winnerLeft;
    public GameObject winnerRight;

    public GameObject highscore;

    public GameObject winAnimationController;

    [System.NonSerialized]
    public static UI Instance;

    public UI() {
        Instance = this;
    }

    public void Start() {
        if (SettingsController.Instance != null) {
            showTotals();
            showPlayers();
        }
    }

    public void showTotals() {
        display4CharNumber(coinTotal, SettingsController.Instance.getCoins());
        display4CharNumber(highScoreTotal, ScoreController.Instance.getCombo());
    }

    public void updateScore(GameObject player, string score) {
        if (player.name.Equals("RacketLeft")) {
            scoreLeft.GetComponent<Text>().text = score.ToString();
        } else if (player.name.Equals("RacketRight")) {
            scoreRight.GetComponent<Text>().text = score.ToString();
        }
    }

    public void showNewHighScore(int combo) {
        if (combo > 9999) {
            combo = 9999; //TODO move out of here
        }
        string highscore = "HIGH SCORE! " + combo;
        highScoreAnimation.GetComponent<DynamicText>().SetText(highscore);
        highScoreAnimation.GetComponent<Animator>().Play("HighScoreIn");
    }

    public void showComboInARow(int combo) {
        comboAnimation.GetComponent<DynamicText>().SetText(combo.ToString() + " IN A ROW");
        comboAnimation.GetComponent<Animator>().Play("ComboIn");
    }

    public void showWinner(GameObject winner) {
        if (winner.name.Equals("RacketRight")) {
            winAnimationController.GetComponent<Animator>().Play("WinRight");
        } else {
            winAnimationController.GetComponent<Animator>().Play("WinLeft");
        }
        StartCoroutine(cleanUp());

    }

    public void showGoal() {
        StartCoroutine(startGoal());
    }

    IEnumerator startGoal() {
        goalAnimation.GetComponent<Animator>().Play("GoalAnimationIn");
        CameraShake.Instance.cameraShake(CameraShakeType.VERY_STRONG);
//        yield return new WaitForSeconds(goalAnimationLength);
        yield return new WaitForSeconds(0.6f);
        BallController.Instance.showBall();
        yield return new WaitForSeconds(1f);
        GameController.Instance.startNewRound();
        display4CharNumber(highScoreTotal, 0);
    }

    public void showPauseMenu() {
        if (Time.timeScale == 0) {
            activate(pauseMenu);
        }
        else {
            AdManager.Instance.hideBannerAd();
            deactivate(pauseMenu);
        }
    }

    public void showWinMenu() {
        deactivate(highscore);
        deactivate(p1);
        deactivate(p2);
        deactivate(ai);
        activate(winMenu);
        activate(roll);
        display4CharNumber(bestScoreTotal, SettingsController.Instance.getMaxCombo());
        rollRefresh();
        if (TimeUtility.getIsFreeRollAvailable()) {
            deactivate(notEnoughCoins);
            deactivate(paidRoll);
            activate(roll);
            activate(freeRoll);
            deactivate(watchAd);
        } else {
            activate(tillFree);
            TimeSpan passedSinceLastRoll = TimeUtility.getTimePassedSinceLastRoll();
            displayTime(passedSinceLastRoll.Hours, passedSinceLastRoll.Minutes);
        }
        StartCoroutine(AdManager.Instance.ShowBannerAdWhenReady());
    }

    public void rollRefresh() {
        if (SettingsController.Instance.getCoins() > 50) {
            activate(paidRoll);
            deactivate(freeRoll);
            deactivate(notEnoughCoins);
            deactivate(watchAd);
        } else {
            deactivate(roll);
            deactivate(paidRoll);
            deactivate(freeRoll);
            activate(notEnoughCoins);
            StartCoroutine(ButtonManager.Instance.ShowAdButtonWhenReady(watchAd));
        }
    }

    public void enoughCoins() {
        ObjectUtility.disableGameObject(notEnoughCoins);
        ObjectUtility.disableGameObject(watchAd);
    }

    public IEnumerator cleanUp() {
        yield return new WaitForSeconds(highScoreAnimationLength);
        highScoreAnimation.GetComponent<DynamicText>().SetText("");
    }

    public Sprite toImage(char symbol) {
        switch (symbol) {
            case '0': return symbolArray[0]; break;
            case '1': return symbolArray[1]; break;
            case '2': return symbolArray[2]; break;
            case '3': return symbolArray[3]; break;
            case '4': return symbolArray[4]; break;
            case '5': return symbolArray[5]; break;
            case '6': return symbolArray[6]; break;
            case '7': return symbolArray[7]; break;
            case '8': return symbolArray[8]; break;
            case '9': return symbolArray[9]; break;
            case 'c': return symbolArray[10]; break;
            case ' ': return symbolArray[11]; break;
            default: return symbolArray[0];
        }
    }

    public void display4CharNumber(GameObject display, int number) {
        Image[] componentImages = display.GetComponentsInChildren<Image>();
        if (componentImages.Length > 0) {
            char[] charArray = number.ToString().ToCharArray();
            for (int i = 0; i < 4; i++) {
                if (i < charArray.Length) {
                    componentImages[i].sprite = toImage(charArray[i]);
                } else {
                    componentImages[i].sprite = toImage(' ');
                }
            }
        }
    }

    private void displayTime(int hour, int minute) {
        Image[] componentImages = timeLeft.GetComponentsInChildren<Image>();
        componentImages[0].sprite = toImage('0');
        componentImages[1].sprite = toImage('0');
        componentImages[2].sprite = toImage('0');
        componentImages[3].sprite = toImage('0');

        char[] hourArray = hour.ToString().ToCharArray();
        if (hourArray.Length > 0) {
            componentImages[1].sprite = toImage(hourArray[0]);
        } else if (hourArray.Length > 1) {
            componentImages[0].sprite = toImage(hourArray[0]);
            componentImages[1].sprite = toImage(hourArray[1]);
        }
        char[] minArray = minute.ToString().ToCharArray();
        if (minArray.Length > 0) {
            componentImages[2].sprite = toImage(minArray[0]);
        } else if (minArray.Length > 1) {
            componentImages[2].sprite = toImage(minArray[0]);
            componentImages[3].sprite = toImage(minArray[1]);
        }
    }

    public void refreshCoins(int coins) {
        display4CharNumber(coinTotal, coins);
    }

    public void refreshScore(int score) {
        display4CharNumber(highScoreTotal, score);
    }

    public void showPlayers() {
        if (SettingsController.Instance.isVersusAI) {
            activate(ai);
            deactivate(p2);
        }
        StartCoroutine(hidePlayers());
    }

    public IEnumerator hidePlayers() {
        yield return new WaitForSeconds(15);
        deactivate(ai);
        deactivate(p1);
        deactivate(p2);
        yield break;
    }
}
