using System.Collections;
using System.Collections.Generic;
using TextFx;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour {

    public float specialInterval = 5f;
    public float coinInterval = 3f;

    public static int DISTANCE_FROM_GOAL = 840;
    public float ARENA_HALF_LENGTH = 840;
    public float DISTANCE_BETWEEN_WALLS = 1800;
    public float WALL_THICKNESS = 90;
    public int playerPosition = 840;

    public int GOALS_TO_WIN = 10;

    public GameObject topWall;
    public GameObject bottomWall;

    public GameObject player1;
    public GameObject player2;
    public GameObject puck;
    public GameObject rollManager;

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
        if (GameObject.Find("SettingsController") == null) {
            Instantiate(Resources.Load("SettingsController"));
        } else {
            player1.GetComponent<RacketManager>().setSprite(SettingsController.Instance.selectedPlayer1Racket);
            player2.GetComponent<RacketManager>().setSprite(SettingsController.Instance.selectedPlayer2Racket);
            puck.GetComponent<BallManager>().setSprite(SettingsController.Instance.selectedPuck);
        }

        InvokeRepeating("spawnRandomSpecial", initialSpawnDelay, specialInterval);
        InvokeRepeating("spawnRandomCoin", initialSpawnDelay, coinInterval);

        DISTANCE_BETWEEN_WALLS = Vector3.Distance(bottomWall.gameObject.transform.position, topWall.gameObject.transform.position) - WALL_THICKNESS;
        ARENA_HALF_LENGTH = DISTANCE_BETWEEN_WALLS / 2;
        PongAIController.Instance.Init();
    }

    public void spawnRandomSpecial() {
        SpecialController.Instance.spawnRandom();
    }

    public void spawnRandomCoin() {
        SpecialController.Instance.spawnCoin();
    }

    public void restart() {
        BallController.Instance.resetBalls();
        ScoreController.Instance.resetMaxCombo();
        SpecialController.Instance.reset();
        if (SettingsController.Instance.isVersusAI) {
            PongAIController.Instance.reset();
        }
    }

    public void startGameAnimation(AnimationType animationType) {
        UIControl.showGameAnimation(animationType);
    }

    public void startSpecialAnimation(PowerUpType powerUpType) {
        UIControl.showSpecialAnimation(powerUpType);
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

    public bool highScoreCheck() {
        if (ScoreController.Instance.highScoreCheck()) {
            return true;
        } else {
            return false;
        }
    }

    public void gameFinished(GameObject winner) {
//        this.winnerId = winnerId;
        UIControl.showWinner(winner);
        startGameAnimation(AnimationType.WIN);
        if (highScoreCheck()) {
//        if (true) {
            UIControl.showNewHighScore(ScoreController.Instance.getCombo());
            startGameAnimation(AnimationType.HIGH_SCORE);
        }
//        restart();
        StartCoroutine(showGameEndMenu());
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
            restart();
            startGameAnimation(AnimationType.GOAL);
            if (ScoreController.Instance.checkWinCondition(player)) {
                gameFinished(player);
            }
        }
    }

    public GameObject getLastTouchedBy() {
        return lastTouchedById;
    }

    public void setLastTouchedBy(GameObject player) {
        lastTouchedById = player;
    }

    public void pauseGame() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
            UIControl.showPauseMenu();
        }
        else {
            Time.timeScale = 1;
            UIControl.showPauseMenu();
        }
    }

    //TODO figure out what to do during end screen
    public IEnumerator showGameEndMenu() {
        yield return new WaitForSeconds(3);
        UIControl.showWinMenu();
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

}
