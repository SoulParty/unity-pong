using UnityEngine;

public class MusicController : MonoBehaviour {
    public GameObject impact;
    public GameObject special;
    public GameObject win;
    public GameObject coin;
    public GameObject goal;

    [System.NonSerialized]
    public static MusicController Instance;

    public MusicController() {
        Instance = this;
    }

    void Awake() {
        if (Instance) {
            DestroyImmediate(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void playWin() {
        win.GetComponent<AudioSource>().Play();
    }

    public void playSpecial() {
        special.GetComponent<AudioSource>().Play();
    }

    public void playImpact() {
        impact.GetComponent<AudioSource>().Play();
    }

    public void playCoin() {
        coin.GetComponent<AudioSource>().Play();
    }

    public void playGoal() {
        goal.GetComponent<AudioSource>().Play();
    }
}
