using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoomManager : MonoBehaviour {
    public GameObject virtualCamera;

    public static bool SwapCameraEnabled = true;

    public bool activateLight = false;

    private Light2D globalLight;

    private void Awake() {
        globalLight = GameObject.FindGameObjectWithTag("Global Light").GetComponent<Light2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!SwapCameraEnabled) {
            return;
        }

        if (other.CompareTag("Player")) {
            virtualCamera.SetActive(true);
            if (activateLight) {
                globalLight.intensity = 0.5f;
                other.GetComponentInChildren<Light2D>().intensity = 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!SwapCameraEnabled) {
            return;
        }

        if (other.CompareTag("Player")) {
            virtualCamera.SetActive(false);
            if (activateLight) {
                globalLight.intensity = 1;
                other.GetComponentInChildren<Light2D>().intensity = 0.5f;
            }
        }
    }
}