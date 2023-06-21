using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PetLevel
{
    LEVEL1,
    LEVEL2

}
public class Pet_Controller : MonoBehaviour
{
    private PetLevel _petLevel;
    public PetLevel PetLevel
    {
        get => _petLevel;
        set
        {
            _petLevel = value;
            print(2);
            ChangeForm();
        }
    }
    private RuntimeAnimatorController[] petLevelRunTimeAnimController;
    private Animator ani;
    private Transform ParentPlayer;
    private Transform ChildPlayer;
    [SerializeField]
    private float Speed = 1f;
    private Rigidbody2D rb;
    private CircleCollider2D dieGround;

    bool isDie = false;
    bool petLev1 = true;

    [SerializeField] GameObject skill1;
    [SerializeField] GameObject skill2;

    private  void Awake()
    {
        petLevelRunTimeAnimController = Resources.LoadAll<RuntimeAnimatorController>("PetController");
        dieGround = GetComponent<CircleCollider2D>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ParentPlayer = GameObject.FindWithTag("Player").transform;
        ChildPlayer = ParentPlayer.GetChild(1);
        ChangeForm();
    }


    void Update()
    {
        if (isDie == true) return;
        transform.position = Vector3.MoveTowards(transform.position, ChildPlayer.position, Speed);
        if(ParentPlayer.position.x> transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (ParentPlayer.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ani.SetTrigger("onSkill");
            if(petLev1 == true)
            {
                Instantiate(skill1, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(skill2, transform.position, Quaternion.identity);

            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (petLev1 == true) petLev1 = false;
            else petLev1 = true;
            PlayChangeAnim();
        }
    }
    
    public void ChangeForm()
    {
        print(PetLevel);
        ani.runtimeAnimatorController = petLevelRunTimeAnimController[(byte)PetLevel];
    }
    private  void PlayChangeAnim()
    {
        ani.SetTrigger("onChange");
    }

    private  void Die()
    {
        isDie = true;
        dieGround.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        ani.Play("Dead");
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    

}
