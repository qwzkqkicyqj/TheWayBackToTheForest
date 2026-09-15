using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using Microsoft.Unity.VisualStudio.Editor;
using System.Runtime.InteropServices;
using System.Threading;

public class Player : MonoBehaviour
{
    [Header("오디오 클립 관리")]
    public AudioClip AttackSound;
    public AudioClip JumpSound;
    public AudioClip DoubleJumpSound;
    public AudioClip RunSound;
    public AudioClip WarkSound;
    public AudioClip HitSound;
    public AudioClip DashSound;
    public AudioClip LandSound;


    [Header("이동 속도, 점프 힘, 체력 등 기본 스펙")]
    public float Speed = 10f; //이동 속도
    public float JumpPower = 15f; //점프 힘
    public float JumpPlusPower = 12f;
    public float DashSpeed = 20f;
    public float DashTime = 0.2f;
    public float CoyoteTimeDuration = 0.1f;
    int _doubleJumpCount = 1;
    float _coyoteTimeTimer = 0;

    [Header("Static변수들")]
    public static bool s_IsKey;
    public static int s_PotionCount = 0;
    public static int s_HP = 100;

    [Header("땅 체크 관련")]
    public Transform LeftGroundCheck; //왼쪽 발에 위치한 ground_check 위치 (인스펙터에서 설정)
    public Transform RightGroundCheck; //왼쪽 발에 위치한 ground_check 위치 (인스펙터에서 설정)
    public LayerMask Ground; //LayerMask형 ground 선언 (인스펙터에서 설정)
    float _groundCheckRadius = 0.1f; //ground_check의 중심을 기준으로 생길 원의 반지름
    bool _isGround; //땅에 있는지 확인
    bool _wasGround; // 공중에 있을때 isGround가 true가 되는 것을 막기 위한 변수
    bool _isJump; // 점프 중인지

    [Header("쿨타임")]
    public float AttackCooltime = 0.5f;
    private float _lastAttackTime = 0f;
    public float DashCooltime = 0.8f;
    private float _lastDashTime = 0f;
    private float _parryCooltime = 0.5f;
    private float _lastParryTime = 0f;


    [Header("기타 변수")]
    public GameObject AttackHitbox;
    public Transform AttackHitboxTransform;
    public Collider2D AttackHitboxCollider;
    public GameObject Parry;
    public Collider2D ParryCollider;
    public Transform ParryTransform;
    public Parry ParryCode;
    Vector3 _originalParryScale;
    Vector3 _originalParryPosition;
    public bool IsDash;
    public bool IsStop = false;
    public bool IsFPress = false; //상호작용 상태 확인
    public bool IsFHold = false;
    public bool IsDie;
    public bool IsParry = false;
    public bool IsInvincible = false;
    public bool IsParrySuccess = false;
    public bool IsUsePotion = false;
    public float UsePotionTime = 2f;
    public float UsePotionTimer = 0;


    Vector2 _dir; //이동 방향
    bool _isAttack = false; //공격 중인지 확인
    bool _isInteract = false; //상호작용 가능 상태 확인
    public bool _isHurt = false;
    bool _isInteractHold = false;

    [Header("컴포넌트 관리")]
    //컴포넌트 변수 선언
    public Animator Animator;
    public Rigidbody2D RigidBody2D;
    public SpriteRenderer SpriteRenderer;
    public TrailRenderer TrailRenderer;

    public GameObject HitEffectPrefeb;
    public GameObject ParryEffectPrefeb;
    public Transform HealEffectTransform;
    public GameObject HealEffect;

    Vector3 _originalHealEffectLocalScale;
    Vector3 _originalHealEffectLocalPosition;

    public CanvasGroup DieWindow;
    float _dieWindowTime = 3;
    public float _dieWindowTimer =0;
    bool _isFootStepSoundPlay = false;
    private void Awake()
    {
        _originalParryScale = ParryTransform.localScale;
        _originalParryPosition = ParryTransform.localPosition;
        _originalHealEffectLocalScale = HealEffectTransform.localScale;
        _originalHealEffectLocalPosition = HealEffectTransform.localPosition;
    }

