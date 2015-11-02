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

    private Vector3 leftWall1Transform;
    private Vector3 leftWall2Transform;
    private Vector3 rightWall1Transform;
    private Vector3 rightWall2Transform;

    public void Start() {
        leftWall1Transform = leftWall1.transform.position;
        leftWall2Transform = leftWall2.transform.position;
        rightWall1Transform = rightWall1.transform.position;
        rightWall2Transform = rightWall2.transform.position;
    }
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
        leftWall1.transform.position = leftWall1Transform;
        leftWall2.transform.position = leftWall2Transform;
        rightWall1.transform.position = rightWall1Transform;
        rightWall2.transform.position = rightWall2Transform;
        CancelInvoke("moveWalls");
    }

}
