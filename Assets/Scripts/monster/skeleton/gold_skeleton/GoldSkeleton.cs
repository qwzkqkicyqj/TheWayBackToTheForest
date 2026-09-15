using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class GoldSkeleton : MonoBehaviour, IEnemyStun
{
    public int _stunCount { get; set; }

    float _dir;
    public Rigidbody2D RigidBody2D;
    public SpriteRenderer SpriteRenderer;
    public GoldSkeletonRange SkeletonRange;
    public Animator Animator;
    public GameObject AttackHitbox;
    public SpriteRenderer PlayerSpriteRenderer;
    public GameObject StunEffect;

    public float Hp = 100;
    public float LastAttack;
    float _speed = 4f;
    float _attackCooltime = 0.5f;

    public bool IsAttack;
    public bool IsHurt = false;
    public bool IsDie;
    public bool IsStun = false;
    bool _isAttack2 = false;



    //이모션 변수
    public GameObject QuestionMark;
    public GameObject ExclamationMark;
    Vector3 _originalPosition;
    float _rotation;
    Vector3 _originalStunPosition;



    void Start()
    {
        RigidBody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 60;
        Animator = GetComponent<Animator>();
        float rotation = QuestionMark.transform.localRotation.x;
        float position = QuestionMark.transform.localPosition.x;
        _originalPosition = QuestionMark.transform.localPosition;
        _rotation = QuestionMark.transform.localRotation.z;
        _originalStunPosition = StunEffect.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDie)
        {
            LastAttack += Time.deltaTime;
            Rotation();
            Animator.SetBool("run", math.abs(RigidBody2D.linearVelocityX) > 0.0001f && !IsStun);
            Attack();
        }
        Emotion();
    }
    private void FixedUpdate()
    {
        Move();
    }

    void Emotion()
    {
        if (IsStun)
        {
            ExclamationMark.SetActive(false);
            QuestionMark.SetActive(false);
            StunEffect.SetActive(true);
        }
        else
        {
            if (SkeletonRange.Target != null)
            {
                StunEffect.SetActive(false);
                QuestionMark.SetActive(false);
                ExclamationMark.SetActive(true);
            }
            else if (SkeletonRange.Target == null)
            {
                StunEffect.SetActive(false);
                ExclamationMark.SetActive(false);
                QuestionMark.SetActive(true);
            }
        }
        if (IsDie)
        {
            StunEffect.SetActive(false);
            ExclamationMark.SetActive(false);
            QuestionMark.SetActive(false);
        }

        if (!SpriteRenderer.flipX)
        {
            QuestionMark.transform.localPosition = _originalPosition;
            ExclamationMark.transform.localPosition = _originalPosition;

            ExclamationMark.transform.localRotation = quaternion.Euler(0, 0, _rotation * 2);
            QuestionMark.transform.localRotation = quaternion.Euler(0, 0, _rotation * 2);

            StunEffect.transform.localPosition = new Vector3(-_originalStunPosition.x, _originalStunPosition.y, _originalStunPosition.z);
        }
        else if (SpriteRenderer.flipX)
        {
            QuestionMark.transform.localPosition = new Vector3(-_originalPosition.x, _originalPosition.y, _originalPosition.z);
            ExclamationMark.transform.localPosition = new Vector3(-_originalPosition.x, _originalPosition.y, _originalPosition.z);

            ExclamationMark.transform.localRotation = quaternion.Euler(0, 0, -_rotation * 2);
            QuestionMark.transform.localRotation = quaternion.Euler(0, 0, -_rotation * 2);

            StunEffect.transform.localPosition = _originalStunPosition;

        }
    }
    void Rotation()
    {
        if (!IsHurt && SkeletonRange.Target != null && !IsAttack && !IsStun && !IsDie)
        {
            if (SkeletonRange.Target.transform.position.x <= gameObject.transform.position.x)
            {
                SpriteRenderer.flipX = true;
                AttackHitbox.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (SkeletonRange.Target.transform.position.x > gameObject.transform.position.x)
            {
                SpriteRenderer.flipX = false;
                AttackHitbox.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    void Move()
    {
        if (SkeletonRange.Target != null && !IsAttack && !(Mathf.Abs(SkeletonRange.Target.transform.position.x - transform.position.x) <= 4) &&!IsDie && !IsStun)
        {
            if (SkeletonRange.Target.GetComponent<Transform>().position.x > gameObject.transform.position.x)
            {
                RigidBody2D.linearVelocityX = _speed;
            }
            else if (SkeletonRange.Target.GetComponent<Transform>().position.x < gameObject.transform.position.x)
            {
                RigidBody2D.linearVelocityX = -_speed;
            }
        }
        else
        {
            if (_isAttack2 || IsStun) return;
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
        }
    }

    void Attack()
    {
        if (SkeletonRange.Target != null && Mathf.Abs(SkeletonRange.Target.transform.position.x - transform.position.x) <= 4 && !IsAttack && SkeletonRange.Target.transform.position.y < transform.position.y && !IsDie && !IsStun)
        {
            if (LastAttack >= _attackCooltime)
            {
                IsAttack = true;
                Animator.SetTrigger("attack1");
            }
        }
    }

    void OnAttack2HitboxStart()
    {
        if (SpriteRenderer.flipX)
        {
            RigidBody2D.linearVelocityX = -60f;
        }
        else
        {
            RigidBody2D.linearVelocityX = 60f;
        }
    }

    void OnAttack2HitboxEnd()
    {
        _isAttack2 = false;
        RigidBody2D.linearVelocityX = 0;
    }

    void OnAttack1End()
    {
        Debug.Log("몬스터 공격1 끝");
        if (SkeletonRange.Target.transform.position.x <= gameObject.transform.position.x)
        {
            SpriteRenderer.flipX = true;
            AttackHitbox.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (SkeletonRange.Target.transform.position.x > gameObject.transform.position.x)
        {
            SpriteRenderer.flipX = false;
            AttackHitbox.transform.localScale = new Vector3(1, 1, 1);
        }
        Animator.SetTrigger("attack2");
        _isAttack2 = true;
    }

    void OnAttack2End()
    {
        IsAttack = false;
        LastAttack = 0f;
    }
    void OnDie()
    {
        Destroy(gameObject);
    }

    void OnHurtEnd()
    {
        Debug.Log("데미지 받음");
        IsHurt = false;
    }

    IEnumerator StunDuration()
    {
        yield return TimeManager.s_Wait_3s;
        IsStun = false;
    }

    public void Stun()
    {
        _stunCount++;
        if(_stunCount >= 3)
        {
            RigidBody2D.linearVelocityX = 0;
            _stunCount = 0;
            AttackHitbox.SetActive(false);
            _isAttack2 = false;
            IsStun = true;
            IsAttack = false;
            LastAttack = 0f;
            StartCoroutine(StunDuration());
            Animator.Play("gold_skeleton_hurt", -1, 0);
            if (!PlayerSpriteRenderer.flipX)
            {
                SpriteRenderer.flipX = true;
                RigidBody2D.linearVelocityX = 0;
                RigidBody2D.AddForceX(3, ForceMode2D.Impulse);
            }
            else if (PlayerSpriteRenderer.flipX)
            {
                SpriteRenderer.flipX = false;
                RigidBody2D.linearVelocityX = 0;
                RigidBody2D.AddForceX(-3, ForceMode2D.Impulse);
            }
        }
    }
}
