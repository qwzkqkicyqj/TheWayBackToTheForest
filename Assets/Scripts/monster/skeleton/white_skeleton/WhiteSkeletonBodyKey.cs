using UnityEngine;
using System.Collections;

public class WhiteSkeletonBodyItemDrop : WhiteSkeletonBody
{
    public GameObject PotionPrefab;
    public GameObject KeyPrefab;
    Vector2 _itemForce;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") && !SkeletonCode.IsDie)
        {
            base.OnTriggerEnter2D(collision);
            if (SkeletonCode.IsDie)
            {
                _itemForce = new Vector2(Random.Range(-3, 3), 13f);
                GameObject _key = Instantiate(KeyPrefab, SkeletonObject.transform.position, Quaternion.identity);
                _key.GetComponent<Rigidbody2D>().AddForce(_itemForce, ForceMode2D.Impulse);
                _itemForce = new Vector2(Random.Range(-3, 3), 13f);
                GameObject _potion = Instantiate(PotionPrefab, SkeletonObject.transform.position, Quaternion.identity);
                _potion.GetComponent<Rigidbody2D>().AddForce(_itemForce, ForceMode2D.Impulse);
            }
        }
    }
}
