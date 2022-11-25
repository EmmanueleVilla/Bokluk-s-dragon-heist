using UnityEngine;

public class ReferenceGizmos : MonoBehaviour {
    private void OnDrawGizmos() {
        return;
        const int deltaX = 26;
        const int deltaY = 15;
        for (var i = -10; i < 30; i++) {
            for (var j = -10; j < 30; j++) {
                Gizmos.DrawWireCube(
                    transform.position + new Vector3(i * deltaX, -j * deltaY, 0),
                    new Vector3(26, 15, 0));
            }
        }
    }
}