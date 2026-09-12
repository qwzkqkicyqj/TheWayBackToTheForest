using UnityEngine;
using System.Collections;

public class StartCouscene_2_1 : MonoBehaviour
{
    public CutsceneSkip CutsceneSkipCode;
    public GameObject PlayerCamera;
    public GameObject PlayerUI;
    public Player PlayerCode;
    public Player PlayerObject;
    public Animator PlayerAnimator;
    Coroutine _cutsceneSkip;
    void Start()
    {
        _cutsceneSkip = StartCoroutine(StartCutscene());
    }

    private void Update()
    {
        if(CutsceneSkipCode.IsCutsceneEnd)
        {
            StopCoroutine(_cutsceneSkip);
            PlayerUI.SetActive(true);
            FadeEffectManager.Instance.FadeSkip();
            PlayerAnimator.Play("player_idle", 0, 0);
            PlayerCode.IsStop = false;
            Destroy(gameObject);
        }
    }

    IEnumerator StartCutscene()
    {
        PlayerUI.SetActive(false);
        PlayerCode.IsStop = true;
        PlayerAnimator.Play("player_crouch_down");
        StartCoroutine(FadeEffectManager.Instance.Stage2FadeIn());
        while (!FadeEffectManager.Instance.FadeInEnd)
        {
            yield return null;
        }
        yield return TimeManager.s_Wait_2s;
        PlayerAnimator.SetTrigger("Crouch_up");
        yield return TimeManager.s_Wait_2s;
        PlayerCode.IsStop = false;
        Destroy(gameObject);
        PlayerUI.SetActive(true);

    }
}
