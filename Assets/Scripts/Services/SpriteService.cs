using System;
using UnityEngine;

public class SpriteService {

    SpriteDao spriteDao;

    [System.NonSerialized]
    public static SpriteService Instance;

    public SpriteService() {
        Instance = this;
    }

    public void Start() {
        spriteDao = SpriteDao.Instance;
    }

    public Sprite buy(Sprite currentSprite) {
        spriteDao.setStatus(currentSprite, (int) SpriteStatus.OWNED);
        //refresh ui
    }

    public Sprite select(Sprite currentSprite) {
        spriteDao.setStatus(currentSprite, (int) SpriteStatus.SELECTED);
        //refresh ui
    }

    public Sprite unSelect(Sprite currentSprite) {
        spriteDao.setStatus(currentSprite, (int) SpriteStatus.OWNED);
        //refresh ui
    }

    public int number(Sprite currentSprite, Sprite[] sprites) {
        return Array.IndexOf(sprites, currentSprite);
    }

    public int price(Sprite currentSprite, Sprite[] sprites) {
        int index = Array.IndexOf(sprites, currentSprite);
        if (index > sprites.Length * 2/3) {
            return 100;
        } else if (index > sprites.Length * 1/3) {
            return 50;
        } else {
            return 25;
        }
        //refresh ui
    }

    public void priceSprite(Sprite currentSprite, Sprite[] sprites) {
        int cost = price(currentSprite, sprites);
        Sprite costSprite;
        switch (cost) {
            case 100: costSprite = currentSprite; break;
            case 50: costSprite = currentSprite; break;
            case 25: costSprite = currentSprite; break;
        }
        //refresh ui
    }

    public Sprite next(Sprite currentSprite, Sprite[] sprites) {
        Sprite sprite;
        if (Array.IndexOf(sprites, currentSprite) != sprites.Length - 1) {
            sprite = sprites[Array.IndexOf(sprites, currentSprite) + 1];
        } else {
            sprite = sprites[0];
        }
        //refresh ui
        return sprite;
    }

    public Sprite prev(Sprite currentSprite, Sprite[] sprites) {
        Sprite sprite;
        if (Array.IndexOf(sprites, currentSprite) != 0) {
            sprite = sprites[Array.IndexOf(sprites, currentSprite) - 1];
        } else {
            sprite = sprites[sprites.Length - 1];
        }
        //refresh ui
        return sprite;
    }
}
