using System.Collections;
using UnityEngine;

public class RacketManager : MonoBehaviour, SpriteChangeable {

    private bool enlarged = false;
    private bool shrunken = false;
    private bool isShielded = false;

    public GameObject logo;

    public void makeRacketLonger() {
        if (!enlarged) {
            enlarge();
            enlarged = true;
            Invoke("shrink", 5f);
        }
    }

    public void makeRacketShorter() {
        if (!shrunken) {
            shrink();
            shrunken = true;
            Invoke("enlarge", 5f);
        }
    }

    public void toggleShield() {
        if (isShielded) {
            ObjectUtility.disableGameObject(ObjectUtility.findChild(gameObject, "Shield"));
        } else {
            ObjectUtility.enableGameObject(ObjectUtility.findChild(gameObject, "Shield"));
        }
        isShielded = !isShielded;
    }

    void enlarge() {
        gameObject.transform.localScale += new Vector3(0, 0.5f, 0);
        shrunken = false;
    }

    void shrink() {
        gameObject.transform.localScale -= new Vector3(0, 0.5f, 0);
        enlarged = false;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name.Equals("Ball")) {
            GameController.Instance.setLastTouchedBy(this.gameObject);
        }
    }

    public void setSprite(Sprite sprite) {
        logo.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
