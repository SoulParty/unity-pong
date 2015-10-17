using UnityEngine;

public class RandomUtility {
    public static int randomNegativeOrPositive() {
        var randomRange = Random.Range(-1.0f, 1.0f);
        if (randomRange >= 0) {
//            Debug.Log("Left " + randomRange);
            return 1;
        } else {
//            Debug.Log("Right " + randomRange);
            return -1;
        }
    }

    public static void setSlowDownByDifficulty() {
        int followSpeed = 0;
        int movementSpeed = 0;
        switch (SettingsController.Instance.getAIDifficulty()) {
            case Difficulty.EASY:
                if (Random.Range(0,1f) < 0.6) {
                    followSpeed = 300;
                    movementSpeed = 400;
                } else {
                    followSpeed = 500;
                    movementSpeed = 600;
                }
            break;
            case Difficulty.MEDIUM:
            if (Random.Range(0,1f) < 0.4) {
                followSpeed = 500;
                movementSpeed = 600;
            } else {
                followSpeed = 700;
                movementSpeed = 800;
            }
            break;
            case Difficulty.HARD:
            if (Random.Range(0,1f) < 0.2) {
                followSpeed = 700;
                movementSpeed = 800;
            } else {
                followSpeed = 900;
                movementSpeed = 1000;
            }
            break;
            case Difficulty.INSANE:
            if (Random.Range(0,1f) < 0.05) {
                followSpeed = 600;
                movementSpeed = 900;
            } else {
                followSpeed = 1100;
                movementSpeed = 1200;
            }
            break;
        }
        PongAIController.Instance.FOLLOW_BALL_SPEED = followSpeed;
        PongAIController.Instance.MOVEMENT_SPEED = movementSpeed;
    }
}
