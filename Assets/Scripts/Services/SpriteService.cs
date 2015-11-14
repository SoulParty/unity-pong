using System;
using UnityEngine;
using UnityEngine.UI;

public class SpriteService : MonoBehaviour {

    SpriteDao spriteDao;
    UI UI;

    public Sprite get;
    public Sprite needCoins;

    public Sprite notOwned;
    public Sprite owned;
    public Sprite selected;

    public Sprite price25c;
    public Sprite price50c;
    public Sprite price100c;

    public Sprite[] symbolArray;

    [System.NonSerialized]
    public static SpriteService Instance;

    public SpriteService() {
        Instance = this;
    }

    public void Start() {
        spriteDao = SpriteDao.Instance;
        UI = UI.Instance;
    }

    public Sprite buy(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite currentSprite, Sprite[] sprites) {
        spriteDao.setStatus(currentSprite, (int) SpriteStatus.OWNED);
        return displaySprite(currentSpriteNumber, statusIcon, priceIcon, currentSprite, sprites);
    }

    public Sprite select(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite currentSprite, Sprite[] sprites) {
        spriteDao.setStatus(currentSprite, (int) SpriteStatus.SELECTED);
        return displaySprite(currentSpriteNumber, statusIcon, priceIcon, currentSprite, sprites);
    }

    public Sprite unSelect(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite currentSprite, Sprite[] sprites) {
        spriteDao.setStatus(currentSprite, (int) SpriteStatus.OWNED);
        return displaySprite(currentSpriteNumber, statusIcon, priceIcon, currentSprite, sprites);
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
        if (index > sprites.Length * 2/3) {
            return 100;
        } else if (index > sprites.Length * 1/3) {
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

    public Sprite next(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite currentSprite, Sprite[] sprites) {
        Sprite sprite;
        if (Array.IndexOf(sprites, currentSprite) != sprites.Length - 1) {
            sprite = sprites[Array.IndexOf(sprites, currentSprite) + 1];
        } else {
            sprite = sprites[0];
        }

        displaySprite(currentSpriteNumber, statusIcon, priceIcon, sprite, sprites);

        return sprite;
    }

    public Sprite prev(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite currentSprite, Sprite[] sprites) {
        Sprite sprite;
        if (Array.IndexOf(sprites, currentSprite) != 0) {
            sprite = sprites[Array.IndexOf(sprites, currentSprite) - 1];
        } else {
            sprite = sprites[sprites.Length - 1];
        }

        displaySprite(currentSpriteNumber, statusIcon, priceIcon, sprite, sprites);

        return sprite;
    }

    public Sprite displaySprite(GameObject currentSpriteNumber, Image statusIcon, Image priceIcon, Sprite sprite, Sprite[] sprites) {
        display2CharNumber(currentSpriteNumber, number(sprite, sprites));
        statusIcon.sprite = statusSprite(sprite);
        priceIcon.sprite = priceSprite(sprite, sprites);
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

    public Sprite statusSprite(Sprite sprite) {
        SpriteStatus status = spriteDao.getStatus(sprite);
        switch (status) {
            case SpriteStatus.SELECTED: return selected;
            case SpriteStatus.OWNED: return owned;
            default: return notOwned;
        }
    }
}
