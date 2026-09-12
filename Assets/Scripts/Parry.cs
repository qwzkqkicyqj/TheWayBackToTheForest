using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public Player PlayerCode;
    public GameObject ParryEffectPrefab;
    public Transform PlayerTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyAttack") && Vector3.Distance(collision.transform.position, PlayerTransform.position) > Vector3.Distance(collision.transform.position, transform.position))
        {
            Debug.Log("패링 성공");
            PlayerCode.IsInvincible = true;
            collision.transform.parent.GetComponent<IEnemyStun>().Stun();
            Instantiate(ParryEffectPrefab, (collision.transform.position + gameObject.transform.position)/2, Quaternion.identity);
            PlayerCode.IsParrySuccess = true;
        }
    }

    public void OnParryEnd()
    {
        PlayerCode.IsParry = false;
        gameObject.SetActive(false);
    }
}
