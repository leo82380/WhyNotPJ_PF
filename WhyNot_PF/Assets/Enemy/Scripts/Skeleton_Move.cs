using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Move : MonoBehaviour
{
    Animator skeleton_anime;
    SpriteRenderer sprite;
    Transform player;
    [SerializeField] float speed = 2;
    [SerializeField] int rd;
    [SerializeField] int far;
    [SerializeField] bool isWalk;
    [SerializeField] bool isAttack;
    [SerializeField] bool isHit;
    [SerializeField] bool isRect;
    [SerializeField] bool isDead;
    [SerializeField] bool ismove;
    
    Vector3 dir;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        skeleton_anime = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        while (true)
        {
            int rdAct = Random.Range(0, 100);

            
            Hit();
            if (rdAct < 50)
            {
                isWalk = true;
                Walk();
            }
            if (rdAct > 50 && rdAct < 60)
            {
                isRect = true;
                Rect();
            }
            Dead();
            yield return new WaitForSeconds(5);
        }
        
    }
    private void Update()
    {
        transform.position += dir * Time.deltaTime * speed;
        AttacCheck();
    }
    void AttacCheck()
    {
        if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= far)
        {
            if (transform.position.x < player.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
            isAttack = true;
            Attack();
        }
    }
    void Attack()
    {
        if (isAttack == true)
        {
            skeleton_anime.SetBool("isAttack", true);
            Invoke("EndAttack", 1.07f);
        }
    }
    void EndAttack()
    {
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
                dir = Vector2.right;
                sprite.flipX = false;
                skeleton_anime.SetBool("isWalk", true);
                StartCoroutine("StopWalk");
                break;
            case 1:
                dir = Vector2.right * -1;
                sprite.flipX = true;
                skeleton_anime.SetBool("isWalk", true);
                StartCoroutine("StopWalk");
                break;
        }
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
        Destroy(gameObject);
    }
}
