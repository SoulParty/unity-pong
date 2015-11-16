using Scripts.Ads;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    public void returnToMainMenu() {
        AdManager.Instance.hideBannerAd();
        Application.LoadLevel(0);
    }

    public void quit() {
        Application.Quit();
    }
}
