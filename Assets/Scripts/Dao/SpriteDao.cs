using UnityEngine;

public class SpriteDao : MonoBehaviour {

    [System.NonSerialized]
    public static SpriteDao Instance;

    public SpriteDao() {
        Instance = this;
    }

    public void setStatus(Sprite sprite, int spriteStatus) {
        PlayerPrefs.SetInt(sprite.ToString(), spriteStatus);
    }

    public void unSelect(string sprite) {
        PlayerPrefs.SetInt(sprite, (int) SpriteStatus.OWNED);
    }

    public SpriteStatus getStatus(Sprite sprite) {
        int status = PlayerPrefs.GetInt(sprite.ToString());
        return (SpriteStatus) status;
    }

    public void setSelected(Sprite sprite, SpriteType spriteType) {
        PlayerPrefs.SetString(spriteType.ToString(), sprite.ToString());
    }

    public string getSelected(SpriteType spriteType) {
        return PlayerPrefs.GetString(spriteType.ToString());
    }
}
