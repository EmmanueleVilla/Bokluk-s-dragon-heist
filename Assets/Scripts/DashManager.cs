using System.Collections;
using UnityEngine;

public class DashManager : MonoBehaviour {
    private const int MaxDashes = 1;
    private const float DashingPower = 8f;
    private const float DashingTime = 0.35f;

    public bool IsDashing { get; private set; }

    private int _timesDashed;
    private float _baseGravity;
    private Rigidbody2D _rigidbody2D;

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _baseGravity = _rigidbody2D.gravityScale;
    }

    public void SetPlayerGrounded() {
        _timesDashed = 0;
    }

    public Vector2 MaybeDash(Vector2 baseVelocity, bool direction) {
        if (IsDashing) {
            return baseVelocity;
        }

        if (_timesDashed >= MaxDashes) {
            return baseVelocity;
        }

        _timesDashed++;
        IsDashing = true;
        _rigidbody2D.gravityScale = 0;
        var velocity = new Vector2(
            DashingPower * (direction ? -1 : 1),
            0
        );

        StartCoroutine(DashCooldown());

        return velocity;
    }

    private IEnumerator DashCooldown() {
        yield return new WaitForSeconds(DashingTime);
        IsDashing = false;
        _rigidbody2D.gravityScale = _baseGravity;
    }
}