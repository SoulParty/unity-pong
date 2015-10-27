using UnityEngine;

public class RandomParticleManager : MonoBehaviour {

    public GameObject particles1;

    public void Start() {
        InvokeRepeating("startParticles1", 1f, 1.99f);
    }

    private void startParticles1() {
        Vector3 screenPosition1 = Camera.main.ScreenToWorldPoint(
        new Vector3(Random.Range(0,Screen.width), Random.Range(0,Screen.height), Camera.main.farClipPlane/2));
        particles1.transform.position = screenPosition1;
    }
}
