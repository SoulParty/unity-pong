using UnityEngine;

public class PowerUp : MonoBehaviour {

    protected PowerUpType powerUpType;

    public void setPowerUpType(PowerUpType powerUpType) {
        this.powerUpType = powerUpType;
        setUpPowerUp(powerUpType);
    }

    public virtual void setUpPowerUp(PowerUpType powerUpType) {

    }

    protected virtual void SelfDestruct() {
        Destroy(gameObject);
    }
}
