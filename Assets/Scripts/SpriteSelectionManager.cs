using Mono.Xml.Xsl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSelectionManager : MonoBehaviour {

    public SpriteType spriteType;

    public GameObject status;
    public GameObject price;
    public GameObject current;

    private Image statusIcon;
    private Image priceIcon;
    private Image currentSprite;

    public GameObject currentSpriteNumber;
    public GameObject totalNumberOfSprites;

    public GameObject buySpecialEffect;

    public RectTransform spriteRectTransform;

    private Sprite[] sprites;

    [System.NonSerialized]
    public static SpriteSelectionManager Instance;

    public SpriteSelectionManager() {
        Instance = this;
    }

    public void Start() {
        statusIcon = status.GetComponent<Image>();
        priceIcon = price.GetComponent<Image>();
        currentSprite = current.GetComponent<Image>();

        if (spriteType.Equals(SpriteType.PUCK)) {
            sprites = SettingsController.Instance.getPuckSprites();
        } else if (spriteType.Equals(SpriteType.RACKET1)) {
            sprites = SettingsController.Instance.getRacketSprites();
        } else {
              sprites = SettingsController.Instance.getRacketSprites();
        }
        SpriteService.Instance.total(totalNumberOfSprites, sprites);
        SpriteService.Instance.displaySprite(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites, spriteType);
    }

    public void prev() {
        currentSprite.sprite = SpriteService.Instance.prev(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites, spriteType);
    }

    public void next() {
        currentSprite.sprite = SpriteService.Instance.next(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites, spriteType);
    }

    public void buy() {
        currentSprite.sprite = SpriteService.Instance.buy(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites, spriteType);
        buySpecialEffect.transform.position = spriteRectTransform.TransformPoint(spriteRectTransform.rect.center);
        StartCoroutine(disableImpactTimer());
        refreshAll();
    }

    IEnumerator disableImpactTimer() {
        ObjectUtility.enableGameObject(buySpecialEffect);
        yield return new WaitForSeconds(3f);
        ObjectUtility.disableGameObject(buySpecialEffect);
    }

    public void refresh() {
        SpriteService.Instance.displaySprite(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites, spriteType);
    }

    public static void refreshAll() {
        foreach(SpriteSelectionManager selectionManager in FindObjectsOfType<SpriteSelectionManager>()) {
            selectionManager.refresh();
        }
    }
}
