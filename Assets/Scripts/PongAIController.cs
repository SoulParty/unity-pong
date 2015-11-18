using Mono.Xml.Xsl;
using System;
using System.Collections;
using UnityEngine;

public class PongAIController : MonoBehaviour {

    public GameObject aIPlayer;
    public GameObject playerOne;
    public GameObject ball;
    public GameObject target;
    public GameObject referenceWallDown;
    public GameObject referenceWallUp;

    public int FOLLOW_BALL_SPEED = 1000;
    public int MOVEMENT_SPEED = 1000;
    public int BASE_BALL_FOLLOW_SPEED = 1000;
    public int BASE_MOVEMENT_SPEED = 1000;

    public int minimumTargetDistance = 20;

    private float distanceFromGoal;

    private GameObject nearestBall;

    private double arenaWidth;

    private bool isArrived;
    public bool isFollowBall = false;
    public int wildBall = 0;
    public double halfArena;

    private Rigidbody2D aiPlayerRigidBody;

    [System.NonSerialized]
    public static PongAIController Instance;

    public PongAIController() {
        Instance = this;
    }

    public void Start() {
        Difficulty difficulty = Difficulty.INSANE;
        if (SettingsController.Instance != null) {
            difficulty = SettingsController.Instance.difficulty;
            if (!SettingsController.Instance.isVersusAI) {
                ObjectUtility.disableGameObject(gameObject);
            }
        }
        switch (difficulty) {
            case Difficulty.EASY:
            FOLLOW_BALL_SPEED = (int) (BASE_BALL_FOLLOW_SPEED * 0.5);
            MOVEMENT_SPEED = (int) (BASE_MOVEMENT_SPEED * 0.5);
            break;
            case Difficulty.MEDIUM: break;
            FOLLOW_BALL_SPEED = (int) (BASE_BALL_FOLLOW_SPEED * 0.7);
            MOVEMENT_SPEED = (int) (BASE_MOVEMENT_SPEED * 0.9);
            case Difficulty.HARD: break;
            FOLLOW_BALL_SPEED = (int) (BASE_BALL_FOLLOW_SPEED * 1);
            MOVEMENT_SPEED = (int) (BASE_MOVEMENT_SPEED * 2);
            case Difficulty.INSANE:
            FOLLOW_BALL_SPEED = (int) (BASE_BALL_FOLLOW_SPEED * 1);
            MOVEMENT_SPEED = (int) (BASE_MOVEMENT_SPEED * 3);
            break;
        }
        aiPlayerRigidBody = aIPlayer.GetComponent<Rigidbody2D>();
    }

    public void Init() {
        arenaWidth = GameController.Instance.DISTANCE_BETWEEN_WALLS; //Wall width
        halfArena = (arenaWidth / 2);
        nearestBall = ball;
        distanceFromGoal = GameController.Instance.DISTANCE_FROM_GOAL;
    }

    public void Update () {
        if (Vector2.Distance(aIPlayer.transform.position, target.transform.position) < minimumTargetDistance) {
            isArrived = true;
        }

        if (GameController.Instance.getIsDoubleBallMode()) {
            double nearestDistance = 20000;
            foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball")) {
                float distance = Vector3.Distance(aIPlayer.transform.position, ball.transform.position);
                if (nearestDistance != 0 && distance < nearestDistance) {
                    nearestDistance = Vector3.Distance(aIPlayer.transform.position, ball.transform.position);
                    nearestBall = ball;
                }
            }
            isFollowBall = true;
        }

