using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
    IDLE,
    CHASE,
    FIND
}
public class enemybace : MonoBehaviour
{
    State state;
    protected Vector3 dir;
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected Transform scale;
    protected PlayerController player;
    protected Collider2D attackCollider;
    protected Rigidbody2D rigid;
    [Header("속도")]
    [SerializeField]
    protected float speed;
    [Header("거리")]
    [SerializeField]
    protected int far;
    [SerializeField]
    protected int attackFar;
    protected bool isAttacking;
    protected float hp = 5;
    protected bool isDead;
    protected bool isChasing;
    public float HP
    {
        get => hp;

        set
        {
            hp = value;
            //if (hp <= 0)
            //{
            //    isDead = true;
            //    Die(3);
            //}
        }
    }
    protected virtual void Awake()
    {
        attackCollider = GetComponentInChildren<CircleCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scale = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        attackCollider.enabled = false;
    }
    protected virtual void Update()
    {
        transform.position += dir * Time.deltaTime * speed;

        AttackCheck();
    }
    bool PlayerPos(float dis)
    {
        if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= dis)
        {
            if (dis == far)
            {
                dir = (player.transform.position - transform.position).normalized;
                isChasing = true;

            }
            if (transform.position.x < player.transform.position.x)
            {
                scale.localScale = new Vector3(2, 2, 2);
            }
            else
            {
                scale.localScale = new Vector3(-2, 2, 2);
            }
            return true;
        }
        else
             if (dis == far)
        {
            isChasing = false;

        }   
        return false;
    }
    void Move()
    {
        PlayerPos(far);
        animator.SetBool("isWalk", true);
    }

    void AttackCheck()
    {
        if (!isAttacking)
        {

            if (PlayerPos(attackFar))
            {
                print(2);
                animator.SetTrigger("isAttack");
                isAttacking = true;
            }
            else
            {
                Move();
            }
            //attackcollider.enabled = true;
        }
    }

    void Hit()
    {
        PlayerPos(far);
        animator.SetTrigger("Hit");
    }
    protected virtual void DeadCheck()
    {
        PlayerPos(far);
        Hit();
    }

    void Die(float time)
    {
        PlayerPos(far);
        animator.SetTrigger("Die");
        Invoke("DestoryEnemy", time);
    }
    public void EndAttack()
    {
        attackCollider.enabled = false;
        isAttacking = false;
    }
    void DestoryEnemy()
    {
        Destroy(gameObject);
    }
    public void ApplyDamage(float Damage)
    {
        Hit();
        hp -= Damage;
        print(hp);
        if (hp <= 0)
        {
            DestoryEnemy();
        }
    }
}
