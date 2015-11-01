using UnityEngine;
using System.Collections;

public class BaseSpecialManager : TimedPowerUp {

    public Sprite[] ballSprites;
    public Sprite[] logoSprites;
    
    public GameObject logo;
    public GameObject particles;

    public Color buff;
    public Color neutral;
    public Color coin;
    public Color nerf;

    public override void Start() {
        base.Start();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Ball") {
            switch (powerUpType) {
                case PowerUpType.LONG:
                SpecialController.Instance.makeRacketLonger(collision.gameObject.transform.position, gameObject.transform.position);
                break;
                case PowerUpType.SHORT:
                SpecialController.Instance.makeRacketShorter(collision.gameObject.transform.position, gameObject.transform.position);
                break;
                case PowerUpType.DOUBLE:
                SpecialController.Instance.makeDoubleBall(collision.gameObject);
                break;
                case PowerUpType.SPEED:
                SpecialController.Instance.makeBallFaster();
                break;
                case PowerUpType.POINT:
                GameController.Instance.goalScored(GameController.Instance.getLastTouchedBy());
                break;
                case PowerUpType.NO_GOALS:
                SpecialController.Instance.disableGoals();
                break;
                case PowerUpType.SHIELD:
                SpecialController.Instance.enableShields();
                break;
                case PowerUpType.HOMING:
                SpecialController.Instance.disableGoals();
                break;
                case PowerUpType.MOVING_GOALS:
                SpecialController.Instance.enableMovingGoals();
                break;
            }
            GameController.Instance.explodeSpecial(gameObject.transform.position);
        }
        if (collision.gameObject.name == "RacketLeft" || collision.gameObject.name == "RacketRight") {
            switch (powerUpType) {
                case PowerUpType.COIN:
                SpecialController.Instance.addCoin(1);
                break;
            }
            GameController.Instance.explodeCoin(gameObject.transform.position);
        }
        SelfDestruct();
    }

    public override void setUpPowerUp(PowerUpType powerUpType) {
        float arenaHalfLength = GameController.Instance.ARENA_HALF_LENGTH;
        gameObject.transform.position = new Vector3(0,
        RandomUtility.randomNegativeOrPositive() * Random.Range(100, arenaHalfLength),
        0);
        switch (powerUpType) {
            case PowerUpType.LONG:
            logo.GetComponent<SpriteRenderer>().sprite = logoSprites[0];
            particles.GetComponent<ParticleSystem>().startColor = buff;
            GetComponent<SpriteRenderer>().sprite = ballSprites[0];
            break;
            case PowerUpType.SHIELD:
            logo.GetComponent<SpriteRenderer>().sprite = logoSprites[1];
            particles.GetComponent<ParticleSystem>().startColor = buff;
            GetComponent<SpriteRenderer>().sprite = ballSprites[0];
            break;
            case PowerUpType.SHORT:
            logo.GetComponent<SpriteRenderer>().sprite = logoSprites[5];
            particles.GetComponent<ParticleSystem>().startColor = nerf;
            GetComponent<SpriteRenderer>().sprite = ballSprites[1];
            break;
            case PowerUpType.SPEED:
            logo.GetComponent<SpriteRenderer>().sprite = logoSprites[2];
            particles.GetComponent<ParticleSystem>().startColor = neutral;
            GetComponent<SpriteRenderer>().sprite = ballSprites[2];
            break;
            case PowerUpType.DOUBLE:
            logo.GetComponent<SpriteRenderer>().sprite = logoSprites[4];
            particles.GetComponent<ParticleSystem>().startColor = nerf;
            GetComponent<SpriteRenderer>().sprite = ballSprites[1];
            break;
            case PowerUpType.NO_GOALS:
            logo.GetComponent<SpriteRenderer>().sprite = logoSprites[6];
            particles.GetComponent<ParticleSystem>().startColor = neutral;
            GetComponent<SpriteRenderer>().sprite = ballSprites[2];
            break;
            case PowerUpType.MOVING_GOALS:
            logo.GetComponent<SpriteRenderer>().sprite = logoSprites[9];
            particles.GetComponent<ParticleSystem>().startColor = neutral;
            GetComponent<SpriteRenderer>().sprite = ballSprites[2];
            break;
            case PowerUpType.COIN:
            logo.GetComponent<SpriteRenderer>().sprite = logoSprites[10];
            gameObject.transform.position += new Vector3(RandomUtility.randomNegativeOrPositive() * GameController.DISTANCE_FROM_GOAL, 0, 0);
            particles.GetComponent<ParticleSystem>().startColor = coin;
            GetComponent<SpriteRenderer>().sprite = ballSprites[3];
            break;
        }
    }
}
