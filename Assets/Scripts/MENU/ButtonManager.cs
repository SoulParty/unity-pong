using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

//MainMenuManager
public class ButtonManager : MonoBehaviour {
//    AudioSource source;

    public GameObject multiPlayer;
    public GameObject aISelection;
    public GameObject spendCoins;
    public GameObject disabled;

    void Awake() {
        hideSubMenus();
    }

    private void hideSubMenus() {
        multiPlayer.SetActive(false);
        aISelection.SetActive(false);
        spendCoins.SetActive(false);
    }

    public void LoadPlayerVersusAI(int difficulty) {
//        source.Play();
//        StartCoroutine ("playSound", level);
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
        //        source.Play();
        //        StartCoroutine ("playSound", level);
        SettingsController.Instance.isVersusAI = false;
        switch (multiplayerType) {
            case 1: SettingsController.Instance.setMultiplayerType(MultiplayerType.LOCAL); break;
            case 2: SettingsController.Instance.setMultiplayerType(MultiplayerType.BLUETOOTH); break;
            case 3: SettingsController.Instance.setMultiplayerType(MultiplayerType.INTERNET); break;
        }
        Application.LoadLevel(1);
    }

    public void showPlayerVersusAI() {
        //        source.Play();
        //        StartCoroutine ("playSound", level);
        hideSubMenus();
        SettingsController.Instance.isVersusAI = true;
        aISelection.SetActive(true);
    }

    public void toggleVibrations() {
        SettingsController.Instance.isVibrate = !SettingsController.Instance.isVibrate;
        disabled.SetActive(SettingsController.Instance.isVibrate);
    }

    public void showPlayerVersusPlayer() {
        //        source.Play();
        //        StartCoroutine ("playSound", level);
        hideSubMenus();
        SettingsController.Instance.isVersusAI = false;
        multiPlayer.SetActive(true);
    }

    public void ShowSpendCoins() {
        //        source.Play();
        //        StartCoroutine ("playSound", level);

        hideSubMenus();
        spendCoins.SetActive(true);
    }

    public void ShowCredits() {
        //        source.Play();
        //        StartCoroutine ("playSound", level);
    }

    public void quit() {
        Application.Quit();
    }

    public void LoadWatchAd() { //TODO make an Ad Manager
        //        source.Play();
        //        StartCoroutine ("playSound", level);
        if (Advertisement.IsReady()) {
            Advertisement.Show(null, new ShowOptions {
                resultCallback = result => {
                    Debug.Log(result.ToString());
                    if (result.ToString().Equals("Finished")) {
                        SettingsController.Instance.addCoins(100);
                        if (UI.Instance != null) {
                            UI.Instance.enoughCoins();
                        }
                    }
                }
            });
        }
    }

    public void next(int selection) {
        //        source.Play();
        //        StartCoroutine ("playSound", level);
    }

    public void prev(int selection) {
        //        source.Play();
        //        StartCoroutine ("playSound", level);
    }
//    IEnumerator playSound(int level) {
//        yield return new WaitForSeconds(0.5f);
//        Application.LoadLevel(level);
//    }
}