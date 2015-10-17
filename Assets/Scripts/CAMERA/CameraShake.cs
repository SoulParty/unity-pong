using UnityEngine;
using System.Collections;
//---------------------
public class CameraShake : MonoBehaviour {

    private Transform cameraTransform = null;
    //Total time for shaking in seconds
    public float shakeTime = 0.002f;
    //Speed of camera moving to shake points
    public float shakeSpeed = 0.006f;

    [System.NonSerialized]
    public static CameraShake Instance;

    public CameraShake() {
        Instance = this;
    }


    void Start () {
        //Get transform component
        cameraTransform = GetComponent<Transform>();
        //Start shaking
    }

    public void cameraShake(CameraShakeType shakeType) {
        StartCoroutine(Shake(shakeType));
    }

    //Shake camera
    private IEnumerator Shake(CameraShakeType shakeType) {
        //Store original camera position
        Vector3 originalPosition = new Vector3(0, 0, 0);
        //Count elapsed time (in seconds)
        float ElapsedTime = 0.0f;
        //Repeat for total shake time
        while (ElapsedTime < shakeTime * (int) shakeType) {
            //Pick random point on unit sphere
            Vector2 randomPoint2D = Random.insideUnitCircle * (int) shakeType;
            Vector3 randomPoint = originalPosition + new Vector3(randomPoint2D.x, randomPoint2D.y, 0);
            //Update Position
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, randomPoint, Time.deltaTime * shakeSpeed * (int) shakeType);
            //Break for next frame
            yield return null;
            //Update time
            ElapsedTime += Time.deltaTime;
        }
        //Restore camera position
        cameraTransform.localPosition = originalPosition;
    }
}
