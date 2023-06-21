using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skeleton_Move : enemybace
{
    //Animator animator;
    //SpriteRenderer sprite;
    //Transform playerpos;
    //PlayerController player;

    //Rigidbody2D rigid;

    [Header("속도")]
    //[SerializeField] float speed = 2;
    int rd;
    [Header("거리")]
    //[SerializeField] int far;
    //int hitCount;
    int nextMove;
    public int look = 1;

    bool isWalk;
    //bool isAttack;
    //bool isDead;
    bool isMove;
    protected override void Awake()
    {

        base.Awake();

    }
    void Start()
    {
        nextMove = 1;
        //hitCount = 3;
       // playerpos = FindObjectOfType<PlayerController>().transform;
        //player = FindObjectOfType<PlayerController>();
        //animator = GetComponent<Animator>();
        //sprite = GetComponent<SpriteRenderer>();
     
        //rigid = GetComponent<Rigidbody2D>();
        StartCoroutine("Moving");
    }
    protected override void Update()
    {
        base.Update();
       
        //AttackCheck();

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down * 10, new Color(1, 0, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 10f, LayerMask.GetMask("Ground"));
        if (rayHit.collider == null && !isChasing)
        {
        
            isWalk = false;
            look *= -1;
            dir = Vector2.right * look;
            sprite.flipX = !sprite.flipX;
            animator.SetBool("isWalk", true);
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
                Rect();
            }
            yield return new WaitForSeconds(5);
        }
        
    }
    //void DeadCheck()
    //{
    //    hitCount--;
    //    if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= far)
    //    {
    //        if (transform.position.x < player.transform.position.x)
    //        {
    //            sprite.flipX = false;
    //        }
    //        else
    //        {
    //            sprite.flipX = true;
    //        }
    //    }
    //    Debug.Log(hitCount);
    //    Hit();
    //    if(hitCount <= 0)
    //    {
    //        isDead = true;
    //        Dead();
    //    }
    //}
    //void AttackCheck()
    //{
    //    if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= far)
    //    {
    //        Attack();
    //        if (transform.position.x < playerpos.position.x)
    //        {
    //            sprite.flipX = false;
    //        }
    //        else
    //        {
    //            sprite.flipX = true;
    //        }
    //    }
    //}
    //void Attack()
    //{
    //    if (!isAttack)
    //    {
    //        animator.SetBool("isAttack", true);
    //        attackCollider.enabled = true;
    //        isAttack = true;
    //    }
    //}
   

    void Walk()
    {
        if (isWalk == true && isMove == false)
        {
            StartCoroutine("Walking");
        }
    }

    IEnumerator Walking()
    {
        isMove = true;
        int time = UnityEngine.Random.Range(10, 20);
        rd = UnityEngine.Random.Range(0, 2);
        yield return new WaitForSeconds(time);
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
        if (isChasing) return;
        StartCoroutine("StopWalk");
        dir = Vector2.right * direction;
        sprite.flipX = isLeft;
        animator.SetBool("isWalk", true);
        nextMove = direction;
    }
    IEnumerator StopWalk()
    {
        yield return StartCoroutine("Walking");
        dir = Vector2.zero;
        isMove = false;
        transform.position += Vector3.zero;
        animator.SetBool("isWalk", false);
    }
    void Hit()
    {
            animator.SetBool("isHit", true);
            Invoke("EndHit", 0.7f);
    }
    void EndHit()
    {
        animator.SetBool("isHit", false);
    }

    void Rect()
    {
        animator.SetBool("isRect", true);
        Invoke("EndRect", 0.7f);
    }
    void EndRect()
    {
        animator.SetBool("isRect", false);
    }

    void Dead()
    {
        if (isDead == true)
        {
            animator.SetTrigger("OnDie");
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
            base.DeadCheck();
        }
    }
}
