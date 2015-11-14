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
//            currentSprite = settingsController.getSpriteFromCache(SpriteType.PUCK, SpriteDao.Instance.getSelected(SpriteType.PUCK));
            sprites = settingsController.getPuckSprites();
        } else if (spriteType.Equals(SpriteType.RACKET1)) {
//            currentSprite = settingsController.getSpriteFromCache(SpriteType.RACKET1, SpriteDao.Instance.getSelected(SpriteType.RACKET1));
            sprites = settingsController.getRacketSprites();
        } else {
//            currentSprite = settingsController.getSpriteFromCache(SpriteType.RACKET2, SpriteDao.Instance.getSelected(SpriteType.RACKET2));
            sprites = settingsController.getRacketSprites();
        }
        SpriteService.Instance.total(totalNumberOfSprites, sprites);
        SpriteService.Instance.displaySprite(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites);
    }

    public void prev() {
        currentSprite.sprite = SpriteService.Instance.prev(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites);
    }

    public void next() {
        currentSprite.sprite = SpriteService.Instance.next(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites);
    }

    public void buy() {
        currentSprite.sprite = SpriteService.Instance.buy(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites);
    }

    public void select() {
        currentSprite.sprite = SpriteService.Instance.select(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites);
    }

    public void unSelect() {
        currentSprite.sprite = SpriteService.Instance.unSelect(currentSpriteNumber, statusIcon, priceIcon, currentSprite.sprite, sprites);
    }
}
