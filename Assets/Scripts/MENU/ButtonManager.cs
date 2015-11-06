using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

//MainMenuManager
public class ButtonManager : MonoBehaviour {
//    AudioSource source;

    public GameObject multiPlayer;
    public GameObject aISelection;
    public GameObject spendCoins;
    public GameObject credits;
    public GameObject settings;

    public GameObject[] mainMenu;

    public GameObject music;
    public GameObject vibrate;
    public GameObject disabled1;
    public GameObject disabled2;

    void Awake() {
        hideSubMenus();
    }

    private void hideSubMenus() {
        multiPlayer.SetActive(false);
        aISelection.SetActive(false);
        spendCoins.SetActive(false);
        credits.SetActive(false);
        settings.SetActive(false);
    }

    public void LoadPlayerVersusAI(int difficulty) {
        MusicController.Instance.playImpact();
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
        MusicController.Instance.playImpact();
        SettingsController.Instance.isVersusAI = false;
        switch (multiplayerType) {
            case 1: SettingsController.Instance.setMultiplayerType(MultiplayerType.LOCAL); break;
            case 2: SettingsController.Instance.setMultiplayerType(MultiplayerType.BLUETOOTH); break;
            case 3: SettingsController.Instance.setMultiplayerType(MultiplayerType.INTERNET); break;
        }
        Application.LoadLevel(1);
    }

    public void showPlayerVersusAI() {
        MusicController.Instance.playImpact();
        hideSubMenus();
        SettingsController.Instance.isVersusAI = true;
        aISelection.SetActive(true);
    }

    public void showSettings() {
        MusicController.Instance.playImpact();
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
        MusicController.Instance.playImpact();
        hideSubMenus();
        SettingsController.Instance.isVersusAI = false;
        multiPlayer.SetActive(true);
    }

    public void ShowSpendCoins() {
        MusicController.Instance.playImpact();
        hideSubMenus();
        spendCoins.SetActive(true);
    }

    public void ShowCredits() {
        MusicController.Instance.playImpact();
        hideSubMenus();
        credits.SetActive(true);
    }

    public void quit() {
        Application.Quit();
    }

    public void LoadWatchAd() { //TODO make an Ad Manager
        MusicController.Instance.playImpact();
        if (Advertisement.IsReady()) {
            Advertisement.Show(null, new ShowOptions {
                resultCallback = result => {
                    Debug.Log(result.ToString());
                    if (result.ToString().Equals("Finished")) {
                        SettingsController.Instance.addCoins(100);
                        UI.Instance.showTotals();
                        if (UI.Instance != null) {
                            UI.Instance.enoughCoins();
                        }
                    }
                }
            });
        }
    }
}