using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoklukController : MonoBehaviour {
    [SerializeField] private Transform[] groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private DashManager _dashManager;


    private float _horizontal;
    private bool _isFacingRight = true;
    private bool _maybeDash;
    private bool _maybeFire;
    private bool _maybeJump;
    private bool _canJump = true;
    private float _jumpedTimes = 0;
    private float _maxJumps = 2;
    private static readonly int RunKey = Animator.StringToHash("Run");
    private static readonly int JumpKey = Animator.StringToHash("Jump");
    private static readonly int FallKey = Animator.StringToHash("Fall");
    private static readonly int DashKey = Animator.StringToHash("Dash");


    private bool IsGrounded() {
        return groundCheck.Any(x => Physics2D.OverlapCircle(x.position, 0.1f, groundLayer));
    }

    public void Move(InputAction.CallbackContext context) {
        _horizontal = context.ReadValue<Vector2>().x;
    }

    public void Dash(InputAction.CallbackContext context) {
        _maybeDash = context.performed;
    }

    public void Fire(InputAction.CallbackContext context) {
        _maybeFire = context.performed;
    }

    public void Jump(InputAction.CallbackContext context) {
        _maybeJump = context.performed;
    }

    private void Awake() {
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _dashManager = GetComponent<DashManager>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        var velocity = _rigidbody2D.velocity;

        var isGrounded = velocity.y < 0.5 && IsGrounded();
        if (isGrounded) {
            _jumpedTimes = 0;
            _dashManager.SetPlayerGrounded();
        }

        if (_horizontal != 0f) {
            _renderer.flipX = _horizontal < 0.0f;
        }

        if (_maybeDash) {
            velocity = _dashManager.MaybeDash(velocity, _renderer.flipX);
        }

        if (!_dashManager.IsDashing) {
            velocity.x = _horizontal * speed;

            if (_maybeFire) {
                //TODO: shoot
            }

            if (_maybeJump && _canJump && _jumpedTimes < _maxJumps) {
                velocity.y = jumpPower;
                _jumpedTimes++;
            }
        }

        if (velocity.y is < 0 and > -30f) {
            velocity.y -= 12 * Time.deltaTime;
        }

        _rigidbody2D.velocity = velocity;

        _animator.SetBool(RunKey, math.abs(velocity.x) > 0.1f);
        _animator.SetBool(JumpKey, !_dashManager.IsDashing && velocity.y > 0.1f && !isGrounded);
        _animator.SetBool(FallKey, !_dashManager.IsDashing && velocity.y < -0.2f && !isGrounded);
        _animator.SetBool(DashKey, _dashManager.IsDashing);

        transform.localEulerAngles = Vector3.zero;

        _maybeDash = false;
        _maybeFire = false;
        _maybeJump = false;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        switch (col.gameObject.tag) {
            case "Trampoline":
                var trampoline = col.GetComponent<HasTrampolineManager>();
                if (trampoline != null) {
                    _canJump = false;
                    StartCoroutine(ResetCanJump());
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,
                        trampoline.trampolineManager.trampolinePower);
                    trampoline.trampolineManager.Trigger();
                }

                break;
            case "Coin":
                Destroy(col.gameObject);
                break;
        }
    }

    private IEnumerator ResetCanJump() {
        yield return new WaitForSeconds(0.5f);
        _canJump = true;
    }
}