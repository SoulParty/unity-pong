using Mono.Xml.Xsl;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public bool canMove = false;
    public int direction = 0;
    public Camera mainCamera;

    public float impactLength = 2f;
    public GameObject hitImpactParticles;

    public void handleInput(Vector3 inputPosition) {
        if (inputPosition != Vector3.zero) {
            Vector3 position = this.gameObject.transform.position;
            #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            canMove = true;
            #endif
            if (canMove) {
                float positionX = Math.Sign(position.x) * GameController.Instance.playerPosition;
                switch (direction) {
                    case 0:
                    this.gameObject.transform.position = new Vector3(positionX, inputPosition.y, position.z);
                    break;
                    case 1:
                    if (inputPosition.y < position.y) {
                        this.gameObject.transform.position = new Vector3(positionX, inputPosition.y, position.z);
                    }
                    break;
                    case -1:
                    if (inputPosition.y > position.y) {
                        this.gameObject.transform.position = new Vector3(positionX, inputPosition.y, position.z);
                    }
                    break;
                }
            }
        }
    }

    public void OnMouseOver() {
        #if UNITY_STANDALONE
            Vector3 point = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            handleInput(point);
        #endif
    }

    public void OnMouseDown() {
        canMove = true;
    }

    public void OnMouseUp() {
        canMove = false;
    }

    public void OnMouseExit() {
        canMove = false;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name.Equals("Ball")) {
            Vibration.Vibrate(90);
            hitImpactParticles.transform.position = new Vector3(transform.position.x, collision.gameObject.transform.position.y, 0);
            ObjectUtility.enableGameObject(hitImpactParticles);
            StartCoroutine(disableImpactTimer());
        }
    }

    IEnumerator disableImpactTimer() {
        yield return new WaitForSeconds(impactLength);
        ObjectUtility.disableGameObject(hitImpactParticles);
    }
}
