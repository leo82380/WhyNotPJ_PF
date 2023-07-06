using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Skill1 : MonoBehaviour
{
    SpriteRenderer playerImage;
    private void Awake()
    {
        playerImage = GameObject.Find("Pet").GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        playerImage.DOColor(Color.white, 0.5f);

    }
    public void delete()
    {
        playerImage.DOColor(Color.white, 1f);
        Destroy(gameObject);
    }



}
