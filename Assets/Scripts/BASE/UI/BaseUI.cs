using UnityEngine;

public class BaseUI : MonoBehaviour{

    protected void activate(GameObject gameObject) {
        ObjectUtility.enableGameObject(gameObject);
    }

    protected void deactivate(GameObject gameObject) {
        ObjectUtility.disableGameObject(gameObject);
    }
}
