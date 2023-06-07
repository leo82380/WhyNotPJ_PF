using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Move : MonoBehaviour
{
    Animator skeleton_anime;
    SpriteRenderer sprite;
    Transform playerpos;
    PlayerController player;
    CircleCollider2D attackCollider;
    [SerializeField] float speed = 2;
    [SerializeField] int rd;
    [SerializeField] int far;
    int hitCount;
    bool isWalk;
    bool isAttack;
    bool isHit;
    bool isRect;
    bool isDead;
    bool ismove;
    bool isCount;
    Vector3 dir;
    void Start()
    {
        hitCount = 3;
        playerpos = FindObjectOfType<PlayerController>().transform;
        player = FindObjectOfType<PlayerController>();
        skeleton_anime = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attackCollider = GetComponentInChildren<CircleCollider2D>();
        attackCollider.enabled = false;
        StartCoroutine(Moving());
    }
    private void Update()
    {
        transform.position += dir * Time.deltaTime * speed;
        AttackCheck();
    }

    IEnumerator Moving()
    {
        while (true)
        {
            int rdAct = Random.Range(0, 100);

            
            Hit();
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
            Dead();
            yield return new WaitForSeconds(5);
        }
        
    }
    void DeadCheck()
    {
        hitCount--;
        isCount = false;
            
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
        int time = Random.Range(5, 10);
        yield return new WaitForSeconds(time);
        rd = Random.Range(0, 2);
        switch (rd)
        {
            case 0:
                Move(1, false);
                break;
            case 1:
                Move(-1, true);
                break;
        }
    }
    void Move(int direction, bool isLeft)
    {
        dir = Vector2.right * direction;
        sprite.flipX = isLeft;
        skeleton_anime.SetBool("isWalk", true);
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
            Invoke("SkeletonDie", 1.2f);
        }
    }
    void SkeletonDie()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DeadCheck();
        }
    }
}
