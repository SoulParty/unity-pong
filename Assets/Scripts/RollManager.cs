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
        TimeSpan passedSinceLastRoll = TimeUtility.getTimePassedSinceLastRoll();
        if (passedSinceLastRoll.Hours >= 4) {
            TimeUtility.saveLastRollTime();
        } else {
            SettingsController.Instance.setCoins(SettingsController.Instance.getCoins() - 100);
        }

        ObjectUtility.enableGameObject(rollFinishedParticles);

        Sprite[] puckSprites = pucks;
        if (SettingsController.Instance.getPuckSprites().Length != 0) {
            puckSprites = SettingsController.Instance.getPuckSprites();
        }

        Sprite sprite = puckSprites[UnityEngine.Random.Range(0, puckSprites.Length - 1)];
        rng.GetComponent<Image>().sprite = sprite;
        SettingsController.Instance.setStatus(sprite.ToString(), 1);
    }
}
