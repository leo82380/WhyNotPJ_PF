using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{

    public float Damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            //Enemybase enemybase = collision.GetComponent<Enemybase>(); ���� ��ũ��Ʈ�� �θ� ����� ��ũ��Ʈ
            //enemybase.ApplyDamage(Damage);
        }
        if(collision.CompareTag("Player"))
        {
            PlayerController pC = collision.GetComponent<PlayerController>();
            pC.ApplyDamage(Damage);
        }

    }
}
