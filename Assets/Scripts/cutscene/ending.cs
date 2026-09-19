using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public Transform GoMainMenu;
    public Transform Player;
    public Transform BackGround;
    public bool EndingCutSceneEnd = false;
    public GameObject MainMenu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("정상 실행");
            StartCoroutine(FadeEffectManager.Instance.FadeIn());
        }
    }

    void Update()
    {
        if(Player.position.x >= GoMainMenu.position.x)
        {
            Player.position = new Vector2(-2.969164f, Player.position.y);
            BackGround.position = new Vector2(-2.969164f, BackGround.position.y);
            EndingCutSceneEnd = true;
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
    }
}
