using Mono.Xml.Xsl;
using System;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class SpriteService : MonoBehaviour {

    SpriteDao spriteDao;

    public Sprite get;
    public Sprite needCoins;

    public Sprite notOwned;
    public Sprite owned;
    public Sprite selected;

    public Sprite price25c;
    public Sprite price50c;
    public Sprite price100c;
    public Sprite nullIcon;

    public Sprite[] symbolArray;

    public GameObject coinsTotal;
    public GameObject coinsEarnedImpact;

    [System.NonSerialized]
    public static SpriteService Instance;

    public SpriteService() {
        Instance = this;
    }

    public void Start() {
        spriteDao = SpriteDao.Instance;
    }

    public Sprite buy(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite currentSprite, Sprite[] sprites, SpriteType spriteType) {
        int price = SpriteService.Instance.price(currentSprite, sprites);
        SpriteStatus status = getStatus(currentSprite, spriteType);
        if (status.Equals(SpriteStatus.NOT_OWNED) && SettingsController.Instance.checkFunds(price)) {
            SettingsController.Instance.setCoins(SettingsController.Instance.getCoins() - price);
            display4CharNumber(coinsTotal, SettingsController.Instance.getCoins());
            spriteDao.setStatus(currentSprite, (int) SpriteStatus.OWNED);
            SettingsController.Instance.removeFromNotOwnedSprites(currentSprite);

            MusicController.Instance.playCoin();
        } else if (status.Equals(SpriteStatus.OWNED)) {
            select(currentSprite, spriteType);
        }
        return displaySprite(currentSpriteNumber, statusIcon, priceIcon, currentSprite, sprites, spriteType);
    }

    public void select(Sprite currentSprite, SpriteType spriteType) {
        if (!spriteDao.getStatus(currentSprite).Equals(SpriteStatus.NOT_OWNED)) {
            spriteDao.unSelect(spriteDao.getSelected(spriteType));
            spriteDao.setStatus(currentSprite, (int) SpriteStatus.SELECTED);
            spriteDao.setSelected(currentSprite, spriteType);
            SettingsController.Instance.setSelected(currentSprite, spriteType);
        }
    }

    private int number(Sprite currentSprite, Sprite[] sprites) {
        int index = Array.IndexOf(sprites, currentSprite);
        return index + 1;
    }

    public int total(GameObject totalNumberOfSprites, Sprite[] sprites) {
        int spritesLength = sprites.Length;
        display2CharNumber(totalNumberOfSprites, spritesLength);
        return spritesLength;
    }

    private int price(Sprite currentSprite, Sprite[] sprites) {
        int index = Array.IndexOf(sprites, currentSprite);
        if (index > sprites.Length * 2 / 3) {
            return 100;
        } else if (index > sprites.Length * 1 / 3) {
            return 50;
        } else {
            return 25;
        }
    }

    public Sprite priceSprite(Sprite currentSprite, Sprite[] sprites) {
        int cost = price(currentSprite, sprites);
        switch (cost) {
            case 100: return price100c;
            case 50: return price50c;
            default: return price25c;
        }
    }

    public Sprite next(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite currentSprite, Sprite[] sprites, SpriteType spriteType) {
        Sprite sprite;
        if (Array.IndexOf(sprites, currentSprite) != sprites.Length - 1) {
            sprite = sprites[Array.IndexOf(sprites, currentSprite) + 1];
        } else {
            sprite = sprites[0];
        }

        displaySprite(currentSpriteNumber, statusIcon, priceIcon, sprite, sprites, spriteType);

        return sprite;
    }

    public Sprite prev(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite currentSprite, Sprite[] sprites, SpriteType spriteType) {
        Sprite sprite;
        if (Array.IndexOf(sprites, currentSprite) != 0) {
            sprite = sprites[Array.IndexOf(sprites, currentSprite) - 1];
        } else {
            sprite = sprites[sprites.Length - 1];
        }

        displaySprite(currentSpriteNumber, statusIcon, priceIcon, sprite, sprites, spriteType);

        return sprite;
    }

    public Sprite displaySprite(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite sprite, Sprite[] sprites, SpriteType spriteType) {
        display2CharNumber(currentSpriteNumber, number(sprite, sprites));
        statusIcon.sprite = statusSprite(sprite, spriteType);
        if (statusIcon.sprite.Equals(notOwned)) {
            priceIcon.sprite = priceSprite(sprite, sprites);
            if (!SettingsController.Instance.checkFunds(price(sprite, sprites))) {
                statusIcon.sprite = needCoins;
                //TODO play animation
            }
        } else {
            priceIcon.sprite = nullIcon;
        }
        //change neighbors
        return sprite;
    }

    private void display2CharNumber(GameObject display, int number) {
        Image[] componentImages = display.GetComponentsInChildren<Image>();
        char[] charArray = number.ToString().ToCharArray();
        if (charArray.Length == 2) {
            componentImages[0].sprite = toImage(charArray[0]);
            componentImages[1].sprite = toImage(charArray[1]);
        } else {
            componentImages[0].sprite = toImage('0');
            componentImages[1].sprite = toImage(charArray[0]);
        }
    }

    public void display4CharNumber(GameObject display, int number) {
        Image[] componentImages = display.GetComponentsInChildren<Image>();
        char[] charArray = number.ToString().ToCharArray();
        for (int i = 0; i < 4; i++) {
            if (i < charArray.Length) {
                componentImages[i].sprite = toImage(charArray[i]);
            } else {
                componentImages[i].sprite = toImage(' ');
            }
        }
    }

    private Sprite toImage(char symbol) {
        switch (symbol) {
            case '0': return symbolArray[0]; break;
            case '1': return symbolArray[1]; break;
            case '2': return symbolArray[2]; break;
            case '3': return symbolArray[3]; break;
            case '4': return symbolArray[4]; break;
            case '5': return symbolArray[5]; break;
            case '6': return symbolArray[6]; break;
            case '7': return symbolArray[7]; break;
            case '8': return symbolArray[8]; break;
            case '9': return symbolArray[9]; break;
            case 'c': return symbolArray[10]; break;
            case ' ': return symbolArray[11]; break;
            default: return symbolArray[0];
        }
    }

    public Sprite statusSprite(Sprite sprite, SpriteType spriteType) {
        SpriteStatus status = getStatus(sprite, spriteType);
        switch (status) {
            case SpriteStatus.SELECTED: return selected;
            case SpriteStatus.OWNED: return owned;
            default: return notOwned;
        }
    }

    public SpriteStatus getStatus(Sprite sprite, SpriteType spriteType) {
        SpriteStatus status = spriteDao.getStatus(sprite);
        string selected = spriteDao.getSelected(spriteType);
        if (status.Equals(SpriteStatus.SELECTED) && sprite.ToString().Equals(selected)) {
            return SpriteStatus.SELECTED;
        } else if (status.Equals(SpriteStatus.SELECTED) && selected.Equals("")) {
            spriteDao.setSelected(sprite, spriteType);
            return SpriteStatus.SELECTED;
        } else if (status.Equals(SpriteStatus.OWNED) || status.Equals(SpriteStatus.SELECTED)) {
            return SpriteStatus.OWNED;
        } else {
            return SpriteStatus.NOT_OWNED;
        }
    }
}
