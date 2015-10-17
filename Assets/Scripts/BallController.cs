using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    public GameObject mainBall;
    private BallManager ballManager;

    [System.NonSerialized]
    public static BallController Instance;

    public BallController() {
        Instance = this;
    }

    public void Start() {
        ballManager = mainBall.GetComponent<BallManager>();
    }

    public void resetBalls() {
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball")) {
            if (!ball.Equals(mainBall)) {
                BallController.Instance.destroy(ball);
            }
        }
        ballManager.reset();
    }

    public void moveInRandomDirection() {
        ballManager.moveInRandomDirection();
    }

    public void destroy(GameObject ball) {
        Destroy(ball);
    }
}
