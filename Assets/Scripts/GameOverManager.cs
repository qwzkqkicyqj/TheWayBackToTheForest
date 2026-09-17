using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    public CanvasGroup DieWindow;
    float GameOverWindowFadeTime = 3;
    public float GameOverWindowFadeTimer = 0;
    public AudioClip GameOver;
    static public GameOverManager Instance;
    private void Awake()
    {   
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnLoadedScene;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnGoMainMenu()
    {
        StartCoroutine(FadeEffectManager.Instance.FadeOut(0));
    }
    public void OnReTry()
    {
        StartCoroutine(FadeEffectManager.Instance.LoadGameFadeOut(DataManager.Instance.PlayerData.SlotNumber));
    }
    void OnLoadedScene(Scene scene, LoadSceneMode mode)
    {
        DieWindow.alpha = 0;
        if (scene.buildIndex == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    public IEnumerator GameOverWindwoFadeIn()
    {
        GameOverWindowFadeTimer = 0;
        yield return TimeManager.s_Wait_1s;
        SoundManager.Instance.SFXPlay(GameOver);
        while (GameOverWindowFadeTimer < GameOverWindowFadeTime)
        {
            GameOverWindowFadeTimer += Time.deltaTime;
            DieWindow.alpha = GameOverWindowFadeTimer / GameOverWindowFadeTime;
            yield return null;
        }
    }
}

