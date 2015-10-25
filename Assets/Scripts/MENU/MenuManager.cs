using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public GameObject coinTotal;
    public GameObject highscoreTotal;
    public Sprite[] symbolArray;

    public void Start() {
        display4CharNumber(coinTotal, PlayerPrefs.GetInt(Const.COINS));
        display4CharNumber(highscoreTotal, PlayerPrefs.GetInt(Const.MAX_COMBO));
    }

    public Sprite toImage(char symbol) {
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

    private void display4CharNumber(GameObject display, int number) {
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
}
