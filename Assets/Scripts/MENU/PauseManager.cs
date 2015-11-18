using Scripts.Ads;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    public void returnToMainMenu() {
//        AdManager.Instance.hideBannerAd();
        GameController.Instance.pauseGame();
        Application.LoadLevel(0);
    }

    public void quit() {
        Application.Quit();
    }
}
