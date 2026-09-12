using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WhiteSkeletonBodyItemDrop : MonoBehaviour
{
    public Animator SkeletonObjectAnimator;
    public GameObject SkeletonObject;
    public float KnockbackForce = 3;
    public GameObject Player;
    public WhiteSkeleton SkeletonCode;
    public GameObject KeyPrefab;
    public Collider2D SkeletonAttackHitboxCollder2D;
    public SpriteRenderer PlayerSpriteRenderer;
    public GameObject PotionPrefab;
    Vector2 _itemForce;
    public int _stunCount { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            if (SkeletonCode.Hp <= 0 || SkeletonCode.IsHurt) return;
            SkeletonCode.IsAttack = false;
            SkeletonCode.LastAttack = 0f;
            SkeletonCode.IsHurt = true;
            SkeletonObjectAnimator.SetTrigger("hurt");
            SkeletonCode.Hp -= 20;
            if (!PlayerSpriteRenderer.flipX)
            {
                SkeletonObject.GetComponent<SpriteRenderer>().flipX = true;
                SkeletonObject.GetComponent<Rigidbody2D>().AddForceX(KnockbackForce, ForceMode2D.Impulse);
            }
            else if (PlayerSpriteRenderer.flipX)
            {
                SkeletonObject.GetComponent<SpriteRenderer>().flipX = false;
                SkeletonObject.GetComponent<Rigidbody2D>().AddForceX(-KnockbackForce, ForceMode2D.Impulse);
            }
            if (SkeletonCode.Hp <= 0)
            {
                _itemForce = new Vector2(Random.Range(-3, 3), 13f);
                GameObject _key = Instantiate(KeyPrefab, SkeletonObject.transform.position, Quaternion.identity);
                _key.GetComponent<Rigidbody2D>().AddForce(_itemForce, ForceMode2D.Impulse);
                _itemForce = new Vector2(Random.Range(-3, 3), 13f);
                GameObject _potion = Instantiate(PotionPrefab, SkeletonObject.transform.position, Quaternion.identity);
                _potion.GetComponent<Rigidbody2D>().AddForce(_itemForce, ForceMode2D.Impulse);
                SkeletonObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
                SkeletonCode.IsDie = true;
                SkeletonObjectAnimator.SetTrigger("die");
            }
        }
    }
}
