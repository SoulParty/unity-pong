using UnityEngine;

public class BaseSpecialManager : TimedPowerUp {

    public Sprite[] ballSprites;

    public Sprite[] logoSprites;

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
                case PowerUpType.COIN:
                SpecialController.Instance.addCoin(1);
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

        }
        if (collision.gameObject.name == "RacketLeft" || collision.gameObject.name == "RacketRight") {
            switch (powerUpType) {
                case PowerUpType.COIN:
                SpecialController.Instance.addCoin(1);
                break;
            }

        }
        GameController.Instance.startSpecialAnimation(powerUpType);
        SelfDestruct();
    }

    public override void setUpPowerUp(PowerUpType powerUpType) {
        float arenaHalfLength = GameController.Instance.ARENA_HALF_LENGTH;
        gameObject.transform.position = new Vector3(0, Random.Range(-arenaHalfLength, arenaHalfLength), 0);
        switch (powerUpType) {
            case PowerUpType.HOMING:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[8];
            GetComponent<SpriteRenderer>().sprite = ballSprites[0];
            break;
            case PowerUpType.LONG:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[0];
            GetComponent<SpriteRenderer>().sprite = ballSprites[0];
            break;
            case PowerUpType.SHIELD:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[1];
            GetComponent<SpriteRenderer>().sprite = ballSprites[0];
            break;
            case PowerUpType.POINT:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[5];
            GetComponent<SpriteRenderer>().sprite = ballSprites[0];
            break;
            case PowerUpType.SHORT:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[1];
            GetComponent<SpriteRenderer>().sprite = ballSprites[1];
            break;
            case PowerUpType.SPEED:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[2];
            GetComponent<SpriteRenderer>().sprite = ballSprites[1];
            break;
            case PowerUpType.DOUBLE:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[4];
            GetComponent<SpriteRenderer>().sprite = ballSprites[2];
            break;
            case PowerUpType.NO_GOALS:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[6];
            GetComponent<SpriteRenderer>().sprite = ballSprites[2];
            break;
            case PowerUpType.MOVING_GOALS:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[9];
            GetComponent<SpriteRenderer>().sprite = ballSprites[2];
            break;
            case PowerUpType.COIN:
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = logoSprites[10];
            gameObject.transform.position += new Vector3(RandomUtility.randomNegativeOrPositive() * GameController.DISTANCE_FROM_GOAL, 0, 0);
            GetComponent<SpriteRenderer>().sprite = ballSprites[3];
            break;
        }
    }
}
