using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Threading;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
public class BossPage1 : MonoBehaviour
{
    public GameObject TornadoPrefab;
    GameObject _tornado;
    public GameObject TornadoDoublePrefab;
    GameObject _tornadoDouble;
    public GameObject MeteorPrefab;
    GameObject _nakha;
    public GameObject BoomPrefab;
    GameObject _boom;
    public GameObject BouncePrefab;
    GameObject _bounce;
    public GameObject GuidedMissilePrefab;
    GameObject _guidedMissile;
    float _leftNakhaXPosition;
    float _rightNakhaXPosition;
    float _nakhaYPosition;
    public Transform PlayerTransform;
    int _attackNum;
    public Transform[] Point;
    public Top LeftTop;
    public Top RightTop;
    public bool HpHalf = false;
    public bool isDoubleAttackStart = false;
    public bool isAttack = false;
    public bool isTopHeal = false;
    public CinemachineCamera[] TopCameraCinemachineCamera;
    public CinemachineCamera CameraCinemachineCamera;
    public Player PlayerCode;
    public Rigidbody2D PlayerRigidBody2D;
    public bool is1PageEnd;
    public bool isStart = false;
    public SpriteRenderer BarrierSpriteRenderer;
    public SpriteRenderer EyesSpriteRenderer;
    public bool EyesStart = false;
    public Rigidbody2D BossRigidBody2D;
    public BossPage1 BossPage1Code;
    public BossPage2 BossPage2Code;
    public ObjectPool<GameObject> MeteorPool;
    float _meteor;

    private void Awake()
    {
        MeteorPool = new ObjectPool<GameObject>(CreateMeteor, OnGetMeteor, OnReleaseMeteor, OnDestroyMeteor, true, maxSize: 212);
    }
    void Update()
    {
        Application.targetFrameRate = 60;

        if(isStart)
        {
            if ((LeftTop.IsDie || RightTop.IsDie) && !isDoubleAttackStart)
            {
                HpHalf = true;
            }
            if (HpHalf && !isAttack && !isDoubleAttackStart)
            {
                StartCoroutine(HpHalfCutscene());
            }
            if (LeftTop.IsDie && RightTop.IsDie && !is1PageEnd && !isAttack)
            {
                StartCoroutine(End());
                is1PageEnd = true;
            }
            if (!is1PageEnd)
            {
                Attack();
            }
        }
    }
    
