using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Controller : MonoBehaviour
{
    Animator ani;
    Transform ParentPlayer;
    Transform ChildPlayer;
    [SerializeField]
    float Speed = 1f;
    void Start()
    {
        ani = GetComponent<Animator>();
        ParentPlayer = GameObject.FindWithTag("Player").transform;
        ChildPlayer = ParentPlayer.GetChild(1);
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, ChildPlayer.position, Speed);
        if(ParentPlayer.position.x> transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (ParentPlayer.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    public void Die()
    {
        ani.Play("1_Dead");
    }
}
