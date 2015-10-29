using System.Collections;
using UnityEngine;

//TODO fix special stacking
public class SpecialController : MonoBehaviour{

    public BallManager ballManager;
    public RacketManager leftRacketManager;
    public RacketManager rightRacketManager;

    public int SPEED_UP = 300;

    public float SHIELD_DURATION = 5;

    public float NO_GOALS_DURATION = 5;

    public int MOVING_GOAL_AMOUNT = 4;
    public float MOVING_GOALS_DURATION = 10;
    public float MOVING_GOALS_STEP = 0.04f;

    [System.NonSerialized]
    public static SpecialController Instance;

    public SpecialController() {
        Instance = this;
    }

    public void makeRacketLonger(Vector3 colliderPosition, Vector3 specialPosition) {
        if (colliderPosition.x - specialPosition.x > 0) {
            rightRacketManager.makeRacketLonger();
        } else {
            leftRacketManager.makeRacketLonger();
        }
    }

    public void makeRacketShorter(Vector3 colliderPosition, Vector3 specialPosition) {
        if (colliderPosition.x - specialPosition.x > 0) {
            rightRacketManager.makeRacketShorter();
        } else {
            leftRacketManager.makeRacketShorter();
        }
    }

    public void makeDoubleBall(GameObject special) {
//        GameObject ball = (GameObject) Instantiate(Resources.Load("Ball"));
//        Vector3 position = ball.transform.position;
//        position = new Vector3(position.x, special.transform.position.y, position.z);
        GameObject ball = Instantiate(GameController.Instance.ball);
        ball.transform.position = new Vector3(ball.transform.position.x, special.transform.position.y, ball.transform.position.z);
        ball.GetComponent<BallManager>().moveInRandomDirection();
        GameController.Instance.toggleIsDoubleBallMode();
    }

    public void makeBallFaster() {
        ballManager.increaseSpeed(SPEED_UP);
    }

    public void addCoin(int coins) {
        coins += SettingsController.Instance.getCoins();
        SettingsController.Instance.setCoins(coins);
    }

    public void makeMovingGoals(GameObject special) {}

    public void makeNoGoals(GameObject special) {}

    public void makeHoming(GameObject special) {}

    public void makeShield(GameObject special) {}

    public void spawnRandom() {
        GameObject special = (GameObject) Instantiate(Resources.Load("Special"));
        switch (Random.Range(1, 8)) {
//        switch (8) {
            case 1: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.LONG); break;
            case 2: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.SHORT); break;
            case 3: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.DOUBLE); break;
            case 4: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.SPEED); break;
            case 5: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.MOVING_GOALS); break;
            case 6: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.NO_GOALS); break;
            case 7: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.SHIELD); break;
//            case 8: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.POINT); break;
//            case 9: special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.HOMING); break;
        }
    }

    public void spawnCoin() {
        GameObject special = (GameObject) Instantiate(Resources.Load("Coin"));
        special.transform.position = new Vector3(0, 0, 0);
        special.GetComponent<BaseSpecialManager>().setPowerUpType(PowerUpType.COIN);
    }

    public void toggleShield(GameObject player) {
        player.GetComponent<RacketManager>().toggleShield();
        StartCoroutine(stopShield(player));
    }

    public IEnumerator stopShield(GameObject player) {
        yield return new WaitForSeconds(SHIELD_DURATION);
        player.GetComponent<RacketManager>().toggleShield();
    }

    public void disableGoals() {
        ArenaController.Instance.toggleGoals();
    }


    public void enableMovingGoals() {
        ArenaController.Instance.enableMovingGoals();
    }

    public void enableShields() {
        SpecialController.Instance.toggleShield(GameController.Instance.getLastTouchedBy());
    }

    public void reset() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Special")) {
            Destroy(go);
        }
        if (GameController.Instance.getIsDoubleBallMode()) {
            GameController.Instance.toggleIsDoubleBallMode();
        }
        if (SettingsController.Instance.isVersusAI) {
            PongAIController.Instance.wildBall = 0;
        }
    }
}