    void Attack()
    {
        if (!isAttack && !HpHalf && !isDoubleAttackStart && !is1PageEnd)
        {
            _attackNum = UnityEngine.Random.Range(1, 6);
            switch (2)
            {
                case 1:
                    isAttack = true;
                    _tornado = Instantiate(TornadoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    break;

                case 2:
                    isAttack = true;
                    if (math.abs(PlayerTransform.position.x - Point[0].position.x) < math.abs(PlayerTransform.position.x - Point[1].position.x))
                    {
                        StartCoroutine(NakhaLeft());
                    }
                    else
                    {
                        StartCoroutine(NakhaRight());
                    }
                    break;

                case 3:
                    isAttack = true;
                    StartCoroutine(BoomAttack());
                    break;

                case 4:
                    isAttack = true;
                    _bounce = Instantiate(BouncePrefab, new Vector3(34.59559f, 5.45012f, 0), quaternion.identity);
                    break;
                case 5:
                    isAttack = true;
                    _guidedMissile = Instantiate(GuidedMissilePrefab, new Vector3(21.21074f, 10.34557f, 0), quaternion.identity);
                    break;
            }
        }
        else if(!isAttack && isDoubleAttackStart && !is1PageEnd && !HpHalf)
        {
            _attackNum = UnityEngine.Random.Range(1, 6);
            switch (_attackNum)
            {
                case 1:
                    isAttack = true;
                    _tornadoDouble = Instantiate(TornadoDoublePrefab, new Vector3(0, 0, 0), quaternion.identity);
                    break;

                case 2:
                    isAttack = true;
                    StartCoroutine(NakhaLeft());
                    StartCoroutine(NakhaDouble());
                    break;

                case 3:
                    isAttack = true;
                    StartCoroutine(BoomDouble());
                    break;

                case 4:
                    isAttack = true;
                    _bounce = Instantiate(BouncePrefab, new Vector3(34.59559f, 5.45012f, 0), quaternion.identity);
                    break;

                case 5:
                    isAttack = true;
                    _guidedMissile = Instantiate(GuidedMissilePrefab, new Vector3(21.21074f, 10.34557f, 0), quaternion.identity);
                    break;
            }
        }
    }

    IEnumerator NakhaLeft()
    {
        isAttack = true;
        _leftNakhaXPosition = 18.5f;
        while (_leftNakhaXPosition < 53.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_leftNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _leftNakhaXPosition++;
            yield return null;
        }
        while (_leftNakhaXPosition > 17.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_leftNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _leftNakhaXPosition--;
            yield return null;
        }
        while (_leftNakhaXPosition < 53.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_leftNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _leftNakhaXPosition++;
            yield return null;
        }
        yield return TimeManager.s_Wait_3s;
        isAttack = false;
    }
    IEnumerator NakhaRight()
    {
        isAttack = true;
        _rightNakhaXPosition = 52.5f;
        while (_rightNakhaXPosition > 17.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_rightNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _rightNakhaXPosition--;
            yield return null;
        }
        while (_rightNakhaXPosition < 53.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_rightNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _rightNakhaXPosition++;
            yield return null;
        }
        while (_rightNakhaXPosition > 17.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_rightNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _rightNakhaXPosition--;
            yield return null;
        }
        yield return TimeManager.s_Wait_3s;

        isAttack = false;
    }
    IEnumerator NakhaDouble()
    {
        isAttack = true;
        _rightNakhaXPosition = 52.5f;
        while (_rightNakhaXPosition > 17.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_rightNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _rightNakhaXPosition--;
            yield return null;
        }
        while (_rightNakhaXPosition < 53.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_rightNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _rightNakhaXPosition++;
            yield return null;
        }
        while (_rightNakhaXPosition > 17.5)
        {
            _nakha = MeteorPool.Get();
            _nakha.transform.position = new Vector3(_rightNakhaXPosition, 13, 0);
            yield return TimeManager.s_Wait_0_05s;
            _rightNakhaXPosition--;
            yield return null;
        }
    }
    IEnumerator BoomAttack()
    {
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_1_5s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_1_5s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_1_5s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_1_5s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_3s;

        isAttack = false;
    }

    IEnumerator BoomDouble()
    {
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_0_2s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_1_5s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_0_2s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_1_5s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_0_2s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_1_5s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_0_2s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_1_5s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_0_2s;
        _boom = Instantiate(BoomPrefab);
        yield return TimeManager.s_Wait_3s;
        isAttack = false;
    }

    IEnumerator HpHalfCutscene()
    {
        PlayerCode.IsStop = true;
        PlayerRigidBody2D.linearVelocityX  = 0;
        if (RightTop.IsDie)
        {
            TopCameraCinemachineCamera[0].Priority = 3;
            yield return TimeManager.s_Wait_2_5s;
            while (LeftTop.Hp < 200)
            {
                LeftTop.Hp++;
                yield return TimeManager.s_Wait_2s;
            }
        }
        if (LeftTop.IsDie)
        {
            TopCameraCinemachineCamera[1].Priority = 3;
            yield return TimeManager.s_Wait_2_5s;
            while (RightTop.Hp < 200)
            {
                RightTop.Hp++;
                yield return TimeManager.s_Wait_2s;
            }
        }
        yield return TimeManager.s_Wait_3s;
        CameraCinemachineCamera.Priority = 5;
        yield return TimeManager.s_Wait_1s;
        isDoubleAttackStart = true;
        PlayerCode.IsStop = false;
        yield return TimeManager.s_Wait_1s;
        HpHalf = false;
    }
    IEnumerator End()
    {
        PlayerCode.IsStop = true;
        PlayerCode.RigidBody2D.linearVelocityX = 0;
        float _fadeTimer = 2;
        float _timer = 0;
        while (_fadeTimer > _timer)
        {
            _timer += Time.deltaTime;
            Color color = EyesSpriteRenderer.color;
            color.a = 1 - _timer / _fadeTimer;
            EyesSpriteRenderer.color = color;
            yield return null;
        }
        _timer = 0;
        while (_fadeTimer > _timer)
        {
            _timer += Time.deltaTime;
            Color color = BarrierSpriteRenderer.color;
            color.a = 1 / 2.5f - (_timer / _fadeTimer);
            BarrierSpriteRenderer.color = color;
            yield return null;
        }
        BossRigidBody2D.gravityScale = 3;
        yield return TimeManager.s_Wait_1s;
        PlayerCode.IsStop = false;
        BossPage2Code.enabled = true;
        enabled = false;
    }

    //Meteor 메서드
    GameObject CreateMeteor()
    {
        return Instantiate(MeteorPrefab);
    }
    void OnGetMeteor(GameObject meteor)
    {
        meteor.SetActive(true);
    }
    private void OnReleaseMeteor(GameObject meteor)
    {
        meteor.tag = "Damage";
        meteor.GetComponent<Rigidbody2D>().gravityScale = 5;
        meteor.SetActive(false);
    }
    private void OnDestroyMeteor(GameObject meteor)
    {
        Destroy(meteor);
    }
}