using Mono.Xml.Xsl;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private float goals = 0;
    public bool isDisabled = false;
    public GameObject racket;
    public GameObject goalImpact;
    public float IMPACT_LENGTH = 1.1f;

    public GameObject getPlayer() {
        return racket;
    }

    public void Start() {
    }

    public float getGoals() {
        return goals;
    }

    public void setGoals(float goals) {
        this.goals = goals;
    }

    //TODO add sprite for disabled goals
    public void toggleDisabled() {
        isDisabled = true;
        StartCoroutine(noGoalSepcial());
    }

    IEnumerator noGoalSepcial() {
        yield return new WaitForSeconds(SpecialController.Instance.NO_GOALS_DURATION);
        isDisabled = false;
    }

    public void incrementGoals() {
        goals++;
        GameController.Instance.UIControl.updateScore(racket, goals.ToString());
    }

    void OnCollisionEnter2D(Collision2D collider) {
        if (collider.gameObject.tag == "Ball" && !isDisabled) {
            GameController.Instance.goalScored(racket);
            goalImpact.transform.position = new Vector3(goalImpact.transform.position.x, collider.gameObject.transform.position.y, 0);
            ObjectUtility.enableGameObject(goalImpact);
            StartCoroutine(disableImpactTimer());
        }
    }

    IEnumerator disableImpactTimer() {
        yield return new WaitForSeconds(IMPACT_LENGTH);
        ObjectUtility.disableGameObject(goalImpact);
    }

    public bool checkWinCondition() {
        if (goals >= GameController.Instance.GOALS_TO_WIN) {
            return true;
        } else {
            return false;
        }
    }
}
