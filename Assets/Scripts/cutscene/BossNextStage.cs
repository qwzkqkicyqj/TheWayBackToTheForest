using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossNextStage : MonoBehaviour
{

    public GameObject CutsceneBackground;
    public Player PlayerCode;
    public Player PlayerObject;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(NextStage());
        }
    }

    IEnumerator NextStage()
    {
        DataManager.Instance.PlayerData.IsClear = true;
        PlayerCode.IsStop = true;
        PlayerObject.GetComponent<Rigidbody2D>().linearVelocityX = 10f;
        UnityEngine.UI.Image _image = CutsceneBackground.GetComponent<UnityEngine.UI.Image>();
        float _fadeTimer = 2;
        float _timer = 0;
        while (_fadeTimer > _timer)
        {
            _timer += Time.deltaTime;
            Color color = _image.color;
            color.a = _timer / _fadeTimer;
            _image.color = color;
            yield return null;
        }
        yield return TimeManager.s_Wait_0_5s;
        SceneManager.LoadScene(0);
    }
}
