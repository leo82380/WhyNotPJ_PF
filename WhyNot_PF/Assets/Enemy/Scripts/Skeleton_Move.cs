using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skeleton_Move : MonoBehaviour
{
    public Vector3 dir;
    Animator skeleton_anime;
    SpriteRenderer sprite;
    Transform playerpos;
    PlayerController player;
    CircleCollider2D attackCollider;
    Rigidbody2D rigid;

    [Header("속도")]
    [SerializeField] float speed = 2;
    int rd;
    [Header("거리")]
    [SerializeField] int far;
    int hitCount;
    int nextMove;
    public int look = 1;

    bool isWalk;
    bool isAttack;
    bool isHit;
    bool isRect;
    bool isDead;
    bool ismove;
    void Start()
    {
        nextMove = 1;
        hitCount = 3;
        playerpos = FindObjectOfType<PlayerController>().transform;
        player = FindObjectOfType<PlayerController>();
        skeleton_anime = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attackCollider = GetComponentInChildren<CircleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        attackCollider.enabled = false;
        StartCoroutine("Moving");
    }
    private void Update()
    {
        transform.position += dir * Time.deltaTime * speed;
        AttackCheck();

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1.5f, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            isWalk = false;
            look *= -1;
            dir = Vector2.right * look;
            sprite.flipX = !sprite.flipX;
            skeleton_anime.SetBool("isWalk", true);
            nextMove = look;
        }
    }
    
    IEnumerator Moving()
    {
        while (true)
        {
            int rdAct = UnityEngine.Random.Range(0, 100);
            if (rdAct < 80)
            {
                isWalk = true;
                Walk();
            }
            if (rdAct > 80 && rdAct < 100)
            {
                isRect = true;
                Rect();
            }
            yield return new WaitForSeconds(5);
        }
        
    }
    void DeadCheck()
    {
        hitCount--;
        if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= far)
        {
            if (transform.position.x < playerpos.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
        Debug.Log(hitCount);
        isHit = true;
        Hit();
        if(hitCount <= 0)
        {
            isDead = true;
            Dead();
        }
    }
    void AttackCheck()
    {
        if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= far)
        {
            Attack();
            if (transform.position.x < playerpos.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
    }
    void Attack()
    {
        if (!isAttack)
        {
            skeleton_anime.SetBool("isAttack", true);
            attackCollider.enabled = true;
            isAttack = true;
        }
    }
    public void EndAttack()
    {
        attackCollider.enabled = false;
        skeleton_anime.SetBool("isAttack", false);
        isAttack = false;
    }

    void Walk()
    {
        if (isWalk == true && ismove == false)
        {
            StartCoroutine("Walking");
        }
    }

    IEnumerator Walking()
    {
        ismove = true;
        int time = UnityEngine.Random.Range(10, 20);
        yield return new WaitForSeconds(time);
        rd = UnityEngine.Random.Range(0, 2);
        switch (rd)
        {
            case 0:
                look = 1;
                Move(look, false);
                break;
            case 1:
                look = -1;
                Move(look, true);
                break;
        }
    }
    void Move(int direction, bool isLeft)
    {
        dir = Vector2.right * direction;
        sprite.flipX = isLeft;
        skeleton_anime.SetBool("isWalk", true);
        nextMove = direction;
        StartCoroutine("StopWalk");
    }
    IEnumerator StopWalk()
    {
        yield return StartCoroutine("Walking");
        dir = Vector2.zero;
        ismove = false;
        transform.position += Vector3.zero;
        skeleton_anime.SetBool("isWalk", false);
    }
    void Hit()
    {
        if (isHit == true)
        {
            skeleton_anime.SetBool("isHit", true);
            Invoke("EndHit", 0.7f);
        }
    }
    void EndHit()
    {
        skeleton_anime.SetBool("isHit", false);
        isHit = false;
    }

    void Rect()
    {
        if (isRect == true)
        {
            skeleton_anime.SetBool("isRect", true);
            Invoke("EndRect", 0.7f);
        }
    }
    void EndRect()
    {
        skeleton_anime.SetBool("isRect", false);
        isRect = false;
    }

    void Dead()
    {
        if (isDead == true)
        {
            skeleton_anime.SetTrigger("OnDie");
            dir = Vector3.zero;
        }
    }
    public void SkeletonDie()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            DeadCheck();
        }
    }
}
