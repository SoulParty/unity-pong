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
        } else {
            if (SettingsController.Instance.checkFunds(SettingsController.Instance.getCoins())) {
                SettingsController.Instance.setCoins(SettingsController.Instance.getCoins() - 100);
            }
        }

        ObjectUtility.enableGameObject(rollFinishedParticles);
        StartCoroutine(disableCoinImpactTimer());

        Sprite[] puckSprites = pucks;
        if (SettingsController.Instance.getPuckSprites().Length != 0) {
            puckSprites = SettingsController.Instance.getPuckSprites();
        }

        Sprite sprite = puckSprites[UnityEngine.Random.Range(0, puckSprites.Length - 1)];
        rng.GetComponent<Image>().sprite = sprite;
        SettingsController.Instance.setStatus(sprite.ToString(), 1);
    }

    IEnumerator disableCoinImpactTimer() {
        yield return new WaitForSeconds(3f);
        ObjectUtility.disableGameObject(rollFinishedParticles);
    }
}
