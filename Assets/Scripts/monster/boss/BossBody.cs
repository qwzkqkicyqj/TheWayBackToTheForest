using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using UnityEditor.Build;
using UnityEngine;
using System.Collections;

public class BossBody : MonoBehaviour
{
    public Animator BossAnimator;
    public GameObject BossObject;
    public float Knockback;
    public GameObject Player;
    public BossPage2 BossCode;
    public Rigidbody2D BossRigidBody2D;
    public SpriteRenderer BossSpriteRenderer;
    public GameObject StunEffect;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            if (BossCode.Hp <= 0 || BossCode.isHurt) return;
            StartCoroutine(Hit());
            BossCode.Hp -= 20;
            if (BossCode.Hp <= 0)
            {
                BossRigidBody2D.linearVelocityX = 0;
                BossCode.isDie = true;
                BossCode.IsStun = false;
                StunEffect.SetActive(false);
                BossAnimator.SetTrigger("die");
            }
        }
    }
    IEnumerator Hit()
    {
        BossSpriteRenderer.color = Color.red;
        yield return TimeManager.s_Wait_0_1s;
        BossSpriteRenderer.color = Color.white;
    }
}