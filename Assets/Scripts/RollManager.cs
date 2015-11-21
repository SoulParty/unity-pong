using Mono.Xml.Xsl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RollManager : MonoBehaviour {

    public Sprite[] rngIcons;
    public Sprite[] pucks;
    public GameObject rng;
    public GameObject rollFinishedParticles;

    public void rollTheDice() {
        if (TimeUtility.getIsFreeRollAvailable()) {
            TimeUtility.saveLastRollTime();
            UI.Instance.showWinMenu(); //Refresh menu
            winPrize();
        } else {
            if (SettingsController.Instance.checkFunds(50)) {
                MusicController.Instance.playCoin();
                SettingsController.Instance.setCoins(SettingsController.Instance.getCoins() - 50);
                winPrize();
            }
        }
        UI.Instance.rollRefresh();
    }

    private void winPrize() {
        ObjectUtility.enableGameObject(rollFinishedParticles);
        StartCoroutine(disableCoinImpactTimer());

        List<Sprite> prizes = new List<Sprite>();
        if (SettingsController.Instance.getPuckSprites().Length != 0) {
            prizes.AddRange(SettingsController.Instance.getPuckSprites());
            prizes.AddRange(SettingsController.Instance.getRacketSprites());
        }
        Sprite sprite = prizes[UnityEngine.Random.Range(0, prizes.Count - 1)];
        rng.GetComponent<Image>().sprite = sprite;
        SettingsController.Instance.removeFromNotOwnedSprites(sprite);
        SpriteDao.Instance.setStatus(sprite, (int) SpriteStatus.OWNED);
    }

    IEnumerator disableCoinImpactTimer() {
        yield return new WaitForSeconds(3f);
        ObjectUtility.disableGameObject(rollFinishedParticles);
    }

    public void LoadWatchAdFromRollScreen() {
        if (Advertisement.IsReady()) {
            Advertisement.Show(null, new ShowOptions {
                resultCallback = result => {
                    Debug.Log(result.ToString());
                    if (result.ToString().Equals("Finished")) {
                        SettingsController.Instance.addCoins(100);

                        UI.Instance.showWinMenu(); //Refresh menu

                        SpriteService.Instance.display4CharNumber(
                                UI.Instance.coinTotal, SettingsController.Instance.getCoins());
                        MusicController.Instance.playCoin();
                        SettingsController.Instance.playCoinsEarned(UI.Instance.coinsEarnedImpact);
                    }
                }
            });
        }
    }
}
