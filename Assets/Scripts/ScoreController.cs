using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
    [System.NonSerialized]
    public static ScoreController Instance;

    public ScoreManager goalLeft;
    public ScoreManager goalRight;

    public int combo = 0;
    public int newHighScore = 0;

    public ScoreController() {
        Instance = this;
    }

    public void incrementCombo() {
        combo++;
        UI.Instance.refreshScore(combo);
        if (combo % 5 == 0) {
            GameController.Instance.comboInARow(combo);
        }
    }

    public bool highScoreCheck() {
//        if (newHighScore > SettingsController.Instance.getMaxCombo()) {
        if (true) {
            GameController.Instance.newHighScore(newHighScore);
            return true;
        } else {
            return false;
        }
    }

    public void resetMaxCombo() {
        if (combo > newHighScore && combo > SettingsController.Instance.getMaxCombo()) {
            newHighScore = combo;
        }
        combo = 0;
    }

    public void incrementGoals(GameObject player) {
        if (goalLeft.getPlayer() != player) {
            goalLeft.incrementGoals();
        } else {
            goalRight.incrementGoals();
        }
    }

    public bool checkWinCondition(GameObject player) {
        if (goalLeft.getPlayer() != player) {
            return goalLeft.checkWinCondition();
        } else {
            return goalRight.checkWinCondition();
        }
    }

    public int getCombo() {
        return combo;
    }
}
