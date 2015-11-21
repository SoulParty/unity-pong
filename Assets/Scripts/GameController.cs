using Scripts.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour {

    public float specialInterval = 3f;
    public float coinInterval = 5f;
    public float impactLength = 0.75f;
    public float timeBeforeWinMenuLength = 1f;

    public int DISTANCE_FROM_GOAL = -840;
    public float ARENA_HALF_LENGTH = 840;
    public float DISTANCE_BETWEEN_WALLS = 1080;
    public float WALL_THICKNESS = 110;
    public int playerPosition = 840;

    public int GOALS_TO_WIN = 10;

    public GameObject topWall;
    public GameObject bottomWall;

    public GameObject player1;
    public GameObject player2;
    public GameObject ball;
    public GameObject rollManager;
    public GameObject aiDot;
    public GameObject player2InputHandler;

    public GameObject specialHitImpact;
    public GameObject coinHitImpact;

    private bool isDoubleBallMode = false;

    private GameObject lastTouchedById;

    public UI UIControl;

    public float gameSpeed = 1;

    public int defaultBallStartSpeed = 1300;
    public int ballSpeedUp = 50;

    public float initialSpawnDelay = 3f;

    [System.NonSerialized]
    public static GameController Instance;

    public GameController() {
        Instance = this;
    }

    public void Start() {
        Init();
        Time.timeScale = gameSpeed;
    }

    public void Init() {
        if (AdManager.Instance != null) {
            AdManager.Instance.hideBannerAd();
        }

        if (GameObject.Find("SettingsController") == null) {
            Instantiate(Resources.Load("SettingsController"));
        } else {
            player1.GetComponent<RacketManager>().setSprite(SettingsController.Instance.selectedPlayer1Racket);
            player2.GetComponent<RacketManager>().setSprite(SettingsController.Instance.selectedPlayer2Racket);
            ball.GetComponent<BallManager>().setSprite(SettingsController.Instance.selectedPuck);
        }

        if (GameObject.Find("SoundController") == null) {
            Instantiate(Resources.Load("SoundController"));
        }

        startSpecialSpawning();

//        DISTANCE_BETWEEN_WALLS = Vector3.Distance(bottomWall.gameObject.transform.position, topWall.gameObject.transform.position) - WALL_THICKNESS;
        ARENA_HALF_LENGTH = DISTANCE_BETWEEN_WALLS / 2;
        if (SettingsController.Instance.isVersusAI) {
            PongAIController.Instance.Init();
        } else {
            ObjectUtility.disableGameObject(aiDot);
        }

        UI.Instance.refreshCoins(SettingsController.Instance.getCoins());
    }

    public void spawnRandomSpecial() {
        SpecialController.Instance.spawnRandom();
    }

    public void spawnRandomCoin() {
        SpecialController.Instance.spawnCoin();
    }

    public void restart() {
        BallController.Instance.resetBalls();
        SpecialController.Instance.reset();
        if (SettingsController.Instance.isVersusAI) {
            PongAIController.Instance.reset();
        }
    }

    public void showGoal() {
        UIControl.showGoal();
    }

    public void incrementCombo() {
        ScoreController.Instance.incrementCombo();
    }

    public void newHighScore(int combo) {
        PlayerPrefs.SetInt(Const.MAX_COMBO, combo);
        UIControl.showNewHighScore(combo);
    }

    public void comboInARow(int combo) {
        UIControl.showComboInARow(combo);
    }

    public void gameFinished(GameObject winner) {

        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball")) {
            ball.GetComponent<Rigidbody2D>().isKinematic = true;
        }
        StartCoroutine(showGameEndMenu(winner));
        MusicController.Instance.playWin();
    }

    public bool getIsDoubleBallMode() {
        return isDoubleBallMode;
    }

    public void toggleIsDoubleBallMode() {
        isDoubleBallMode = !isDoubleBallMode;
    }

    public void goalScored(GameObject player) {
        if (player != null) {
            ScoreController.Instance.incrementGoals(player);
            showGoal();
            restart();
            if (ScoreController.Instance.checkWinCondition(player)) {
                gameFinished(player);
            }
        }
        ScoreController.Instance.highScoreCheck();
        ScoreController.Instance.resetMaxCombo();
        MusicController.Instance.playGoal();
        stopSpecialSpawning();
    }

    public GameObject getLastTouchedBy() {
        return lastTouchedById;
    }

    public void setLastTouchedBy(GameObject player) {
        lastTouchedById = player;
    }

    public void pauseGame() {
        if (Time.timeScale == 1) {
            if (AdManager.Instance != null) {
                StartCoroutine(AdManager.Instance.ShowBannerAdWhenReady());
            }
            Time.timeScale = 0;
            UIControl.showPauseMenu();
        }
        else {
            Time.timeScale = 1;
            if (AdManager.Instance != null) {
                AdManager.Instance.hideBannerAd();
            }
            UIControl.showPauseMenu();
        }
    }

    public IEnumerator showGameEndMenu(GameObject winner) {
        yield return new WaitForSeconds(timeBeforeWinMenuLength);
        ObjectUtility.disableGameObject(ball);
        player1.GetComponent<Rigidbody2D>().isKinematic = true;
        player2.GetComponent<Rigidbody2D>().isKinematic = true;

        stopSpecialSpawning();

        UIControl.showWinMenu();
        UIControl.showWinner(winner);
    }

    public void stopSpecialSpawning() {
        CancelInvoke("spawnRandomSpecial");
        CancelInvoke("spawnRandomCoin");
    }

    public void startSpecialSpawning() {
        InvokeRepeating("spawnRandomSpecial", initialSpawnDelay, specialInterval);
        InvokeRepeating("spawnRandomCoin", initialSpawnDelay, coinInterval);
    }

    public void playAgain() {
        Application.LoadLevel(1);
    }

    public void rollForPrize() {
//        if (SettingsController.Instance.getCoins() > 1) {
//            rollManager.GetComponent<RollManager>().rollTheDice();
//        } else {
//
//        }
    }

    public void explodeSpecial(Vector3 specialPosition) {
        specialHitImpact.transform.position = specialPosition;
        ObjectUtility.enableGameObject(specialHitImpact);
        StartCoroutine(disableSpecialImpactTimer());
    }

    IEnumerator disableSpecialImpactTimer() {
        yield return new WaitForSeconds(impactLength);
        ObjectUtility.disableGameObject(specialHitImpact);
    }

    public void explodeCoin(Vector3 coinPosition) {
        coinHitImpact.transform.position = coinPosition;
        ObjectUtility.enableGameObject(coinHitImpact);
        StartCoroutine(disableCoinImpactTimer());
    }

    IEnumerator disableCoinImpactTimer() {
        yield return new WaitForSeconds(impactLength);
        ObjectUtility.disableGameObject(coinHitImpact);
    }

    public void startNewRound() {
        BallController.Instance.moveInRandomDirection();
        startSpecialSpawning();
    }
}
