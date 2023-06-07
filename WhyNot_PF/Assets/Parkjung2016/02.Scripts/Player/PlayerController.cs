using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Other
    private float _hor;
    [HideInInspector]
    public bool _isMoveTrue;
    #endregion
    #region Speed
    [Header("속도")]
    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _airWalkSpeed;
    [SerializeField]
    private float _airRunSpeed;
    [SerializeField]
    private float _runSpeed;
    [SerializeField]
    private float _speed;
    private float _maxSpeed;
    #endregion
    #region Jump
    [Header("점프")]
    [SerializeField]
    private LayerMask _groundCheckLayerMask;
    private Transform _groundCheckTrans;
    [SerializeField]
    private float _groundCheckRadius;
    [SerializeField]
    private float _jumpForce;
    #endregion
    #region Component
    private Animator _anim;
    private Rigidbody2D _rb2D;
    #endregion
    #region Attack
    private bool _attackBuffer;
    private bool _attackBufferCheck;
    [Header("공격")]
    [HideInInspector]
    public int _attackNum;
    [HideInInspector]
    public bool _attacking;
    private bool _comboPossible;
    [HideInInspector]
    public bool _airAttacking;
    private bool _airAttackReady;
    [HideInInspector]
    public int AttackPower;

    private GameObject _attackCol;

    [SerializeField]
    private float _airAttackForce;
    #endregion

    private void Awake()
    {
        _groundCheckTrans = transform.Find("GroundCheck").transform;
        _attackCol = transform.Find("AttackCol").gameObject;
        _anim = GetComponent<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
        _isMoveTrue = true;

    }
    private void Start()
    {
        _speed = _walkSpeed;
        _maxSpeed = _walkSpeed;
        _attackBufferCheck = false;
        _attackBuffer = false;
        DisableAttackCol();
    }
    private void Update()
    {
        Move();
        Anim();
        AttackCheck();
    }
    private void Attack()
    {
        if (!_attacking)
        {
            _attacking = true;
            _isMoveTrue = false;
            _rb2D.velocity = Vector2.zero;
            //if (Input.GetButton("Crouch"))
            //{

            //    //Player.instance._playerAnim.CrouchAttackAnim();
            //}
            if (IsGround())
            {

                AttackAnim(_attackNum);
            }
            else if (!_airAttacking )
            {

                    _airAttacking = true;
                    AirAttackAnim();

            }

        }
        else if (_comboPossible)
        {

            //if (Input.GetButton("Crouch"))
            //{

            //    Player.instance._playerAnim.CrouchAttackAnim();
            //}
            //else if (Player.instance._isGrounded)
            AttackAnim(_attackNum);
            //else
            //{
            //    _airAttacking = true;
            //    Player.instance._playerAnim.AirAttackAnim(_attackNum);
            //    Player.instance._rdb2D.velocity = Vector2.zero;
            //}
            _comboPossible = false;
        }
    }
    private void AttackCheck()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_attackBufferCheck)
            {
                _attackBuffer = true;

            }
            else
            {
                Attack();
            }

        }
    }
    public void AirAttackReady()
    {
        transform.DOMoveY(transform.position.y+.5f,.4f).SetEase(Ease.OutSine);

          _airAttackReady = true;
    }
    public void EnableAttackCol()
    {
        _attackCol.SetActive(true);
    }
    public void DisableAttackCol()
    {
        _attackCol.SetActive(false);
    }
    private void AttackAnim(int attackNum)
    {
        _anim.Play("Attack" + attackNum);
    }
    private void AirAttackAnim()
    {
        _anim.Play("AirAttacking");
    }
    public void AirAttack()
    {
        _airAttackReady = false;
        _rb2D.AddForce(Vector2.down * _airAttackForce, ForceMode2D.Impulse);
    }
    public void AttackBuffer() //입력 먼저 받고 콤보 함수 실행될때 버퍼가 남아있으면 공격이 되게 함,애니메이션 이벤트로 실행
    {
        _attackBufferCheck = true;
    }
    public void ResetAttack() //애니메이션 이벤트로 실행, 모든 공격 변수들 초기화
    {
        _attackNum = 0;
        _rb2D.simulated = true;
        _attacking = false;
        _comboPossible = false;
        _airAttacking = false;
        _isMoveTrue = true;
        _attackBuffer = false;
        _attackBufferCheck = false;
        _airAttackReady = false;
    }
    public void Combo() //애니메이션 이벤트로 실행
    {
        _comboPossible = true;
        _attackNum++;
        if (_attackBuffer)
        {
            _attackBuffer = false; _attackBufferCheck = false;
            Attack();
        }

    }
    private void Anim()
    {

        _anim.SetBool("Move", _hor != 0);
        _anim.SetBool("Falling", Falling());
        _anim.SetBool("IsGround", IsGround());
    }
    public bool Falling()
    {
        return _rb2D.velocity.y < 0 && !IsGround();
    }
    private void Move()
    {
        if (_airAttackReady) _rb2D.velocity = Vector2.zero;
        if (!_isMoveTrue) return;

        _hor = Input.GetAxisRaw("Horizontal");
        Vector2 input = new Vector2(_hor * _speed, 0);
        if (_hor > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        if (_hor < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        _rb2D.AddForce(input, ForceMode2D.Impulse);

        if (transform.localScale.x > 0 && _rb2D.velocity.x > _maxSpeed)
            _rb2D.velocity = new Vector2(_maxSpeed, _rb2D.velocity.y);
        if (transform.localScale.x < 0 && _rb2D.velocity.x < -_maxSpeed)
            _rb2D.velocity = new Vector2(-_maxSpeed, _rb2D.velocity.y);
        if (IsGround())
        {

            if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("JumpUp") && _rb2D.velocity.x != 0)
            {

                if (Input.GetButton("Run"))
                {
                    _speed = _runSpeed;
                    _anim.SetFloat("MoveSpeed", 1);
                }
                else
                {
                    _speed = _walkSpeed;
                    _anim.SetFloat("MoveSpeed", .5f);

                }
                _maxSpeed = _speed;
            }
            if (Input.GetButtonDown("Jump"))
            {
                _anim.SetTrigger("Jump");
                print(_speed);
                if (_speed == _runSpeed)
                {

                    _speed = _airRunSpeed;
                }
                if (_speed == _walkSpeed)
                {
                    _speed = _airWalkSpeed;

                }
                _rb2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
            if (Input.GetButtonUp("Horizontal") || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                _rb2D.velocity = Vector2.zero;
            }

        }
    }
    private bool IsGround()
    {
        return Physics2D.OverlapCircle(_groundCheckTrans.position, _groundCheckRadius, _groundCheckLayerMask);
    }
}
