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

    private Sprite[] sprites;
    private SettingsController settingsController;

    public void Start() {
        settingsController = SettingsController.Instance;

        statusIcon = status.GetComponent<Image>();
        priceIcon = price.GetComponent<Image>();
        currentSprite = current.GetComponent<Image>();

        if (spriteType.Equals(SpriteType.PUCK)) {
            sprites = settingsController.getPuckSprites();
        } else if (spriteType.Equals(SpriteType.RACKET1)) {
            sprites = settingsController.getRacketSprites();
        } else {
              sprites = settingsController.getRacketSprites();
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
    }
}
