using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WhiteSkeletonBody : MonoBehaviour
{
    public Animator SkeletonObjectAnimator;
    public GameObject SkeletonObject;
    public float KnockbackForce = 3;
    public GameObject Player;
    public WhiteSkeleton SkeletonCode;
    public SpriteRenderer PlayerSpriteRenderer;
    public SpriteRenderer SkeletonSpriteRenderer;
    public Rigidbody2D SkeletonRigidBody2D;

    public int _stunCount { get; set; }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            if (SkeletonCode.Hp <= 0 || SkeletonCode.IsHurt) return;
            StartCoroutine(GetDabage());
            SkeletonCode.IsAttack = false;
            SkeletonCode.LastAttack = 0f;
            SkeletonCode.IsHurt = true;
            SkeletonObjectAnimator.SetTrigger("hurt");
            StartCoroutine(GetDabage());
            SkeletonCode.Hp -= 20;
            if (!PlayerSpriteRenderer.flipX)
            {
                SkeletonObject.GetComponent<SpriteRenderer>().flipX = true;
                SkeletonRigidBody2D.linearVelocityX = 0;
                SkeletonRigidBody2D.AddForceX(KnockbackForce, ForceMode2D.Impulse);
            }
            else if (PlayerSpriteRenderer.flipX)
            {
                SkeletonObject.GetComponent<SpriteRenderer>().flipX = false;
                SkeletonRigidBody2D.linearVelocityX = 0;
                SkeletonRigidBody2D.AddForceX(-KnockbackForce, ForceMode2D.Impulse);
            }
            if (SkeletonCode.Hp <= 0)
            {
                SkeletonObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
                SkeletonCode.IsDie = true;
                SkeletonObjectAnimator.SetTrigger("die");
            }
        }
    }

    IEnumerator GetDabage()
    {
        SkeletonSpriteRenderer.color = Color.red;
        yield return TimeManager.s_Wait_0_1s;
        SkeletonSpriteRenderer.color = Color.white;
    }
}
