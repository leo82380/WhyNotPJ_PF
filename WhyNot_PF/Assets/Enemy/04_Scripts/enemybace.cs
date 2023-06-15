using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemybace : MonoBehaviour
{
    protected Vector3 dir;
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected PlayerController player;
    protected Rigidbody2D rigid;
    [Header("속도")]
    [SerializeField]
    protected float speed;
    [Header("거리")]
    [SerializeField]
    protected int far;
    bool isAtack;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rigid = GetComponent<Rigidbody2D>();
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
            if (transform.position.x < player.transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
            return true;
        }
        return false;
    }
    void Move()
    {
        PlayerPos(far);
        Vector3 dir = new Vector3();
        transform.position += dir * Time.deltaTime * speed;
    }

    void AttackCheck()
    {
        if (!isAtack)
        {

            if (PlayerPos(far))
            {
                print(2);
                animator.SetTrigger("Attack");


            isAtack = true;
            }
            //attackcollider.enabled = true;
        }
    }

    void Hit()
    {
        PlayerPos(far);
        animator.SetTrigger("Hit");
    }

    void Die(float time)
    {
        PlayerPos(far);
        animator.SetTrigger("Die");
        Invoke("DestoryEnemy", time);
    }
    void DestoryEnemy()
    {
        Destroy(gameObject);
    }
}
