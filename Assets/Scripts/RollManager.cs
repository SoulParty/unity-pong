using Mono.Xml.Xsl;
using System;
using System.Collections;
using UnityEngine;
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
            if (SettingsController.Instance.checkFunds(100)) {
                SettingsController.Instance.setCoins(SettingsController.Instance.getCoins() - 100);
                winPrize();
            }
        }
    }

    private void winPrize() {
        ObjectUtility.enableGameObject(rollFinishedParticles);
        StartCoroutine(disableCoinImpactTimer());

        Sprite[] puckSprites = pucks;
        if (SettingsController.Instance.getPuckSprites().Length != 0) {
            puckSprites = SettingsController.Instance.getPuckSprites();
        }

        Sprite sprite = puckSprites[UnityEngine.Random.Range(0, puckSprites.Length - 1)];
        rng.GetComponent<Image>().sprite = sprite;
        SpriteDao.Instance.setStatus(sprite, (int) SpriteStatus.OWNED);
    }

    IEnumerator disableCoinImpactTimer() {
        yield return new WaitForSeconds(3f);
        ObjectUtility.disableGameObject(rollFinishedParticles);
    }
}
