using Mono.Xml.Xsl;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class BallManager : MonoBehaviour, SpriteChangeable {

    public float speed;
    public float speedCapMultiplier = 50;
    private float speedCap;
    private enum TrailColor : int {
        WHITE = 0,
        BLUE = 1,
        ORANGE = 2,
        RED = 3,
    }

    void Start() {
        speed = GameController.Instance.defaultBallStartSpeed;
        speedCap = GameController.Instance.ballSpeedUp * speedCapMultiplier + GameController.Instance.defaultBallStartSpeed;
        // Initial Velocity
        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
    }

    public void setSprite(Sprite sprite) {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Note: 'col' holds the collision information. If the
        // Ball collided with a racket, then:
        //   col.gameObject is the racket
        //   col.transform.position is the racket's position
        //   col.collider is the racket's collider

        // Hit the left Racket?
        bool hitRacket = false;
        if (SettingsController.Instance.isVersusAI) {
            PongAIController.Instance.wildBall++;
        }

        if (collision.gameObject.name == "RacketLeft") {
            // Calculate hit Factor
            float y = hitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1, y).normalized;

            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            hitRacket = true;
            if (SettingsController.Instance.isVersusAI && y != 0) {
                PongAIController.Instance.isFollowBall = false;
                //Debug.Log("Angle (orig)" + Vector2.Angle(new Vector2(1, 0), dir));
                double angle = Math.Abs(90 - Vector2.Angle(new Vector2(1, 0), dir));
                //                angle = MathUtility.angleSanityCheck(angle);
                y = y * collision.collider.bounds.size.y;
                if (y > 0) {
                    //Debug.Log("Angle " + angle);
                    PongAIController.Instance.calculateBallCrossPoint(1, MathUtility.convertToRadians(angle), y);
                } else if (y < 0) {
                    //Debug.Log("Angle " + angle);
                    PongAIController.Instance.calculateBallCrossPoint(-1, MathUtility.convertToRadians(angle), y);
                } else {
                    PongAIController.Instance.calculateBallCrossPoint(0, MathUtility.convertToRadians(angle), y);
                }
            }
            if (SettingsController.Instance.isVersusAI) {
                PongAIController.Instance.wildBall = 0;
            }
            increaseSpeed(GameController.Instance.ballSpeedUp);
        }

        // Hit the right Racket?
        if (collision.gameObject.name == "RacketRight") {
            // Calculate hit Factor
            float y = hitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1, y).normalized;

            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            hitRacket = true;
            if (SettingsController.Instance.isVersusAI) {
                PongAIController.Instance.wildBall = 0;
            }
            increaseSpeed(GameController.Instance.ballSpeedUp);
        }

        //        //Debug.Log("Speed:" + speed.ToString());
        if (SettingsController.Instance.isVersusAI &&
        (collision.gameObject.name == "leftWall" || collision.gameObject.name == "leftWall2")
        || collision.gameObject.name == "rightWall" || collision.gameObject.name == "rightWall2") {
            PongAIController.Instance.isFollowBall = true;
        }

        if (hitRacket) {
            GameController.Instance.incrementCombo();
        }
    }

    //TODO add more complex angles
    private float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight) {
        // ascii art:
        // ||  1 <- at the top of the racket
        // ||
        // ||  0 <- at the middle of the racket
        // ||
        // || -1 <- at the bottom of the racket
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    public void reset() {
        speed = GameController.Instance.defaultBallStartSpeed;
        transform.position = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void moveInRandomDirection() {
        GetComponent<Rigidbody2D>().isKinematic = false;
        var direction = RandomUtility.randomNegativeOrPositive();
        GetComponent<Rigidbody2D>().velocity = direction * Vector2.right * speed;
    }

    public void increaseSpeed(int speedUp) {
        if (speed < speedCap) {
            speed += speedUp;
            changeTrail();
        }
    }

    public void changeTrail() {
        if (speed > GameController.Instance.defaultBallStartSpeed * 2.5) {
            gameObject.transform.GetChild((int) TrailColor.ORANGE).GetComponent<TrailRenderer>().enabled = false;
            gameObject.transform.GetChild((int) TrailColor.RED).GetComponent<TrailRenderer>().enabled = true;
        } else if (speed > GameController.Instance.defaultBallStartSpeed * 2) {
            gameObject.transform.GetChild((int) TrailColor.BLUE).GetComponent<TrailRenderer>().enabled = false;
            gameObject.transform.GetChild((int) TrailColor.ORANGE).GetComponent<TrailRenderer>().enabled = true;
        } else if (speed > GameController.Instance.defaultBallStartSpeed * 1.4) {
            gameObject.transform.GetChild((int) TrailColor.WHITE).GetComponent<TrailRenderer>().enabled = false;
            gameObject.transform.GetChild((int) TrailColor.BLUE).GetComponent<TrailRenderer>().enabled = true;
        } else if (speed < GameController.Instance.defaultBallStartSpeed * 1.4) {
            gameObject.transform.GetChild((int) TrailColor.RED).GetComponent<TrailRenderer>().enabled = false;
            gameObject.transform.GetChild((int) TrailColor.ORANGE).GetComponent<TrailRenderer>().enabled = false;
            gameObject.transform.GetChild((int) TrailColor.BLUE).GetComponent<TrailRenderer>().enabled = false;
            gameObject.transform.GetChild((int) TrailColor.WHITE).GetComponent<TrailRenderer>().enabled = true;
        }
    }
}
