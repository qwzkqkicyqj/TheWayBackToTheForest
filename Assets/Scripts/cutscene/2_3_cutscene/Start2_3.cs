using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Start2_3 : MonoBehaviour
{
    public GameObject CutsceneCamera;
    public GameObject PlayerCamera;
    public GameObject CutsceneBackground;
    //public Transform[] CameraPoint;
    public Player PlayerCode;
    public Rigidbody2D PlayerRigidBody2D;
    public GameObject Message1;
    public GameObject Message2;
    public GameObject Message3;
    public Rigidbody2D BossRigidBody2D;
    public Transform BossTransform;
    public Top LeftTop;
    public Top RightTop;
    public GameObject LockDoor;
    public GameObject UnlockDoor;
    public CutsceneSkip CutsceneSkipCode;
    public GameObject CutsceneSkipObject;
    public bool IsPage1Start;
    public GameObject UI;
    public Image CutsceneBackgroundImage;
    public CinemachineCamera CutsceneCameraCinemachineCamera;
    Coroutine _cutsceneSkip;
    public SpriteRenderer LeftTopLightSpriteRenderer;
    public SpriteRenderer RightTopLightSpriteRenderer;
    public SpriteRenderer BarrierSpriteRenderer;
    public SpriteRenderer EyesSpriteRenderer;
    public BossPage1 BossPage1Code;
    public GameObject CutSceneSkip;

    void Start()
    {
        _cutsceneSkip = StartCoroutine(StartCutscene());
    }

    void Update()
    {
        if (CutsceneSkipCode.IsCutsceneEnd)
        {
            StartCoroutine(CutsceneSkip());
        }
    }

    IEnumerator StartCutscene()
    {
        UI.SetActive(false);
        PlayerCode.IsStop = true;
        PlayerRigidBody2D.linearVelocityX = 10f;
        yield return TimeManager.s_Wait_0_5s;
        StartCoroutine(FadeEffectManager.Instance.FadeIn());
        while (!FadeEffectManager.Instance.FadeInEnd)
        {
            yield return null;
        }

        yield return TimeManager.s_Wait_0_5s;
        PlayerRigidBody2D.linearVelocityX = 0f;
        CutsceneSkipObject.SetActive(true);
        yield return TimeManager.s_Wait_0_7s;
        UnlockDoor.SetActive(false);
        LockDoor.SetActive(true);
        yield return TimeManager.s_Wait_0_7s;
        CutsceneCameraCinemachineCamera.Priority = 2;
        Message1.SetActive(true);
        yield return TimeManager.s_Wait_2s;
        Message1.SetActive(false);
        Message2.SetActive(true);
        yield return TimeManager.s_Wait_2s;
        Message2.SetActive(false);
        Message3.SetActive(true);
        yield return TimeManager.s_Wait_2s;
        Message3.SetActive(false);
        yield return TimeManager.s_Wait_1s;
        while (math.abs(Vector2.Distance(BossTransform.position, new Vector2(35.78f, 7.23f))) > 0.01f)
        {
            BossTransform.position = Vector2.MoveTowards(BossTransform.position, new Vector2(35.78f, 7.23f), 5 * Time.deltaTime);
            yield return null;
        }
        while (LeftTop.Hp < 200)
        {
            LeftTop.Hp++;
            RightTop.Hp++;
            yield return TimeManager.s_Wait_0_015s;
        }
        yield return TimeManager.s_Wait_0_5s;
        float fadetimer = 2;
        float timer = 0;
        StartCoroutine(StartBarrier());
        while (fadetimer > timer)
        {
            timer += Time.deltaTime;
            Color _lightColor = LeftTopLightSpriteRenderer.color;
            _lightColor.a = timer / fadetimer;
            LeftTopLightSpriteRenderer.color = _lightColor;
            RightTopLightSpriteRenderer.color = _lightColor;
            yield return null;
        }
    }
    IEnumerator StartBarrier()
    {
        yield return TimeManager.s_Wait_0_5s;
        float fadetimer = 2;
        float timer = 0;
        while (fadetimer > timer)
        {
            timer += Time.deltaTime;
            Color _BarrierColor = BarrierSpriteRenderer.color;
            _BarrierColor.a = (timer / fadetimer) / 2.5f;
            BarrierSpriteRenderer.color = _BarrierColor;
            yield return null;
        }
        yield return TimeManager.s_Wait_1s;
        timer = 0;
        while (fadetimer > timer)
        {
            timer += Time.deltaTime;
            Color _eyesColor = EyesSpriteRenderer.color;
            _eyesColor.a = timer / fadetimer;
            EyesSpriteRenderer.color = _eyesColor;
            yield return null;
        }
        PlayerCode.IsStop = false;
        UI.SetActive(true);
        BossPage1Code.EyesStart = true;
        CutSceneSkip.SetActive(false);
        yield return TimeManager.s_Wait_1s;
        BossPage1Code.isStart = true;
        Destroy(gameObject);
    }
    IEnumerator CutsceneSkip()
    {
        FadeEffectManager.Instance.FadeSkip();
        UI.SetActive(false);
        StopCoroutine(_cutsceneSkip);
        PlayerCode.IsStop = false;
        UnlockDoor.SetActive(false);
        LockDoor.SetActive(true);
        Message1.SetActive(false);
        Message2.SetActive(false);
        Message3.SetActive(false);
        CutsceneCameraCinemachineCamera.Priority = 2;
        BossTransform.position = new Vector2(35.78f, 7.23f);
        LeftTop.Hp = 200;
        RightTop.Hp = 200;
        Color _lightColor = LeftTopLightSpriteRenderer.color;
        _lightColor.a = 1;
        LeftTopLightSpriteRenderer.color = _lightColor;
        RightTopLightSpriteRenderer.color = _lightColor;
        Color _barrierColor = BarrierSpriteRenderer.color;
        _barrierColor.a = 1 / 2.5f;
        BarrierSpriteRenderer.color = _barrierColor;
        EyesSpriteRenderer.color = _lightColor;
        PlayerCode.IsStop = false;
        UI.SetActive(true);
        BossPage1Code.EyesStart = true;
        LeftTop.Animator.Play("100");
        RightTop.Animator.Play("100");
        CutSceneSkip.SetActive(false);
        yield return TimeManager.s_Wait_1s;
        BossPage1Code.isStart = true;
        Destroy(gameObject);
    }
}
