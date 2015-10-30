using System.Collections;
using UnityEngine;

public class HitAnimationManager : MonoBehaviour {

    public float IMPACT_LENGTH = 2f;
    public int direction;

    public enum Orientation {
        HORIZONTAL, VERTiCAL
    }

    public Orientation orientation = Orientation.HORIZONTAL;
    public GameObject hitImpactParticles;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name.Equals("Ball")) {
            BallManager ballManager = collision.gameObject.GetComponent<BallManager>();
            float speed = ballManager.speed;
            if (speed > GameController.Instance.defaultBallStartSpeed * 3) {
                CameraShake.Instance.cameraShake(CameraShakeType.STRONG);
            } else if (speed > GameController.Instance.defaultBallStartSpeed * 2.2) {
                CameraShake.Instance.cameraShake(CameraShakeType.NORMAL);
            } else if (speed > GameController.Instance.defaultBallStartSpeed * 1.6) {
                CameraShake.Instance.cameraShake(CameraShakeType.NORMAL);
            } else {
                CameraShake.Instance.cameraShake(CameraShakeType.WEAK);
            }

            if (orientation.Equals(Orientation.HORIZONTAL)) {
                hitImpactParticles.transform.position = new Vector3(collision.gameObject.transform.position.x, hitImpactParticles.transform.position.y, 0);
            } else {
                hitImpactParticles.transform.position = new Vector3(hitImpactParticles.transform.position.x, collision.gameObject.transform.position.y, 0);
            }

            ObjectUtility.enableGameObject(hitImpactParticles);
            StartCoroutine(disableImpactTimer());
        }
        if (collision.gameObject.tag.Equals("Player")) {
            collision.gameObject.GetComponent<PlayerController>().direction = direction;
        }
    }

    IEnumerator disableImpactTimer() {
        yield return new WaitForSeconds(IMPACT_LENGTH);
        ObjectUtility.disableGameObject(hitImpactParticles);
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Player")) {
            collision.gameObject.GetComponent<PlayerController>().direction = 0;
        }
    }

}
