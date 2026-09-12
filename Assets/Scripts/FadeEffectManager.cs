using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeEffectManager : MonoBehaviour
{
    public static FadeEffectManager Instance;
    public GameObject FadeObject;
    public Image FadeObjectImage;
    public bool FadeInEnd = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public IEnumerator FadeIn()
    {
        FadeObject.SetActive(true);
        Color FadeColor = FadeObjectImage.color;
        float _fadeTimer = 2;
        float _timer = 0;
        while (_fadeTimer > _timer)
        {
            _timer += Time.deltaTime;
            FadeColor.a = 1 - _timer / _fadeTimer;
            FadeObjectImage.color = FadeColor;
            yield return null;
        }
        FadeObject.SetActive(false);
        FadeInEnd = true;
        yield return TimeManager.s_Wait_0_015s;
        FadeInEnd = false;
    }

    public IEnumerator Stage2FadeIn()
    {
        FadeObject.SetActive(true);
        Color FadeColor = FadeObjectImage.color;
        float _fadeTimer = 3;
        float _timer = 0;
        FadeColor.a = 1;
        FadeObjectImage.color = FadeColor;
        yield return TimeManager.s_Wait_2s;
        while (_fadeTimer > _timer)
        {
            _timer += Time.deltaTime;
            FadeColor.a = 1 - _timer / _fadeTimer;
            FadeObjectImage.color = FadeColor;
            yield return null;
        }
        FadeObject.SetActive(false);
        FadeInEnd = true;
        yield return TimeManager.s_Wait_0_015s;
        FadeInEnd = false;
    }
    public IEnumerator FadeOut(int i)
    {
        FadeObject.SetActive(true);
        Color FadeColor = FadeObjectImage.color;
        float _fadeTimer = 2;
        float _timer = 0;
        while (_fadeTimer > _timer)
        {
            _timer += Time.deltaTime;
            FadeColor.a = _timer / _fadeTimer;
            FadeObjectImage.color = FadeColor;
            yield return null;
        }
        yield return TimeManager.s_Wait_0_5s;
        SceneManager.LoadScene(i);
    }
    public IEnumerator LoadGameFadeOut(int i)
    {
        FadeObject.SetActive(true);
        Color FadeColor = FadeObjectImage.color;
        float _fadeTimer = 2;
        float _timer = 0;
        while (_fadeTimer > _timer)
        {
            _timer += Time.deltaTime;
            FadeColor.a = _timer / _fadeTimer;
            FadeObjectImage.color = FadeColor;
            yield return null;
        }
        yield return TimeManager.s_Wait_0_5s;
        DataManager.Instance.PlayerDataLoad(i);
    }
    public void FadeSkip()
    {
        FadeObject.SetActive(false);
        FadeInEnd = false;
    }
}
