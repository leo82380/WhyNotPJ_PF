using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _groundCheckRadius;
    [SerializeField]
    private float _jumpForce;
    private float _hor;
    private float _ver;

    private Animator _anim;
    private Rigidbody2D _rb2D;


    private Transform _groundCheckTrans;

    [SerializeField]
    private LayerMask _groundCheckLayerMask;

    private void Awake()
    {
        _groundCheckTrans = transform.Find("GroundCheck").transform;
        _anim = GetComponentInChildren<Animator>();
        _rb2D=GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _speed = _moveSpeed;
    }
    private void Update()
    {
        Move();
        Anim();
    }
    private void FixedUpdate()
    {
    }
    private void Anim()
    {

        _anim.SetBool("Move", _hor != 0);
    }
    private void Move()
    {
        _hor = Input.GetAxisRaw("Horizontal");

        Vector2 input = new Vector2(_hor * _speed, 0);
        if (_hor > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        if (_hor < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        _rb2D.AddForce(input, ForceMode2D.Impulse);

        if (transform.localScale.x > 0 && _rb2D.velocity.x > _speed)
            _rb2D.velocity = new Vector2(_speed, _rb2D.velocity.y);
        if (transform.localScale.x < 0 && _rb2D.velocity.x < -_speed)
            _rb2D.velocity = new Vector2(-_speed, _rb2D.velocity.y);
        if (Input.GetButtonUp("Horizontal"))
        {
            _rb2D.velocity = Vector2.zero;
        }

        if(Input.GetButtonDown("Jump") && IsGround())
        {
            _rb2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }
    private bool IsGround()
    {
      return  Physics2D.OverlapCircle(_groundCheckTrans.position, _groundCheckRadius, _groundCheckLayerMask);
    }
}
