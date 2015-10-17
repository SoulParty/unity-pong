public class TimedPowerUp : PowerUp {

    public float powerUpSelfDestructTime = 5f;

    public virtual void Start() {
        Invoke("SelfDestruct", powerUpSelfDestructTime);
    }
}
