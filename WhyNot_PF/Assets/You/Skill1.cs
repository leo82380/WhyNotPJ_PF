using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Skill1 : MonoBehaviour
{
    public static Skill1 Instance;
    SpriteRenderer playerImage;

    [ColorUsage(true,true)]
    private Color originColor;
    [ColorUsage(true,true),SerializeField]
    private Color changeColor;

    private void Awake()
    {
        playerImage = GameObject.Find("Pet").GetComponent<SpriteRenderer>();
        originColor = playerImage.material.GetColor("_Color");
        
    }
    void Start()
    {
        playerImage.material.DOColor(changeColor, "_Color", 1);
    }
    public void delete()
    {
        playerImage.material.DOColor(originColor, "_Color", .5f);
        Destroy(gameObject);
    }


    
}
