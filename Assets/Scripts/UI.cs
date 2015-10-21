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

    public float goalAnimationLength = 3f;
    public float specialAnimationLength = 1.5f;
    public float winAnimationLength = 5f;
    public float highScoreAnimationLength = 1f;

    public GameObject scoreLeft;
    public GameObject scoreRight;

    public void updateScore(GameObject player, string score) {
        if (player.name.Equals("RacketLeft")) {
            scoreLeft.GetComponent<Text>().text = score.ToString();
        } else if (player.name.Equals("RacketRight")) {
            scoreRight.GetComponent<Text>().text = score.ToString();
        }
//        ObjectUtility.enableGameObject(goalAnimation);
//        goalAnimation.GetComponent<Animator>().enabled = true;
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
        if (winner.name.Equals("RacketRight")) {
            direction = -1;
        }
        winnerAnimation.transform.position = new Vector3(direction * 500, 400, 0);
        winnerAnimation.GetComponent<TextFxNative>().AnimationManager.PlayAnimation();
    }

    public void showGameAnimation(AnimationType animationType) {
        StartCoroutine(startAnimation(animationType));
    }

    IEnumerator startAnimation(AnimationType animationType) {
        switch (animationType) {
            case AnimationType.GOAL:
            CameraShake.Instance.cameraShake(CameraShakeType.VERY_STRONG);
            yield return new WaitForSeconds(goalAnimationLength);
            BallController.Instance.moveInRandomDirection(); //TODO move out of here
            break;
            case AnimationType.HIGH_SCORE:
            yield return new WaitForSeconds(highScoreAnimationLength);
            break;
            case AnimationType.WIN:
            yield return new WaitForSeconds(winAnimationLength);
            break;
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
