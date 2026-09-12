using System.Collections;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public bool IsParry = false;
    public Player PlayerCode;
    public Rigidbody2D PlayerRigidBody2D;
    public Animator PlayerAnimator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.CompareTag("EnemyAttack"))
        {
            IsParry = true;
            StartCoroutine(ParryDuration());
        }
    }

    IEnumerator ParryDuration()
    {
        Debug.Log("패링 발동중.");
        yield return TimeManager.s_Wait_0_2s;
        IsParry = false;
    }
}