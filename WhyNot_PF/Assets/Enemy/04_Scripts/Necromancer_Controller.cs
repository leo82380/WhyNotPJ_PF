using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer_Controller : MonoBehaviour
{
    Animator necro_Anime;
    SpriteRenderer necro_Sprite;
    PlayerController player;
    void Start()
    {
        necro_Anime = GetComponent<Animator>();
        necro_Sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        
    }
    private void Update()
    {
        int rd = Random.Range(0, 100);
        if (rd < 50)
        {
            StartCoroutine(Move(1));
        }
        else
        {
            StartCoroutine(Move(-1));
        }
    }
    IEnumerator Move(int look)
    {
        while (true)
        {
            Vector3 dir = new Vector3();
            if (look == 1)
            {
                necro_Sprite.flipX = false;
                dir = Vector3.right;
            }
            else if (look == -1)
            {
                necro_Sprite.flipX = true;
                dir = Vector3.left;
            }
            transform.position += dir * Time.deltaTime;
            yield return null;
        }
    }
}
