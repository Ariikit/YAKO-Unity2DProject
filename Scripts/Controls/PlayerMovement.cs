using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 10f;
    public float jumpingPower = 20f;
    private bool isFacingRight = true;

    private TrailRenderer _trailRenderer;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private bool _active = true;

    //Header
    [SerializeField] public float _dashingVelocity = 14f;
    [SerializeField] public float _dashingTime = 0.5f;
    private Vector2 _dashingDir;
    private bool _isDashing;
    private bool _canDash = true;
    public bool _isJumping = false;

    //START IS OVER HERE
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }
    void Update()
    {
        if (!_active)
            return;
        //Dash!!
        var dashInput = Input.GetButtonDown("Dash");

        //Dashing Logic
        if (dashInput && _canDash)
        {
            _isDashing = true;
            _canDash = false;
            _trailRenderer.emitting = true;
            _dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (_dashingDir == Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
        }
        //Stop dashing
        

        if (_isDashing)
        {
            _rigidbody.velocity = _dashingDir.normalized * _dashingVelocity;
            return;
        }

        if (IsGrounded())
        {
            _canDash = true;
            _isJumping = false;
        }

        if (!IsGrounded())
        {
            _isJumping = false;
        }


        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            
        }

        Flip();
    }

    //FIXED UPDATE IS OVER HERE!!!
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    // REMEMBER:
    // You specify what to wait for after 'yeild return'
    //After that, you specify what should happen after the event
    //The method has to return 'IEnumerator' NOT 'IEnumerable'
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime);
        _trailRenderer.emitting = false;
        _isDashing = false;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
