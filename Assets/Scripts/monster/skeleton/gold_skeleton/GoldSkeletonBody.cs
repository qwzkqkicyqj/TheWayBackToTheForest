using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using UnityEditor.Build;
using UnityEngine;
using System.Collections;

public class GoldSkeletonBody : MonoBehaviour
{
    public Animator SkeletonObjectAnimator;
    public GameObject SkeletonObject;
    public SpriteRenderer SkeletonObjectSpriteRenderer;
    public Rigidbody2D SkeletonObjectRigidbody2D;
    //public GameObject Player;
    public GoldSkeleton SkeletonCode;
    public GameObject KeyPrefab;
    public GameObject PotionPrefab;
    Vector2 _itemForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            if (SkeletonCode.Hp <= 0 || SkeletonCode.IsHurt) return;
            StartCoroutine(Hit());
            SkeletonCode.Hp -= 20;
            if (SkeletonCode.Hp <= 0)
            {
                _itemForce = new Vector2(Random.Range(-3, 3), 13f);
                GameObject _key = Instantiate(KeyPrefab, SkeletonObject.transform.position, Quaternion.identity);
                _key.GetComponent<Rigidbody2D>().AddForce(_itemForce, ForceMode2D.Impulse);
                _itemForce = new Vector2(Random.Range(-3, 3), 13f);
                GameObject _potion = Instantiate(PotionPrefab, SkeletonObject.transform.position, Quaternion.identity);
                _potion.GetComponent<Rigidbody2D>().AddForce(_itemForce, ForceMode2D.Impulse);
                SkeletonObjectRigidbody2D.linearVelocityX = 0;
                SkeletonCode.IsDie = true;
                SkeletonObjectAnimator.SetTrigger("die");
            }
        }
    }
    IEnumerator Hit()
    {
        SkeletonObjectSpriteRenderer.color = Color.red;
        yield return TimeManager.s_Wait_0_1s;
        SkeletonObjectSpriteRenderer.color = Color.white;
    }
}