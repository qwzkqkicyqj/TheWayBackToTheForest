using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Top : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public SpriteRenderer TopLightSpriteRenderer;
    public Animator Animator;
    public Animator TopLightAnimator;
    public BossPage1 BossCode;
    public GameObject TopLightObject;

    public int Hp = 200;
    bool isStart = false;
    public bool IsDie = false;

    void Update()
    {
        Animator.SetInteger("hp", Hp / 2);

        if (Hp == 200 && !isStart)
        {
            //StartCoroutine(StartLight());
            isStart = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerAttack" && !IsDie)
        {
            if(Hp > 0)
            {
                StartCoroutine(Hit());
                if (Hp == 20 && BossCode.HpHalf)
                {
                    return;
                }
                    

                Hp -= 100;
                if (Hp <= 0)
                {
                    TopLightAnimator.SetTrigger("die");
                    Animator.SetInteger("hp", 0);
                    IsDie = true;
                }
            }
        }
    }

    IEnumerator Hit()
    {
        SpriteRenderer.color = Color.red;
        yield return TimeManager.s_Wait_0_1s;
        SpriteRenderer.color = Color.white;
    }

    //IEnumerator StartLight()
    //{
    //    yield return TimeManager.s_Wait_0_5s;
    //    float fadetimer = 2;
    //    float timer = 0;
    //    while (fadetimer > timer)
    //    {
    //        timer += Time.deltaTime;
    //        Color color = TopLightSpriteRenderer.color;
    //        color.a = timer / fadetimer;
    //        TopLightSpriteRenderer.color = color;
    //        yield return null;
    //    }
    //}

    void OnRealDie()
    {
        Animator.SetTrigger("real_die");
    }
}