        if (!isArrived) {
            Vector2 position = new Vector2(target.transform.position.x, aiPlayerRigidBody.position.y);
            Vector2 targetVector;
            if (!(isFollowBall || wildBall > 1)) {
                targetVector = new Vector2(target.transform.position.x, target.transform.position.y);
            } else {
                if (nearestBall == null) {
                    nearestBall = ball;
                }
                targetVector = new Vector2(target.transform.position.x, (float) MathUtility.roundToNearestTenth(nearestBall.transform.position.y));
            }
            Vector2 direction = (targetVector - position).normalized;
            Vector2 movementTargetPosition = position + direction * Time.deltaTime * MOVEMENT_SPEED;
            if (Vector2.Distance(position, targetVector) < (direction * Time.deltaTime * MOVEMENT_SPEED).y) {
                movementTargetPosition = position + direction * Vector2.Distance(position, targetVector);
                isArrived = true;
            }
            aiPlayerRigidBody.MovePosition(movementTargetPosition);
        }
    }

    public void calculateBallCrossPoint(int direction, double hitAngle, double hitPosition) {
        double xTotal = Math.Abs(MathUtility.roundToNearestTenth(distanceFromGoal - playerOne.transform.position.x));
        int scenario = determineScenario(direction, xTotal, hitAngle, hitPosition);
        switch (scenario) {
            case 1:
            double playerPositionY1 = playerOne.transform.position.y + hitPosition;
            double yPosition1 = MathUtility.roundToNearestTenth(playerPositionY1 - (xTotal / Math.Tan(hitAngle)));
            target.transform.position = new Vector3(distanceFromGoal, (float) yPosition1, 0);
            break;
            case 2:
            double distanceToWall2 = Math.Abs(halfArena - (playerOne.transform.position.y + hitPosition));
            double distanceToCollision2 = MathUtility.roundToNearestTenth(distanceToWall2 * Math.Tan(hitAngle));
            if (distanceToCollision2 > xTotal) {
                distanceToCollision2 = MathUtility.roundToNearestTenth(distanceToWall2 * Math.Tan(1.57 - hitAngle));
            }
            double yPosition2 = halfArena - (distanceToWall2 / distanceToCollision2 * (xTotal - distanceToCollision2));
            target.transform.position = new Vector3(distanceFromGoal, (float) yPosition2, 0);
            break;
            case 3:
            double playerPositionY3 = playerOne.transform.position.y + hitPosition;
            double yPosition3 = MathUtility.roundToNearestTenth(playerPositionY3 + (xTotal / Math.Tan(hitAngle)));
            target.transform.position = new Vector3(distanceFromGoal, (float) yPosition3, 0);
            break;
            case 4:
            double playerPositionY4 = playerOne.transform.position.y + hitPosition;
            double distanceToWall4 = Math.Abs(playerPositionY4 + halfArena);
            double distanceToCollision4 = distanceToWall4 * Math.Tan(hitAngle);
            if (distanceToCollision4 > xTotal) {
                double yNewPosition4 = MathUtility.roundToNearestTenth(playerPositionY4 - (xTotal / Math.Tan(hitAngle)));
                target.transform.position = new Vector3(distanceFromGoal, (float) yNewPosition4, 0);
                break;
            }
            double yPosition4 = -halfArena + (distanceToWall4 / distanceToCollision4 * (xTotal - distanceToCollision4));
            target.transform.position = new Vector3(distanceFromGoal, (float) yPosition4, 0);
            break;
            case 5:
            target.transform.position = new Vector3(distanceFromGoal, playerOne.transform.position.y, 0);
            break;
            case 6:
            double distanceToWall6 = Math.Abs(halfArena - (playerOne.transform.position.y + hitPosition));
            double distanceToCollision6 = MathUtility.roundToNearestTenth(distanceToWall6 * Math.Tan(hitAngle));
            if (distanceToCollision6 > xTotal) {
                distanceToCollision6 = distanceToWall6 * Math.Tan(1.57 - hitAngle);
            }
            double yPosition6 = halfArena - (distanceToWall6 / distanceToCollision6 * (xTotal - distanceToCollision6));
            target.transform.position = new Vector3(distanceFromGoal, (float) yPosition6, 0);
            break;
            case 7:
            double positionY7 = playerOne.transform.position.y + hitPosition;
            double distanceToWall7 = Math.Abs(positionY7 + halfArena);
            double distanceToCollision7 = MathUtility.roundToNearestTenth(distanceToWall7 * Math.Tan(hitAngle));
            if (distanceToCollision7 > xTotal) {
                distanceToCollision7 = distanceToWall7 * Math.Tan(1.57 - hitAngle);
            }
            double yPosition7 = -halfArena + (distanceToWall7 / distanceToCollision7 * (xTotal - distanceToCollision7));
            target.transform.position = new Vector3(distanceFromGoal, (float) yPosition7, 0);
            break;
            case 8:
            double playerPositionY8 = playerOne.transform.position.y + hitPosition;
            double yPosition8 = MathUtility.roundToNearestTenth(playerPositionY8 - (xTotal * Math.Tan(1.57 - hitAngle)));
            target.transform.position = new Vector3(distanceFromGoal, (float) yPosition8, 0);
            break;
        }
        if (target.transform.position.y > halfArena || target.transform.position.y < -halfArena) {
            target.transform.position = new Vector3(distanceFromGoal, 0, 0);
        }

        minimumTargetDistance = RandomUtility.doMistakes();
        isArrived = false;
    }

    public void reset() {
        target.transform.position = new Vector3(distanceFromGoal, 0, 0);
        isArrived = false;
        isFollowBall = false;
    }

    public int determineScenario(int direction, double xTotal, double hitAngle, double hitPosition) {
        double positionY = playerOne.transform.position.y + hitPosition;
        if (direction == 1 && positionY > 0) {
            double y = positionY + (xTotal / Math.Tan(hitAngle));
            if (y > halfArena) {
                return 2;//+
            } else {
                return 3; //++
            }
        } else if (direction == 1 && positionY <= 0) {
            double y = positionY + (xTotal / Math.Tan(hitAngle));
            if (y > halfArena) {
                return 6;//+
            } else {
                return 3;//+
            }
        } else if (direction == -1 && positionY < 0) {
            double y = positionY - (xTotal / Math.Tan(1.57 - hitAngle));
            if (y < -halfArena) {
                return 4;//+
            } else {
                return 1;
            }
        } else if (direction == -1 && positionY >= 0) {
            double y = positionY - (xTotal / Math.Tan(hitAngle));
            if (y < -halfArena) {
                return 7; //+
            } else {
                return 8;//+
            }
        } else if (direction == 0) {
            return 0;
        } else {
            isFollowBall = true;
            return 0;
        }
    }
}
