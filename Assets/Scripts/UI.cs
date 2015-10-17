using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TextFx;

public class UI : BaseUI {
    public GameObject pauseMenu;
    public GameObject winMenu;

    public GameObject freeRoll;
    public GameObject paidRoll;

    public GameObject goalAnimation;
    public GameObject highScoreAnimation;
    public GameObject winnerAnimation;
    public GameObject comboAnimation;
    public GameObject doubleAnimation;
    public GameObject speedAnimation;
    public GameObject longAnimation;
    public GameObject shortAnimation;
    public GameObject coinAnimation;
    public GameObject noGoalsAnimation;
    public GameObject shieldAnimation;
    public GameObject homingAnimation;
    public GameObject movingGoalsAnimation;

    public float goalAnimationLength = 3f;
    public float specialAnimationLength = 1f;
    public float winAnimationLength = 5f;
    public float highScoreAnimationLength = 1f;

    public GameObject scoreLeft;
    public GameObject scoreRight;

    public void showDoubleAnimation() {
        doubleAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showSpeedAnimation() {
        speedAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showLongAnimation() {
        longAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showShortAnimation() {
        shortAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showNoGoalsAnimation() {
        noGoalsAnimation.GetComponent<TextFxNative>().GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showShieldAnimation() {
        shieldAnimation.GetComponent<TextFxNative>().GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showCoinAnimation() {
        coinAnimation.GetComponent<TextFxNative>().SetText("COINS " + SettingsController.Instance.getCoins());
        coinAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void updateScore(GameObject player, string score) {
        if (player.name.Equals("RacketLeft")) {
            scoreLeft.GetComponent<Text>().text = score.ToString();
        } else if (player.name.Equals("RacketRight")) {
            scoreRight.GetComponent<Text>().text = score.ToString();
        }
        goalAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showNewHighScore(int combo) {
        highScoreAnimation.GetComponent<TextFxNative>().SetText("HIGH SCORE! " + combo.ToString());
        highScoreAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showComboInARow(int combo) {
        comboAnimation.GetComponent<TextFxNative>().SetText(combo.ToString() + " IN A ROW");
        comboAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showWinner(GameObject winner) {
        int direction = 1;
        if (winner.name.Equals("RacketLeft")) {
            direction = -1;
        }
        winnerAnimation.transform.position = new Vector3(direction * 500, 400, 0);
        winnerAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showSpecialAnimation(PowerUpType powerUpType) {
        switch (powerUpType) {
            case PowerUpType.DOUBLE: showDoubleAnimation(); break;
            case PowerUpType.SPEED: showSpeedAnimation(); break;
            case PowerUpType.LONG: showLongAnimation(); break;
            case PowerUpType.SHORT: showShortAnimation(); break;
            case PowerUpType.COIN: showCoinAnimation(); break;
            case PowerUpType.NO_GOALS: showNoGoalsAnimation(); break;
//            case SpecialType.HOMING: showHomingAnimation(); break;
            case PowerUpType.MOVING_GOALS: showNoGoalsAnimation(); break;
            case PowerUpType.SHIELD: showShieldAnimation(); break;
        }
    }

    public void showGameAnimation(AnimationType animationType) {
        StartCoroutine(startAnimation(animationType));
    }

    IEnumerator startAnimation(AnimationType animationType) {//TODO cancel or delay current animation
//        Debug.Log("Waiting for animation to finish");
        switch (animationType) {
            case AnimationType.GOAL:
            yield return new WaitForSeconds(goalAnimationLength);
            break;
            case AnimationType.HIGH_SCORE:
            yield return new WaitForSeconds(highScoreAnimationLength);
            break;
            case AnimationType.WIN:
            yield return new WaitForSeconds(winAnimationLength);
            break;
            case AnimationType.SPECIAL: yield return new WaitForSeconds(specialAnimationLength);break;
        }
//        Debug.Log("Animation finished");
        StartCoroutine(postAnimationAction(animationType));
    }

    IEnumerator postAnimationAction(AnimationType animationType) {
        switch (animationType) {
            case AnimationType.GOAL: BallController.Instance.moveInRandomDirection();break;
            case AnimationType.WIN: break;
            case AnimationType.HIGH_SCORE: yield break;
            case AnimationType.SPECIAL: break;
        }
    }

    public void showPauseMenu() {
        if (Time.timeScale == 0) {
            activate(pauseMenu);
        }
        else {
            deactivate(pauseMenu);
        }
    }

    public void showWinMenu() {
        activate(winMenu);
            if (SettingsController.Instance.getCoins() > 1) {
                activate(paidRoll);
                deactivate(freeRoll);
                //if time passed
                deactivate(paidRoll);
                activate(freeRoll);
            } else {
                deactivate(paidRoll);
                deactivate(freeRoll);
                //TODO Show not enough coins, watch ad
            }
    }
}
