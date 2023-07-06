using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer_Controller : enemybace
{
    SpriteRenderer necro_Sprite;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            base.DeadCheck();
        }
    }
    void Attack1()
    {
        base.animator.SetBool("isAttack1", true);
        base.animator.SetTrigger("isAttack");
    }
    void Attack2()
    {
        base.animator.SetBool("isAttack2", true);
        base.animator.SetTrigger("isAttack");
    }
    }
