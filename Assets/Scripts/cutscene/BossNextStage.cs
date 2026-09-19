using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossNextStage : MonoBehaviour
{
    public Player PlayerCode;
    public Rigidbody2D PlayerRigidbody2D;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DataManager.Instance.PlayerData.IsClear = true;
            PlayerCode.IsStop = true;
            PlayerRigidbody2D.linearVelocityX = 10f;
            StartCoroutine(FadeEffectManager.Instance.FadeOut(0));
        }
    }
}
