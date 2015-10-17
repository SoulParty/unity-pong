using UnityEngine;

public class ObjectUtility {

    public static void disableGameObject(GameObject gameObject) {
        gameObject.SetActive(false);
    }

    public static void enableGameObject(GameObject gameObject) {
        gameObject.SetActive(true);
    }

    public static GameObject findChild(GameObject gameObject, string childName) {
        return gameObject.transform.Find(childName).gameObject;
    }
}
