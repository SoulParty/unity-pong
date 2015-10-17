using System.Collections;
using UnityEngine;

public class ArenaController : MonoBehaviour {

    [System.NonSerialized]
    public static ArenaController Instance;

    public ArenaController() {
        Instance = this;
    }

    public ScoreManager leftScoreManager;
    public ScoreManager rightScoreManager;

    private float movingGoalsScale = 0f;

    private int topDirection = 1;
    private int bottomDirection = 1;

    public GameObject leftWall1;
    public GameObject leftWall2;
    public GameObject rightWall1;
    public GameObject rightWall2;

    private void moveWalls() {
        movingGoalsScale += SpecialController.Instance.MOVING_GOALS_STEP;
        if (movingGoalsScale > 1.2f) {
            topDirection *= -1;
            bottomDirection *= -1;
            movingGoalsScale = -1.2f;
        }
        leftWall1.transform.position += new Vector3(0, topDirection * SpecialController.Instance.MOVING_GOAL_AMOUNT, 0);
        leftWall2.transform.position += new Vector3(0, bottomDirection * SpecialController.Instance.MOVING_GOAL_AMOUNT, 0);
        rightWall1.transform.position += new Vector3(0, topDirection * SpecialController.Instance.MOVING_GOAL_AMOUNT, 0);
        rightWall2.transform.position += new Vector3(0, bottomDirection * SpecialController.Instance.MOVING_GOAL_AMOUNT, 0);
    }

    public void toggleGoals() {
        leftScoreManager.toggleDisabled();
        rightScoreManager.toggleDisabled();
    }

    public void enableMovingGoals() {
        InvokeRepeating("moveWalls", 0, 0.02f);
        StartCoroutine(stopMovingGoals());
    }

    public IEnumerator stopMovingGoals() {
        yield return new WaitForSeconds(SpecialController.Instance.MOVING_GOALS_DURATION);
        CancelInvoke("moveWalls");
    }

}
