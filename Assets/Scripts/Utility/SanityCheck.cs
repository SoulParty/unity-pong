using UnityEngine;

public class SanityCheck : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "RacketRight") {
            collision.gameObject.transform.position = new Vector3(840, 0, 0);
        } else if (collision.gameObject.name == "RacketLeft") {
            collision.gameObject.transform.position = new Vector3(-840, 0, 0);
        } else if (collision.gameObject.name == "Ball") {
            BallController.Instance.resetBalls();
        }
    }
}
