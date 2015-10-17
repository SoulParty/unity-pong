using UnityEngine;

public class PauseManager : MonoBehaviour {

    public void returnToMainMenu() {
        Application.LoadLevel(0);
    }
}
