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
    [Header("�ӵ�")]
    [SerializeField, Range(0, 10)]
    private float _walkSpeed;
    [SerializeField, Range(0, .1f)]
    private float _airWalkSpeed;
    [SerializeField, Range(0, .1f)]
    private float _airRunSpeed;
    [SerializeField, Range(0, 10)]
    private float _runSpeed;
    private float _speed;
    private float _maxSpeed;
    #endregion
    #region Jump
    [Header("����")]
    [SerializeField]
    private LayerMask _groundCheckLayerMask;
    private Transform _groundCheckTrans;
    [SerializeField]
    private float _groundCheckRadius;
    [SerializeField, Range(0, 10)]
    private float _jumpForce;
    #endregion
    #region Component
    private Animator _anim;
    private Rigidbody2D _rb2D;
    private CapsuleCollider2D cC;
    #endregion
    #region Attack
    private bool _attackBuffer;
    private bool _attackBufferCheck;
    [Header("����")]
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

    [SerializeField, Range(0, 20)]
    private float _airAttackForce;
    #endregion
    #region Slide
    private bool _sliding;
    [Header("�����̵�")]
    [SerializeField, Range(0, 10)]
    private float _slideSpeed;
    [SerializeField, Range(0, 2)]
    private float _slideTime;
    #endregion
    #region Roll
    [HideInInspector]
    public bool Rolling;

    [Header("������")]
    private float _rollSpeed;
    [SerializeField, Range(0, 10)]
    private float _rollWalkSpeed;
    [SerializeField, Range(0, 10)]
    private float _rollRunSpeed;
    #endregion

    #region HP
    private float hp;
    [SerializeField]
    private float maxHP;

    public float HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHP);
            if (hp <= 0)
            {
                print("����");
            }
        }
    }
    #endregion



    private WeaponCollider weaponCollider;
    private void Awake()
    {
        _groundCheckTrans = transform.Find("GroundCheck").transform;
        _attackCol = transform.Find("AttackCol").gameObject;
        _anim = GetComponent<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
        cC = GetComponent<CapsuleCollider2D>();
        _isMoveTrue = true;
        weaponCollider = transform.GetComponentInChildren<WeaponCollider>();
        weaponCollider.Damage = AttackPower;
    }
    private void Start()
    {
        _speed = _walkSpeed;
        _maxSpeed = _walkSpeed;
        _attackBufferCheck = false;
        _attackBuffer = false;
        DisableAttackCol();
        hp = maxHP;
    }
    private void Update()
    {
        Move();
        Anim();
        AttackCheck();
        SlideCheck();
        //SlopCheck();
    }

    private void SkillInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            
        }
    }
    private void SlideCheck()
    {
        if (Input.GetKeyDown(KeyCode.C) && !_sliding && !Rolling && !_attacking && !_airAttacking && Mathf.Abs(_hor) > 0)
        {

            _sliding = true;
            _rb2D.AddForce((Vector2.right * transform.localScale.x) * _slideSpeed, ForceMode2D.Impulse);
            _anim.SetBool("Sliding", true);
            StartCoroutine(EndSlide());
        }
    }

    IEnumerator EndSlide()
    {
        yield return new WaitForSeconds(_slideTime);
        _sliding = false;
        _anim.SetBool("Sliding", false);
        _rb2D.velocity = new Vector2(_rb2D.velocity.x * .5f, _rb2D.velocity.y);
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
            else if (!_airAttacking)
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
        if (Input.GetKeyDown(KeyCode.Z) && !Rolling &&!_sliding)
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
    public void AirAttackReady() //���� ���ݿ��� ��� ���� ��� ���߰� ��� ���� �ö󰡰� ��.
    {
        transform.DOMoveY(transform.position.y + .5f, .4f).SetEase(Ease.OutSine);

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
    public void AirAttack() //���߰��ݿ��� ���
    {
        _airAttackReady = false;
        _rb2D.AddForce(Vector2.down * _airAttackForce, ForceMode2D.Impulse);
    }
    public void AttackBuffer() //�Է� ���� �ް� �޺� �Լ� ����ɶ� ���۰� ���������� ������ �ǰ� ��,�ִϸ��̼� �̺�Ʈ�� ����
    {
        _attackBufferCheck = true;
    }
    public void ResetAttack() //�ִϸ��̼� �̺�Ʈ�� ����, ��� ���� ������ �ʱ�ȭ
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
    public void Combo() //�ִϸ��̼� �̺�Ʈ�� ����
    {
        _comboPossible = true;
        _attackNum++;
        if (_attackBuffer)
        {
            _attackBuffer = false; _attackBufferCheck = false;
            Attack();
        }

    }

    public void ApplyDamage(float Dam)
    {
        _anim.Play("Hit");
        HP -= Dam;
    }
    private void Anim()
    {

        _anim.SetBool("Move", _hor != 0);
        _anim.SetBool("Falling", Falling());
        _anim.SetBool("IsGround", IsGround());
    }

    private void Move()
    {
        _hor = Input.GetAxisRaw("Horizontal");
        if (_airAttackReady) _rb2D.velocity = Vector2.zero;
        if (!_isMoveTrue || _sliding) return;

        Vector2 input = new Vector2(_hor * _speed, 0);

        _rb2D.AddForce(input, ForceMode2D.Impulse);


        if (!_sliding && !Rolling)
        {
            if (_hor > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            if (_hor < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            if (transform.localScale.x > 0 && _rb2D.velocity.x > _maxSpeed)
                _rb2D.velocity = new Vector2(_maxSpeed, _rb2D.velocity.y);
            if (transform.localScale.x < 0 && _rb2D.velocity.x < -_maxSpeed)
                _rb2D.velocity = new Vector2(-_maxSpeed, _rb2D.velocity.y);
        }
        if(Rolling)
        {
            _rb2D.velocity = transform.right *transform.localScale.x * _rollSpeed;
        }

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
            if (Input.GetKeyDown(KeyCode.X) && !Rolling)
            {
                _anim.SetTrigger("Roll");

                if (_speed == _runSpeed)
                {

                    _rollSpeed = _rollRunSpeed;
                }
                if (_speed == _walkSpeed)
                {
                    _rollSpeed = _rollWalkSpeed;

                }
                Rolling = true;

            }
            if (Input.GetButtonDown("Jump"))
            {
                _anim.SetTrigger("Jump");
               if(Rolling)
                {
                    Rolling = false;
                }
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
            if (!_sliding && (Input.GetButtonUp("Horizontal") || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
            {
                _rb2D.velocity = Vector2.zero;
            }
        }
    }

    private bool IsGround()
    {
        return Physics2D.OverlapCircle(_groundCheckTrans.position, _groundCheckRadius, _groundCheckLayerMask);
    }
    public bool Falling()
    {
        return _rb2D.velocity.y < 0 && !IsGround();
    }
    public void RotateWhileAttack()
    {
        if (_hor > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        if (_hor < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }
}
