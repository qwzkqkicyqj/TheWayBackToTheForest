using UnityEngine;

public class End2_1 : MonoBehaviour
{
    public GameObject PlayerCamera;
    public Player PlayerCode;
    public Player PlayerObject;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            NextStage();
        }
    }

    void NextStage()
    {
        PlayerCode.IsStop = true;
        PlayerObject.GetComponent<Rigidbody2D>().linearVelocityX = 10f;
        StartCoroutine(FadeEffectManager.Instance.FadeOut(3));
    }
}
