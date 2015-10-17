using Mono.Xml.Xsl;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//TODO implement free roll timer
public class RollManager : MonoBehaviour {

    public Sprite[] rngIcons;
    public GameObject rng;
    public GameObject glowParticles;
    public GameObject rollFinishedParticles;

    public void Awake() {
        glowParticles.GetComponent<ParticleSystem>().Play();
    }

    public void rollTheDice() {
        ObjectUtility.enableGameObject(rollFinishedParticles);

        Sprite[] puckSprites = SettingsController.Instance.getPuckSprites();
        Sprite sprite = puckSprites[Random.Range(0, puckSprites.Length)];

        rng.GetComponent<Image>().sprite = sprite;
        SettingsController.Instance.setStatus(sprite.ToString(), 1);
    }
}
