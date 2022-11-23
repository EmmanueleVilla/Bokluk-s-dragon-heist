using UnityEngine;

public class RoomManager : MonoBehaviour {
    public GameObject virtualCamera;

    public static bool SwapCameraEnabled = true;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!SwapCameraEnabled) {
            return;
        }

        if (other.CompareTag("Player")) {
            virtualCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!SwapCameraEnabled) {
            return;
        }

        if (other.CompareTag("Player")) {
            virtualCamera.SetActive(false);
        }
    }
}