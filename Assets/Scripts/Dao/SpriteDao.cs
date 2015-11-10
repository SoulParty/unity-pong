using UnityEngine;

public class SpriteDao {

    [System.NonSerialized]
    public static SpriteDao Instance;


    public SpriteDao() {
        Instance = this;
    }

    public void setStatus(Sprite sprite, int spriteStatus) {

    }
}
