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
    protected Transform scale;
    protected PlayerController player;
    protected Collider2D attackCollider;
    protected Collider2D defaultCollider;
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
    [SerializeField]
    protected float hp = 5;
    protected bool isDead;
    protected bool isChasing;
    public float HP
    {
        get => hp;

        set => hp = value;
        
    }
    protected virtual void Awake()
    {
        attackCollider = transform.GetChild(0).GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        defaultCollider = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scale = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        animator.SetBool("isWalk", false);
        attackCollider.enabled = false;
    }
    protected virtual void Update()
    {
        dir.y = 0;
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
        else if (dis == far)
        {
            isChasing = false;
        }   
        else if (Vector3.Distance(gameObject.transform.position, player.transform.position) > dis)
        {
            return false;
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
                attackCollider.enabled = true;
                animator.SetTrigger("isAttack");
                isAttacking = true;
            }
            else
            {
                Move();
                EndAttack();
            }
            
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
    }

    void Die()
    {
        speed = 0;
        PlayerPos(far);
        animator.SetTrigger("Die");
        StartCoroutine(DestoryEnemy(3));
    }
    public void EndAttack()
    {
        attackCollider.enabled = false;
        isAttacking = false;
    }
    IEnumerator DestoryEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    public void ApplyDamage(float Damage)
    {
        Hit();
        hp -= Damage;
        print($"<color=black>{gameObject.name}</color> 남은 hp:  {hp}");
        if (hp <= 0)
        {
            print($"<color=black>{gameObject.name}</color> 죽음");
            defaultCollider.enabled = false;
            rigid.gravityScale = 0;
            Die();
        }
    }
}
