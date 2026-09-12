using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject CutsceneBackground;
    public Transform GoMainMenu;
    public Transform Player;

    void Start()
    {
        StartCoroutine(EndingStart());
    }

    void Update()
    {
        if(Player.position.x >= GoMainMenu.position.x)
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator EndingStart()
    {
        CutsceneBackground.SetActive(true);
        UnityEngine.UI.Image _image = CutsceneBackground.GetComponent<UnityEngine.UI.Image>();
        yield return TimeManager.s_Wait_0_5s;
        float _fadeTimer = 2;
        float _timer = 0;
        while (_fadeTimer > _timer)
        {
            _timer += Time.deltaTime;
            Color color = _image.color;
            color.a = 1 - _timer / _fadeTimer;
            _image.color = color;
            yield return null;
        }
    }
}
