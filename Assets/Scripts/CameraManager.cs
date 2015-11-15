using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public float orthographicSize = 540;
    public float aspect = 1.777777778f;

    Camera camera;

    void Start() {
        camera = GetComponent<Camera>();
    }

    void Update() {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
        -orthographicSize * aspect, orthographicSize * aspect,
        -orthographicSize, orthographicSize,
        camera.nearClipPlane, camera.farClipPlane);
    }
}