    void Update()
    {
        _lastParryTime += Time.deltaTime;
        _lastAttackTime += Time.deltaTime;
        _lastDashTime += Time.deltaTime;
        Rotation();
        if(IsUsePotion)
        {
            Animator.SetBool("Run", false);
            Animator.SetBool("Walk", math.abs(RigidBody2D.linearVelocityX) > 0.1f);
        }
        else
        {
            Animator.SetBool("Walk", false);
            Animator.SetBool("Run", math.abs(RigidBody2D.linearVelocityX) > 0.1f);
        }
        Animator.SetBool("Ground", _isGround);
        Animator.SetBool("Crouch", IsStop);
        Animator.SetBool("Fall", RigidBody2D.linearVelocityY < -0.1f);
        if(IsParrySuccess)
        {
            IsParrySuccess = false;
            StartCoroutine(InvincibleDurationn());
        }
        if(IsUsePotion && !IsStop && !IsDie && !_isHurt)
        {
            UsePotion();
        }
        else
        {
            UsePotionTimer = 0;
        }

        if (!_isFootStepSoundPlay && _isGround && math.abs(RigidBody2D.linearVelocityX) > 0.1f)
        {
            if(!IsUsePotion && !_isAttack)
            {
                _isFootStepSoundPlay = true;
                SoundManager.Instance.FootStepPlay(RunSound);
            }
            else
            {
                _isFootStepSoundPlay = true;
                SoundManager.Instance.FootStepPlay(WarkSound);
            }            
        }
        else if(_isFootStepSoundPlay && (!_isGround || math.abs(RigidBody2D.linearVelocityX) < 0.1f))
        {
            _isFootStepSoundPlay = false;
            SoundManager.Instance.FootStepStop();
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (!IsStop && !IsDash && !_isHurt && !IsParry && !IsUsePotion)
        {
            RigidBody2D.linearVelocityX = Speed * _dir.x;
        }
        else if(IsParry)
        {
            RigidBody2D.linearVelocityX = 0f;
        }
        else if(IsUsePotion)
        {
            RigidBody2D.linearVelocityX = 5 * _dir.x;
        }
    }

    void OnMove(InputValue value)
    {
        _dir = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && !IsDash && !IsParry && !IsUsePotion)
        {
            if (_coyoteTimeTimer > 0 && !IsStop)
            {
                //if (_isGround) Debug.Log("일반 점프");
                //else Debug.Log("코요테 점프");

                SoundManager.Instance.SFXPlay(JumpSound);
                RigidBody2D.linearVelocityY = JumpPower;
                Animator.SetTrigger("Jump");
                _isJump = true;
            }
            else if (_doubleJumpCount > 0 && !IsStop)
            {
                _doubleJumpCount--;
                //SoundManager.Instance.SoundPlay(DoubleJumpSound);
                RigidBody2D.linearVelocityY = JumpPlusPower;
            }
        }
        else //가변 점프 (점프 버튼을 떼었을 때 낙하)
        {
            if (RigidBody2D.linearVelocityY > 0)
            {
                RigidBody2D.linearVelocityY *= 0.5f;
            }
        }
    }
    // 이동 방향에 따라 캐릭터의 좌우 반전 여부 결정
    void Rotation() 
    {
        if (!IsStop && !IsDash && !_isHurt && !_isAttack && !IsParry)
        {
            if (_dir.x == 1)
            {
                SpriteRenderer.flipX = false;
                AttackHitboxTransform.localScale = new Vector3(1, 1, 1);
                ParryTransform.localScale = new Vector3(_originalParryScale.x, _originalParryScale.y, _originalParryScale.z);  
                ParryTransform.localPosition = new Vector3(_originalParryPosition.x, _originalParryPosition.y, _originalParryPosition.z);
                HealEffectTransform.localScale = _originalHealEffectLocalScale;
                HealEffectTransform.localPosition = _originalHealEffectLocalPosition;
                HealEffectTransform.localRotation = Quaternion.Euler(0, 0, -81);
            }
            else if (_dir.x == -1)
            {
                SpriteRenderer.flipX = true;
                AttackHitboxTransform.localScale = new Vector3(-1, 1, 1);
                ParryTransform.localScale = new Vector3(-_originalParryScale.x, _originalParryScale.y, _originalParryScale.z);
                ParryTransform.localPosition = new Vector3(-_originalParryPosition.x, _originalParryPosition.y, _originalParryPosition.z);
                HealEffectTransform.localScale = new Vector3(-_originalHealEffectLocalScale.x, _originalHealEffectLocalScale.y, _originalHealEffectLocalScale.z);
                HealEffectTransform.localPosition = new Vector3(-_originalHealEffectLocalPosition.x, _originalHealEffectLocalPosition.y, _originalHealEffectLocalPosition.z);
                HealEffectTransform.localRotation = Quaternion.Euler(0, 0, 81);
            }
        }
    }

