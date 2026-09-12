using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;
using TMPro;

public class StartCutscene1 : MonoBehaviour
{
    public MainMenuAndStage1Player Player;
    public GameObject Barrier;
    public GameObject QuestionMark;
    public GameObject ExclamationMark;
    public GameObject Message1;
    public GameObject Message2;
    public GameObject Message3;
    public GameObject Message4;
    public GameObject Message5;
    public GameObject MoveMessage;
    public Rigidbody2D PlayerRigidBody2D;
    public SpriteRenderer PlayerSpriteRenderer;
    public CutsceneSkip CutsceneSkipCode;
    public GameObject CutsceneSkipObject;
    public Transform PlayerTransform;
    Coroutine _cutsceneSkip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CutsceneSkipObject.SetActive(true);
        _cutsceneSkip = StartCoroutine(cutscene());
    }
    private void Update()
    {
        if(CutsceneSkipCode.IsCutsceneEnd)
        {
            StopCoroutine(_cutsceneSkip);
            QuestionMark.SetActive(false);
            Message1.SetActive(false);
            Message2.SetActive(false);
            Message3.SetActive(false);
            PlayerSpriteRenderer.flipX = false;
            ExclamationMark.SetActive(false);
            Message4.SetActive(false);
            Message5.SetActive(false);
            Player.IsEnd = true;    
            Barrier.SetActive(true);
            PlayerTransform.position = new Vector3(154.8972f, -2.969146f, 0);
            Destroy(gameObject);
        }
    }
    IEnumerator cutscene()
    {
        yield return TimeManager.s_Wait_1s;
        Message1.SetActive(true);
        yield return TimeManager.s_Wait_2s;
        Message1.SetActive(false);
        yield return TimeManager.s_Wait_1s;
        Message2.SetActive(true);
        yield return TimeManager.s_Wait_2s;
        Message2.SetActive(false);
        yield return TimeManager.s_Wait_0_7s;
        Message3.SetActive(true);
        yield return TimeManager.s_Wait_0_05s;
        Player.IsStop = true;
        QuestionMark.SetActive(false);
        ExclamationMark.SetActive(true);
        yield return TimeManager.s_Wait_1s;
        Message3.SetActive(false);
        yield return TimeManager.s_Wait_0_3s;
        PlayerSpriteRenderer.flipX = true;
        yield return TimeManager.s_Wait_0_9s;
        PlayerSpriteRenderer.flipX = false;
        yield return TimeManager.s_Wait_0_7s;
        ExclamationMark.SetActive(false);
        yield return TimeManager.s_Wait_1_1s;
        Message4.SetActive(true);
        yield return TimeManager.s_Wait_4_5s;
        Message4.SetActive(false);
        Message5.SetActive(true);
        yield return TimeManager.s_Wait_3s;
        Message5.SetActive(false);
        Player.IsEnd = true;
        Barrier.SetActive(true);
        Destroy(gameObject);
    }
}
