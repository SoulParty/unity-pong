using UnityEngine;

public class MusicController : MonoBehaviour {
    public GameObject pingImpact;
    public GameObject pongImpact;
    public GameObject click;
    public GameObject special;
    public GameObject win;
    public GameObject coin;
    public GameObject goal;

    bool ping = true;

    [System.NonSerialized]
    public static MusicController Instance;

    public MusicController() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Awake() {
        if (!this.Equals(Instance)) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void playWin() {
        if (checkMute())
            win.GetComponent<AudioSource>().Play();
    }

    public void playSpecial() {
        if (checkMute())
            special.GetComponent<AudioSource>().Play();
    }

    public void playImpact() {
        if (checkMute()) {
            if (ping) {
                pingImpact.GetComponent<AudioSource>().Play();
                ping = false;
            } else {
                pongImpact.GetComponent<AudioSource>().Play();
                ping = true;
            }
        }
    }

    public void playClick() {
        if (checkMute())
            click.GetComponent<AudioSource>().Play();
    }

    public void playCoin() {
        if (checkMute())
            coin.GetComponent<AudioSource>().Play();
    }

    public void playGoal() {
        if (checkMute())
            goal.GetComponent<AudioSource>().Play();
    }

    private bool checkMute() {
        if (!SettingsController.Instance.isMusic) {
            return false;
        } else {
            return true;
        }
    }
}
