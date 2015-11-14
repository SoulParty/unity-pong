using Mono.Xml.Xsl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    public GameObject background;

    public Sprite selectedPlayer1Racket;
    public Sprite selectedPlayer2Racket;
    public Sprite selectedPuck;

    private Dictionary<string, Sprite> ownedPuckSpritesCache = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> ownedRacketSpritesCache = new Dictionary<string, Sprite>();

    public Sprite[] puckLogos;
    public Sprite[] racketLogos;

    public GameObject adButton;

    [SerializeField]
    private int coins = 0;

    public bool isVersusAI = true;

    [System.NonSerialized]
    public static SettingsController Instance;

    public Difficulty difficulty = Difficulty.INSANE;
    public MultiplayerType multiplayerType = MultiplayerType.LOCAL;

    [SerializeField]
    private bool _isVibrate;
    public bool isVibrate {
        get {
            return _isVibrate;
        }
        set {
            _isVibrate = value;
            PlayerPrefs.SetInt(Const.VIBRATE, Convert.ToInt32(value));
        }
    }

    [SerializeField]
    private bool _isMusic;
    public bool isMusic {
        get {
            return _isMusic;
        }
        set {
            _isMusic = value;
            PlayerPrefs.SetInt(Const.MUSIC, Convert.ToInt32(value));
        }
    }

    public SettingsController() {
        Instance = this;
    }

    void Start () {
        if (GameObject.FindObjectsOfType<SettingsController>().Length > 1) {
            Destroy(gameObject);
        }

        Advertisement.Initialize("82920", true);
        StartCoroutine(ShowAdButtonWhenReady(adButton));

        coins = PlayerPrefs.GetInt(Const.COINS);

        loadSprites(SpriteType.PUCK);
        selectedPuck = getSpriteFromCache(SpriteType.PUCK, SpriteDao.Instance.getSelected(SpriteType.PUCK));

        loadSprites(SpriteType.RACKET1);
        selectedPlayer1Racket = getSpriteFromCache(SpriteType.RACKET1, SpriteDao.Instance.getSelected(SpriteType.RACKET1));
        selectedPlayer2Racket = getSpriteFromCache(SpriteType.RACKET2, SpriteDao.Instance.getSelected(SpriteType.RACKET2));
    }

    public void Awake () {
        DontDestroyOnLoad(gameObject);
        isVibrate = PlayerPrefs.GetInt(Const.VIBRATE) == 0 ? false : true;
    }

    public IEnumerator ShowAdButtonWhenReady(GameObject adButton) {
        while (!Advertisement.IsReady()) {
            yield return null;
        }
        ObjectUtility.enableGameObject(adButton);
    }

    public int getMaxCombo() {
        return PlayerPrefs.GetInt(Const.MAX_COMBO);
    }

    public int getCoins() {
        return PlayerPrefs.GetInt(Const.COINS);
    }

    public void setCoins(int coins) {
        if (coins <= 0) {
            coins = 0;
        }
        PlayerPrefs.SetInt(Const.COINS, coins);
        this.coins = coins;
        if (UI.Instance != null) {
            UI.Instance.refreshCoins(coins);
        }
    }

    public Difficulty getAIDifficulty() {
        return difficulty;
    }

    public void setAIDifficulty(Difficulty difficulty) {
        this.difficulty = difficulty;
    }

    public void setMultiplayerType(MultiplayerType multiplayerType) {
        this.multiplayerType = multiplayerType;
    }

    public void setSelected(Sprite sprite, SpriteType spriteType) {
        switch (spriteType) {
            case SpriteType.PUCK:this.selectedPuck = sprite; break;
            case SpriteType.RACKET1:this.selectedPlayer1Racket = sprite; break;
            case SpriteType.RACKET2:this.selectedPlayer2Racket = sprite; break;
        }
    }

    public void setPuck(Sprite puck) {
        this.selectedPuck = puck;
    }

    public void setPlayer1Racket(Sprite racket1) {
        this.selectedPlayer1Racket = racket1;
    }

    public void setPlayer2Racket(Sprite racket2) {
        this.selectedPlayer2Racket = racket2;
    }

    public bool checkFunds(int cost) {
        if (getCoins() < cost) {
            return false;
        } else {
            return true;
        }
    }

    public Sprite[] getPuckSprites() {
        return puckLogos;
    }

    public Sprite[] getRacketSprites() {
        return racketLogos;
    }

    public Sprite[] loadSprites(SpriteType spriteType) {
        Debug.Log("-------- LOADING SPRITES ---------");
        if (spriteType == SpriteType.PUCK) {
            foreach (Sprite sprite in getPuckSprites()) {
                if (sprite != null) {
                    int status = PlayerPrefs.GetInt(sprite.ToString());
                    ownedPuckSpritesCache.Add(sprite.ToString(), sprite);
                    Debug.Log("Puck sprite: " + sprite.ToString() + "status: " + status);
                }
            }
            return getPuckSprites();
        } else {
            foreach (Sprite sprite in getRacketSprites()) {
                if (sprite != null && !ownedRacketSpritesCache.ContainsKey(sprite.ToString())) {
                    int status = PlayerPrefs.GetInt(sprite.ToString());
                    ownedRacketSpritesCache.Add(sprite.ToString(), sprite);
                    Debug.Log("Racket sprite: " + sprite.ToString() + "status: " + status);
                }
            }
            return getRacketSprites();
        }
    }

    public Sprite getSpriteFromCache(SpriteType spriteType, string sprite) {
        if (spriteType.Equals(SpriteType.PUCK)) {
            return ownedPuckSpritesCache.FirstOrDefault(x => x.Key == sprite).Value;
        } else {
            return ownedRacketSpritesCache.FirstOrDefault(x => x.Key == sprite).Value;
        }
    }

    public void addCoins(int reward) {
        coins += reward;
        PlayerPrefs.SetInt(Const.COINS, coins);
    }
}