    void GroundCheck()
    {
        //왼발 혹은 오른발의 원이 ground와 겹친다면(닿는다면) -> is_ground = true
        _isGround = Physics2D.OverlapCircle(RightGroundCheck.position, _groundCheckRadius, Ground) || Physics2D.OverlapCircle(LeftGroundCheck.position, _groundCheckRadius, Ground);
        if (_isGround)
        {
            _coyoteTimeTimer = CoyoteTimeDuration;
            _doubleJumpCount = 1;
        }
        else if (!_isGround)
        {
            if (!_isJump)
            {
                _coyoteTimeTimer -= Time.deltaTime;
            }
            else if (_isJump)
            {
                _coyoteTimeTimer = 0f;
            }
        }
        if (!_wasGround && _isGround)
        {
            _isJump = false;
            SoundManager.Instance.SFXPlay(LandSound);
        }
        _wasGround = _isGround;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Damage") && !IsDash && !_isHurt && !IsInvincible && !IsDie) //충돌한 오브젝트의 태그가 damage라면
        {
            s_HP -= 20;
            if (s_HP <= 0)
            {
                s_HP = 0;
                Die();
            }
            else
            {
                Instantiate(HitEffectPrefeb, collision.contacts[0].point, quaternion.identity);
                StartCoroutine(GetDamage()); //get_dagage() 실행 (IEnumerator는 StartCoroutine이 반드시 필요하다.)
                RigidBody2D.linearVelocityY = 10f; //위로 10만큼 이동
                _doubleJumpCount = 1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage") && !IsDash && !IsDie && !_isHurt && !IsInvincible) //충돌한 오브젝트의 태그가 damage라면
        {
            s_HP -= 20;
            if (s_HP <= 0)
            {
                s_HP = 0;
                Die();  
            }
            else
            {
                StartCoroutine(GetDamage()); //get_dagage() 실행 (IEnumerator는 StartCoroutine이 반드시 필요하다.)
                RigidBody2D.linearVelocityY = 5f; //위로 10만큼 이동
            }   
        }
        if (collision.CompareTag("EnemyAttack") && ParryCollider.IsTouching(collision) && !_isHurt && Vector3.Distance(collision.transform.position, transform.position) > Vector3.Distance(collision.transform.position, ParryTransform.transform.position))
        {
            Debug.Log("패링 작동");
            StartCoroutine(InvincibleDurationn());
        }
        else if (collision.CompareTag("EnemyAttack") && !IsDash && !_isHurt && !IsInvincible && !IsDie)
        {
            Debug.Log("패링 실패");
            StartCoroutine(HurtDuration());
            ParryCode.OnParryEnd();
            Animator.Play("player_idle", -1, 0);
            OnAttackEnd();
            s_HP -= 20;
            if (s_HP <= 0)
            {
                s_HP = 0;   
                Die();
            }
            else
            {
                StartCoroutine(GetDamage()); //get_dagage() 실행 (IEnumerator는 StartCoroutine이 반드시 필요하다.)
                Instantiate(HitEffectPrefeb, collision.ClosestPoint(transform.position), quaternion.identity);
                RigidBody2D.linearVelocityY = 3f;
                if (transform.position.x > collision.transform.position.x)
                {
                    RigidBody2D.AddForceX(3f, ForceMode2D.Impulse);
                }
                else
                {
                    RigidBody2D.AddForceX(-3f, ForceMode2D.Impulse);
                }
            }
        }
    }

    IEnumerator HurtDuration()
    {
        _isHurt = true;
        yield return TimeManager.s_Wait_0_2s;
        _isHurt = false;
    }

    IEnumerator InvincibleDurationn()
    {
        IsInvincible = true;
        yield return TimeManager.s_Wait_1_5s;
        IsInvincible = false;
    }

    private IEnumerator GetDamage()
    {
        Time.timeScale = 0f; //게임을 멈춤
        SpriteRenderer.color = Color.red;
        yield return TimeManager.s_WaitRealTime_0_15;
        Time.timeScale = 1f; //게임을 다시 재생
        SpriteRenderer.color = Color.white;
    }



    // 공격 입력이 들어왔을 때 발동하는 함수
    void OnAttack() 
    {
        if (!IsStop && _lastAttackTime >= AttackCooltime && !IsDash && !IsUsePotion)
        {
            _isAttack = true;
            _lastAttackTime = 0;
            Animator.SetTrigger("Attack");

            Speed = 5f; // 공격할 때 이동 속도 감소
        }
    }
    
    void OnAttackSoundPlay()
    {
        SoundManager.Instance.SFXPlay(AttackSound);
    }

    void OnAttackEnd()
    {
        Speed = 10;
        _isAttack = false;
    }

    IEnumerator Dash()
    {
        IsDash = true;
        //SoundManager.Instance.SoundPlay(DashSound);
        float original_gravity_scale = RigidBody2D.gravityScale; //현재 중력 스케일 저장
        RigidBody2D.linearVelocity = Vector2.zero;
        RigidBody2D.gravityScale = 0f; //대시 중에는 중력 무시
        Animator.SetTrigger("Dash");
        TrailRenderer.emitting = true; //대쉬 잔상 생성
        RigidBody2D.linearVelocityX = DashSpeed * (SpriteRenderer.flipX ? -1 : 1); //대시 방향으로 힘 가하기
        yield return new WaitForSeconds(DashTime); //dash_time만큼 대시 유지
        RigidBody2D.gravityScale = original_gravity_scale; //대시 후 원래 중력으로 변경
        yield return TimeManager.s_Wait_0_2s;
        TrailRenderer.emitting = false; //대쉬 잔상 생성 끄기
    }

    void OnDash()
    {
        if (_lastDashTime >= DashCooltime && !_isAttack && !IsStop &&!IsUsePotion)
        {
            _lastDashTime = 0f;
            StartCoroutine(Dash());
        }
    }

    void OnDashEnd()
    {
        IsDash = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.transform.CompareTag("Interact"))
        {
            _isInteract = true;
        }
        if (collision.gameObject.transform.CompareTag("InteractHold"))
        {
            _isInteractHold = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.CompareTag("Interact"))
        {
            _isInteract = false;
        }
        if (collision.gameObject.transform.CompareTag("InteractHold"))
        {
            _isInteractHold = false;
        }
    }

    void OnInteract()
    {
        if (_isInteract && !IsStop)
        {
            IsFPress = IsFPress ? false : true;
        }
    }

    public void OnHold(InputValue value)
    {
        if (_isInteractHold && s_IsKey)
        {
            if (value.isPressed)
            {
                IsFHold = true;
                IsStop = true;
            }
            else
            {
                IsFHold = false;
                IsStop = false;

            }
        }
    }

    void OnParry()
    {
        if(!_isHurt && _lastParryTime >= _parryCooltime && !IsDash && !_isAttack && !IsDie && !IsParry && !IsUsePotion && !IsStop)
        {
            IsParry = true;
            _lastParryTime = 0;
            Parry.SetActive(true);
        }
    }

    void OnDrawGizmos()
    {
        if (LeftGroundCheck != null)
        {
            bool hit = Physics2D.OverlapCircle(
                LeftGroundCheck.position,
                _groundCheckRadius,
                Ground);

            Gizmos.color = hit ? Color.green : Color.red;
            Gizmos.DrawWireSphere(LeftGroundCheck.position, _groundCheckRadius);
        }

        if (RightGroundCheck != null)
        {
            bool hit = Physics2D.OverlapCircle(
                RightGroundCheck.position,
                _groundCheckRadius,
                Ground);

            Gizmos.color = hit ? Color.green : Color.red;
            Gizmos.DrawWireSphere(RightGroundCheck.position, _groundCheckRadius);
        }
    }
    void OnUsePotion(InputValue value)
    {
        if(value.isPressed && !IsStop && !IsDie && !_isHurt && s_PotionCount > 0)
        {
            IsUsePotion = true;
        }
        else
        {
            IsUsePotion = false;
        }
    }
    void UsePotion()
    {
        UsePotionTimer += Time.deltaTime;
        if(UsePotionTimer >= UsePotionTime)
        {
            s_PotionCount--;
            if(s_HP <= 50)
            {
                s_HP += 50;
            }
            else
            {
                s_HP = 100;
            }
            HealEffect.SetActive(true);
            UsePotionTimer = 0;
            IsUsePotion = false;
        }
    }
    void Die()
    {
        IsDie = true;
        IsStop = true;
        Animator.Play("PlayerDie");
        gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0f;
    }
    void OnDieEnd()
    {
        StartCoroutine(DIeWindow());
    }
    IEnumerator DIeWindow()
    {
        yield return TimeManager.s_Wait_1s;
        while(_dieWindowTimer < _dieWindowTime)
        {
            _dieWindowTimer += Time.deltaTime;
            DieWindow.alpha = _dieWindowTimer / _dieWindowTime;
            yield return null;
        }
    }
}

