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

    TrailRenderer orangeTrail;
    TrailRenderer blueTrail;
    TrailRenderer redTrail;
    TrailRenderer whiteTrail;
    
    Rigidbody2D rigidBody2D;
    
    void Start() {
        speed = GameController.Instance.defaultBallStartSpeed;
        speedCap = GameController.Instance.ballSpeedUp * speedCapMultiplier + GameController.Instance.defaultBallStartSpeed;
        rigidBody2D = GetComponent<Rigidbody2D>();
        // Initial Velocity
        rigidBody2D.velocity = Vector2.right * speed;

        orangeTrail = gameObject.transform.GetChild((int) TrailColor.ORANGE).GetComponent<TrailRenderer>();
        blueTrail = gameObject.transform.GetChild((int) TrailColor.BLUE).GetComponent<TrailRenderer>();
        redTrail = gameObject.transform.GetChild((int) TrailColor.RED).GetComponent<TrailRenderer>();
        whiteTrail = gameObject.transform.GetChild((int) TrailColor.WHITE).GetComponent<TrailRenderer>();
        
        orangeTrail.sortingOrder = -2;
        blueTrail.sortingOrder = -2;
        redTrail.sortingOrder = -2;
        whiteTrail.sortingOrder = -2;
    }

    public void setSprite(Sprite sprite) {
        if (sprite != null) {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {

        bool hitRacket = false;
        if (SettingsController.Instance.isVersusAI) {
            PongAIController.Instance.wildBall++;
        }

        if (collision.gameObject.name == "RacketRight") {
            // Calculate hit Factor
            float y = hitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1, y).normalized;

            // Set Velocity with dir * speed
            rigidBody2D.velocity = dir * speed;
            hitRacket = true;
            if (SettingsController.Instance.isVersusAI && y != 0) {
                PongAIController.Instance.isFollowBall = false;

                double angle = Math.Abs(90 - Vector2.Angle(new Vector2(1, 0), dir));
                y = y * collision.collider.bounds.size.y;
                if (y > 0) {
                    PongAIController.Instance.calculateBallCrossPoint(1, MathUtility.convertToRadians(angle), y);
                } else if (y < 0) {
                    PongAIController.Instance.calculateBallCrossPoint(-1, MathUtility.convertToRadians(angle), y);
                } else {
                    PongAIController.Instance.calculateBallCrossPoint(0, MathUtility.convertToRadians(angle), y);
                }
            }
            if (SettingsController.Instance.isVersusAI) {
                PongAIController.Instance.wildBall = 0;
            }
            increaseSpeed(GameController.Instance.ballSpeedUp);
        } else if (collision.gameObject.name == "RacketLeft") {
            // Calculate hit Factor
            float y = hitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1, y).normalized;

            // Set Velocity with dir * speed
            rigidBody2D.velocity = dir * speed;
            hitRacket = true;
            if (SettingsController.Instance.isVersusAI) {
                PongAIController.Instance.wildBall = 0;
            }
            increaseSpeed(GameController.Instance.ballSpeedUp);
        } else { //This way the ball shouldn't loose speed after collisions
            rigidBody2D.velocity = rigidBody2D.velocity.normalized * speed;
        }

        //        //Debug.Log("Speed:" + speed.ToString());
        if (SettingsController.Instance.isVersusAI &&
        (collision.gameObject.name == "leftWall" || collision.gameObject.name == "leftWall2")
        || collision.gameObject.name == "rightWall" || collision.gameObject.name == "rightWall2") {
            PongAIController.Instance.isFollowBall = true;
        }

        if (hitRacket) {
            GameController.Instance.incrementCombo();
            MusicController.Instance.playImpact();
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
        rigidBody2D.isKinematic = true;
    }

    public void moveInRandomDirection() {
        rigidBody2D.isKinematic = false;
        var direction = RandomUtility.randomNegativeOrPositive();
        rigidBody2D.velocity = direction * Vector2.right * speed;
    }

    public void increaseSpeed(int speedUp) {
        if (speed < speedCap) {
            speed += speedUp;
            changeTrail();
        }
    }

    public void changeTrail() {
        if (speed > GameController.Instance.defaultBallStartSpeed * 2.5) {
            orangeTrail.enabled = false;
            redTrail.enabled = true;
        } else if (speed > GameController.Instance.defaultBallStartSpeed * 2) {
            blueTrail.enabled = false;
            orangeTrail.enabled = true;
        } else if (speed > GameController.Instance.defaultBallStartSpeed * 1.4) {
            whiteTrail.enabled = false;
            blueTrail.enabled = true;
        } else if (speed < GameController.Instance.defaultBallStartSpeed * 1.4) {
            redTrail.enabled = false;
            orangeTrail.enabled = false;
            blueTrail.enabled = false;
            whiteTrail.enabled = true;
        }
    }
}
