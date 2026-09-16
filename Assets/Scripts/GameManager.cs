using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public CanvasGroup DieWindow;
    float _dieWindowTime = 3;
    public float _dieWindowTimer = 0;
    public AudioClip GameOver;
    static public GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public IEnumerator GameOverWindwoFadeIn()
    {
        yield return TimeManager.s_Wait_1s;
        SoundManager.Instance.SFXPlay(GameOver);
        while (_dieWindowTimer < _dieWindowTime)
        {
            _dieWindowTimer += Time.deltaTime;
            DieWindow.alpha = _dieWindowTimer / _dieWindowTime;
            yield return null;
        }
    }
}
