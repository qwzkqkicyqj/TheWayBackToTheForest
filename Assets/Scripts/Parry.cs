using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public Player PlayerCode;
    public GameObject ParryEffectPrefab;
    public Transform PlayerTransform;
    public AudioClip ParrySound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyAttack") && Vector3.Distance(collision.transform.position, PlayerTransform.position) > Vector3.Distance(collision.transform.position, transform.position))
        {
            Debug.Log("패링 성공");
            PlayerCode.IsInvincible = true;
            collision.transform.parent.GetComponent<IEnemyStun>().Stun();
            SoundManager.Instance.SFXPlay(ParrySound);
            StartCoroutine(HitStop());
            Instantiate(ParryEffectPrefab, (collision.transform.position + gameObject.transform.position)/2, Quaternion.identity);
            PlayerCode.IsParrySuccess = true;
        }
    }

    public void OnParryEnd()
    {
        PlayerCode.IsParry = false;
        gameObject.SetActive(false);
    }

    IEnumerator HitStop()
    {
        //Time.timeScale = 0.1f;
        //Debug.Log("멈춤");
        yield return new WaitForSecondsRealtime(0.15f);
        //Time.timeScale = 1;
        //Debug.Log("멈춤 끝");
    }
}
