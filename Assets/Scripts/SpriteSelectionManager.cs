using Mono.Xml.Xsl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSelectionManager : MonoBehaviour {

    public SpriteType spriteType;

    public int LOGO_COST = 1;
    public GameObject owned;
    public GameObject selectionSprite;
    public Sprite[] sprites;
    public Sprite[] selections;

    private int _currentSprite = 0;
    public int currentSprite {
        get {
            return this._currentSprite;
        }
        set {
            if (value >= sprites.Length - 1) {
                this._currentSprite = 0;
            } else if (value < 0) {
                this._currentSprite = sprites.Length - 1;
            } else {
                this._currentSprite = value;
            }
        }
    }

    public void Start () {
        SettingsController.Instance.loadSprites(spriteType);
        if (spriteType == SpriteType.PUCK) {
            sprites = SettingsController.Instance.getPuckSprites();
            showSprite(System.Array.IndexOf (sprites, SettingsController.Instance.selectedPuck));
        } else if (spriteType == SpriteType.RACKET1) {
            sprites = SettingsController.Instance.getRacketSprites();
            showSprite(System.Array.IndexOf (sprites, SettingsController.Instance.selectedPlayer1Racket));
        } else {
            sprites = SettingsController.Instance.getRacketSprites();
            showSprite(System.Array.IndexOf (sprites, SettingsController.Instance.selectedPlayer2Racket));
        }
    }

    public void buySprite() {
        if (SettingsController.Instance.spriteStatus(spriteType, sprites[currentSprite].ToString()) == 0 && SettingsController.Instance.checkFunds(LOGO_COST)) {
            SettingsController.Instance.setCoins(SettingsController.Instance.getCoins() - LOGO_COST);
            SettingsController.Instance.setStatus(spriteType, sprites[currentSprite].ToString(), 1);
            showSprite(currentSprite);
        }
    }

    public void selectSprite(int sprite) {
        string exSprite = null;
        switch (sprite) {
            case 1:
            exSprite = SettingsController.Instance.selectedPuck.ToString();
            SettingsController.Instance.setPuck(sprites[currentSprite]);
            SettingsController.Instance.spriteKeyByStatus(spriteType, 2); break;
            case 2:
            exSprite = SettingsController.Instance.selectedPlayer1Racket.ToString();
            SettingsController.Instance.setPlayer1Racket(sprites[currentSprite]);
            SettingsController.Instance.spriteKeyByStatus(spriteType, 3); break;
            case 3:
            exSprite = SettingsController.Instance.selectedPlayer2Racket.ToString();
            SettingsController.Instance.setPlayer2Racket(sprites[currentSprite]);
            SettingsController.Instance.spriteKeyByStatus(spriteType, 4); break;
        }
        if (exSprite != null) {
            SettingsController.Instance.setStatus(spriteType, exSprite, 1);
        }

        switch (sprite) {
            case 1: SettingsController.Instance.setPuck(sprites[currentSprite]);
            SettingsController.Instance.setStatus(spriteType, sprites[currentSprite].ToString(), 2);
            break;
            case 2: SettingsController.Instance.setPlayer1Racket(sprites[currentSprite]);
            SettingsController.Instance.setStatus(spriteType, sprites[currentSprite].ToString(), 3);
            break;
            case 3: SettingsController.Instance.setPlayer2Racket(sprites[currentSprite]);
            SettingsController.Instance.setStatus(spriteType, sprites[currentSprite].ToString(), 4);
            break;
        }
        showSprite(currentSprite);
    }

    public void nextSprite() {
        currentSprite++;
        showSprite(currentSprite);
    }

    public void prevSprite() {
        currentSprite--;
        showSprite(currentSprite);
    }

    public void showSprite(int index) {
        if (index != -1) {
            selectionSprite.GetComponent<Image>().sprite = sprites[index];
            int spriteStatus = SettingsController.Instance.spriteStatus(spriteType, sprites[index].ToString());
            switch (spriteStatus) {
                case 2: if (spriteType != SpriteType.PUCK) {
                    spriteStatus = 1;
                } break;
                case 3: if (spriteType != SpriteType.RACKET1) {
                    spriteStatus = 1;
                } break;
                case 4: if (spriteType != SpriteType.RACKET2) {
                    spriteStatus = 1;
                } break;
            }
            owned.GetComponent<Image>().sprite = selections[spriteStatus];
        }
    }
}
