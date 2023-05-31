using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Move : MonoBehaviour
{
    [SerializeField] float speed = 3;
    Animator skeleton_anime;
    [SerializeField] bool isWalk;
    [SerializeField] bool isAttack;
    [SerializeField] bool isHit;
    [SerializeField] bool isRect;
    [SerializeField] bool isDead;
    [SerializeField] int rd;
    // Start is called before the first frame update
    void Start()
    {
        skeleton_anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttack == true)
        {
            skeleton_anime.SetBool("isAttack", true);
            Invoke("EndAttack", 1.07f);
        }

        if(isWalk == true)
        {
            rd = Random.Range(0, 2);
            switch (rd)
            {
                case 0:
                    transform.position += Vector3.right * Time.deltaTime * speed;
                    skeleton_anime.SetBool("isWalk", true);
                    break;
                case 1:
                    transform.position += Vector3.left * Time.deltaTime * speed;
                    skeleton_anime.SetBool("isWalk", true);
                    break;
            }
            
        }
        else
        {
            transform.position = Vector3.zero;
            skeleton_anime.SetBool("isWalk", false);
        }

        if(isHit == true)
        {
            skeleton_anime.SetBool("isHit", true);
            Invoke("EndHit", 0.7f);
        }
        if(isRect == true)
        {
            skeleton_anime.SetBool("isRect", true);
            Invoke("EndRect", 0.7f);
        }
        if(isDead == true)
        {
            skeleton_anime.SetTrigger("OnDie");
            Invoke("SkeletonDie", 1.2f);
        }
    }
    void EndAttack()
    {
        skeleton_anime.SetBool("isAttack", false);
        isAttack = false;
    }
    void EndHit()
    {
        skeleton_anime.SetBool("isHit", false);
        isHit = false;
    }
    void EndRect()
    {
        skeleton_anime.SetBool("isRect", false);
        isRect = false;
    }
    void SkeletonDie()
    {
        Destroy(gameObject);
    }
}
