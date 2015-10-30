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

    private Dictionary<string, int> ownedPuckSpritesCache = new Dictionary<string, int>();
    private Dictionary<string, int> ownedRacket1SpritesCache = new Dictionary<string, int>();
    private Dictionary<string, int> ownedRacket2SpritesCache = new Dictionary<string, int>();

    public Sprite[] puckLogos;
    public Sprite[] racketLogos;

    public GameObject adButton;

    [SerializeField]
    private int maxCombo = 0;
    [SerializeField]
    private int coins = 0;

    public bool isVersusAI = true;

    [System.NonSerialized]
    public static SettingsController Instance;

    public Difficulty difficulty = Difficulty.INSANE;
    public MultiplayerType multiplayerType = MultiplayerType.LOCAL;

    public bool isVibrate = false;

    public SettingsController() {
        Instance = this;
    }

    void Start () {
        Advertisement.Initialize("82920", true);
        StartCoroutine(ShowAdButtonWhenReady(adButton));

        maxCombo = PlayerPrefs.GetInt(Const.MAX_COMBO);
        coins = PlayerPrefs.GetInt(Const.COINS);

        if (UI.Instance != null) {
            UI.Instance.showTotals();
        }
    }

    public IEnumerator ShowAdButtonWhenReady(GameObject adButton) {
        while (!Advertisement.IsReady()) {
            yield return null;
        }
        ObjectUtility.enableGameObject(adButton);
    }

    public void Awake () {
        DontDestroyOnLoad(gameObject);
    }

    public int getMaxCombo() {
        return maxCombo;
    }

    public int getCoins() {
        return coins;
    }

    public void setCoins(int coins) {
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

    public void setPuck(Sprite puck) {
        this.selectedPuck = puck;
        PlayerPrefs.SetInt(puck.ToString(), 2);
    }

    public void setPlayer1Racket(Sprite racket1) {
        this.selectedPlayer1Racket = racket1;
        PlayerPrefs.SetInt(racket1.ToString(), 3);
    }

    public void setPlayer2Racket(Sprite racket2) {
        this.selectedPlayer2Racket = racket2;
        PlayerPrefs.SetInt(racket2.ToString(), 4);
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

    public void setStatus(string sprite, int status) {
        PlayerPrefs.SetInt(sprite, status);
    }

    public Sprite[] loadSprites(SpriteType spriteType) {
        Debug.Log("-------- LOADING SPRITES ---------");
        if (spriteType == SpriteType.PUCK) {
            foreach (Sprite sprite in getPuckSprites()) {
                if (sprite != null) {
                    int status = PlayerPrefs.GetInt(sprite.ToString());
                    ownedPuckSpritesCache.Add(sprite.ToString(), status);
                    Debug.Log("Puck sprite: " + sprite.ToString() + "status: " + status);
                    if (status == 2) {
                        selectedPuck = sprite;
                        Debug.Log("Selected sprite: " + sprite.ToString() + "status: " + status);
                    }
                }
            }
            return getPuckSprites();
        } else if (spriteType == SpriteType.RACKET1) {
            foreach (Sprite sprite in getRacketSprites()) {
                if (sprite != null && !ownedRacket1SpritesCache.ContainsKey(sprite.ToString())) {
                    int status = PlayerPrefs.GetInt(sprite.ToString());
                    ownedRacket1SpritesCache.Add(sprite.ToString(), status);
                    Debug.Log("Racket 1 sprite: " + sprite.ToString() + "status: " + status);
                    if (status == 3) {
                        selectedPlayer1Racket = sprite;
                        Debug.Log("Selected racket 1: " + sprite.ToString() + "status: " + status);
                    }
                }
            }
            return getRacketSprites();
        } else {
            foreach (Sprite sprite in getRacketSprites()) {
                if (sprite != null && !ownedRacket2SpritesCache.ContainsKey(sprite.ToString())) {
                    int status = PlayerPrefs.GetInt(sprite.ToString());
                    Debug.Log("Racket 2 sprite: " + sprite.ToString() + "status: " + status);
                    if (status == 4) {
                        selectedPlayer2Racket = sprite;
                        Debug.Log("Selected racket 2: " + sprite.ToString() + "status: " + status);
                    }
                }
            }
            return getRacketSprites();
        }
    }

    public void setStatus(SpriteType spriteType, string sprite, int status) {
        PlayerPrefs.SetInt(sprite, status);
        if (spriteType == SpriteType.PUCK) {
            if (!ownedPuckSpritesCache.ContainsKey(sprite)) {
                ownedPuckSpritesCache.Add(sprite, status);
            } else {
                ownedPuckSpritesCache.Remove(sprite);
                ownedPuckSpritesCache.Add(sprite, status);
            }
        } else if (spriteType == SpriteType.RACKET1) {
            if (!ownedRacket1SpritesCache.ContainsKey(sprite)) {
                ownedRacket1SpritesCache.Add(sprite, status);
            } else {
                ownedRacket1SpritesCache.Remove(sprite);
                ownedRacket1SpritesCache.Add(sprite, status);
            }
        } else {
            if (!ownedRacket2SpritesCache.ContainsKey(sprite)) {
                ownedRacket2SpritesCache.Add(sprite, status);
            } else {
                ownedRacket2SpritesCache.Remove(sprite);
                ownedRacket2SpritesCache.Add(sprite, status);
            }
        }
    }

    public int spriteStatus(SpriteType spriteType, string sprite) {
        if (spriteType == SpriteType.PUCK) {
            return ownedPuckSpritesCache.FirstOrDefault(x => x.Key == sprite).Value;
        } else if (spriteType == SpriteType.RACKET1) {
            return ownedRacket1SpritesCache.FirstOrDefault(x => x.Key == sprite).Value;
        } else {
            return ownedRacket2SpritesCache.FirstOrDefault(x => x.Key == sprite).Value;
        }
    }

    public string spriteKeyByStatus(SpriteType spriteType, int status) {
        if (spriteType == SpriteType.PUCK) {
            return ownedPuckSpritesCache.FirstOrDefault(x => x.Value == status).Key;
        } else if (spriteType == SpriteType.RACKET1) {
            return ownedRacket1SpritesCache.FirstOrDefault(x => x.Value == status).Key;
        } else {
            return ownedRacket2SpritesCache.FirstOrDefault(x => x.Value == status).Key;
        }
    }

    public void addCoins(int reward) {
        coins += reward;
        PlayerPrefs.SetInt(Const.COINS, coins);
    }
}
