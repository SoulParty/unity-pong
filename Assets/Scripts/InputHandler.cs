using Mono.Xml.Xsl;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;
    public Camera mainCamera;

    public void Update() {
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            foreach (Touch touch in Input.touches) {
                Vector3 point = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                if (point.x > 0) {
                    player1.GetComponent<PlayerController>().handleInput(point);
                }
                if (point.x < 0 && !SettingsController.Instance.isVersusAI) {
                    player2.GetComponent<PlayerController>().handleInput(point);
                }
            }
        #endif //End of mobile platform dependendent compilation section started above with #elif
    }

    public void OnMouseDrag() {
        player1.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver); //Mouse down
    }

    public void OnMouseDown() {
        player1.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver); //Mouse down
    }

    public void OnMouseUp() {
        player1.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver); //Mouse down
    }

    public void OnMouseEnter() {
        player1.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver); //Mouse down
    }
}
