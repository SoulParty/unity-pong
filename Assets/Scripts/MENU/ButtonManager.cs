using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

//MainMenuManager
public class ButtonManager : MonoBehaviour {
//    AudioSource source;

    public GameObject multiPlayer;
    public GameObject aISelection;
    public GameObject spendCoins;

    public Animator spendCoinsAnimator;
    public Animator watchAdAnimator;

    public GameObject credits;
    public GameObject settings;

    public GameObject[] mainMenu;

    public GameObject music;
    public GameObject vibrate;
    public GameObject disabled1;
    public GameObject disabled2;

    public GameObject watchAd;

    [System.NonSerialized]
    public static ButtonManager Instance;

    public ButtonManager() {
        Instance = this;
    }

    void Start() {
        hideSubMenus();
        StartCoroutine(ShowAdButtonWhenReady(watchAd));
    }

    public IEnumerator ShowAdButtonWhenReady(GameObject adButton) {
        while (!Advertisement.IsReady()) {
            yield return null;
        }
        ObjectUtility.enableGameObject(adButton);
    }

    private void hideSubMenus() {
        multiPlayer.SetActive(false);
        aISelection.SetActive(false);
        spendCoins.SetActive(false);
        credits.SetActive(false);
        settings.SetActive(false);
        spendCoinsAnimator.SetBool("notInShop", true);
        watchAdAnimator.SetBool("inShop", false);
    }

    public void LoadPlayerVersusAI(int difficulty) {
        pressSound();
        SettingsController.Instance.isVersusAI = true;
        switch (difficulty) {
            case 1: SettingsController.Instance.setAIDifficulty(Difficulty.EASY); break;
            case 2: SettingsController.Instance.setAIDifficulty(Difficulty.MEDIUM); break;
            case 3: SettingsController.Instance.setAIDifficulty(Difficulty.HARD); break;
            case 4: SettingsController.Instance.setAIDifficulty(Difficulty.INSANE); break;
        }
        Application.LoadLevel(1);
    }

    public void LoadPlayerVersusPlayer(int multiplayerType) {
        pressSound();
        SettingsController.Instance.isVersusAI = false;
        switch (multiplayerType) {
            case 1: SettingsController.Instance.setMultiplayerType(MultiplayerType.LOCAL); break;
            case 2: SettingsController.Instance.setMultiplayerType(MultiplayerType.BLUETOOTH); break;
            case 3: SettingsController.Instance.setMultiplayerType(MultiplayerType.INTERNET); break;
        }
        Application.LoadLevel(1);
    }

    public void showPlayerVersusAI() {
        pressSound();
        hideSubMenus();
        SettingsController.Instance.isVersusAI = true;
        aISelection.SetActive(true);
    }

    public void showSettings() {
        pressSound();
        hideSubMenus();
        settings.SetActive(true);
        disabled1.SetActive(!SettingsController.Instance.isVibrate);
        disabled2.SetActive(!SettingsController.Instance.isMusic);
    }

    public void toggleVibrations() {
        SettingsController.Instance.isVibrate = !SettingsController.Instance.isVibrate;
        disabled1.SetActive(!SettingsController.Instance.isVibrate);
    }

    public void toggleMusic() {
        SettingsController.Instance.isMusic = !SettingsController.Instance.isMusic;
        disabled2.SetActive(!SettingsController.Instance.isMusic);
    }

    public void showPlayerVersusPlayer() {
        pressSound();
        hideSubMenus();
        SettingsController.Instance.isVersusAI = false;
        multiPlayer.SetActive(true);
    }

    public void ShowSpendCoins() {
        pressSound();
        hideSubMenus();
        spendCoins.SetActive(true);
        spendCoinsAnimator.SetBool("notInShop", false);
        watchAdAnimator.SetBool("inShop", true);
    }

    public void ShowCredits() {
        pressSound();
        hideSubMenus();
        credits.SetActive(true);
    }

    public void quit() {
        Application.Quit();
    }

    public void LoadWatchAd() { //TODO make an Ad Manager
        pressSound();
        if (Advertisement.IsReady()) {
            Advertisement.Show(null, new ShowOptions {
                resultCallback = result => {
                    Debug.Log(result.ToString());
                    if (result.ToString().Equals("Finished")) {
                        SettingsController.Instance.addCoins(100);
                        SpriteService.Instance.display4CharNumber(
                                SpriteService.Instance.coinsTotal, SettingsController.Instance.getCoins());
                        SettingsController.Instance.playCoinsEarned(SpriteService.Instance.coinsEarnedImpact);
                        SpriteSelectionManager.refreshAll();
                    }
                }
            });
        }
    }

    private void pressSound() {
        MusicController.Instance.playClick();
    }
}