using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class WhiteSkeleton : MonoBehaviour, IEnemyStun
{
    public int _stunCount { get; set; }
    float _dir;
    public Rigidbody2D RigidBody2D;
    public SpriteRenderer SpriteRenderer;
    public WhiteSkeletonRange SkeletonRange;
    public Animator Animator;
    public GameObject AttackHitbox;
    public float Hp = 100;
    float _speed = 4f;
    public bool IsAttack;
    float attackCooltime = 0.5f;
    public float LastAttack;
    public bool IsHurt = false;
    public bool IsDie;
    public bool IsStun = false;
    public GameObject StunEffect;
    public SpriteRenderer PlayerSpriteRenderer;
    public AudioSource SkeletonAttackAudioSource;


    //이모션 변수
    public GameObject QuestionMark;
    public GameObject ExclamationMark;
    Vector3 _originalEmotionPosition;
    Vector3 _originalStunPosition;
    float _rotation;

    void Start()
    {
        Animator = GetComponent<Animator>();
        float rotation = QuestionMark.transform.localRotation.x;
        float position = QuestionMark.transform.localPosition.x;
        _originalEmotionPosition = QuestionMark.transform.localPosition;
        _rotation = QuestionMark.transform.localRotation.z;
        _originalStunPosition = StunEffect.transform.localPosition;
    }

    void Update()
    {
        if(!IsDie)
        {
            LastAttack += Time.deltaTime;
            Rotation();
            Animator.SetBool("run", math.abs(RigidBody2D.linearVelocityX) > 0.0001f && !IsStun);
            Attack();
        }
        Status();
    }
    private void FixedUpdate()
    {
        Move();
    }

    void Status()
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
            QuestionMark.transform.localPosition = _originalEmotionPosition;
            ExclamationMark.transform.localPosition = _originalEmotionPosition;

            ExclamationMark.transform.localRotation = quaternion.Euler(0, 0, _rotation * 2);
            QuestionMark.transform.localRotation = quaternion.Euler(0, 0, _rotation * 2);

            StunEffect.transform.localPosition = new Vector3(-_originalStunPosition.x, _originalStunPosition.y, _originalStunPosition.z);
        }
        else if (SpriteRenderer.flipX)
        {
            QuestionMark.transform.localPosition = new Vector3(-_originalEmotionPosition.x, _originalEmotionPosition.y, _originalEmotionPosition.z);
            ExclamationMark.transform.localPosition = new Vector3(-_originalEmotionPosition.x, _originalEmotionPosition.y, _originalEmotionPosition.z);

            ExclamationMark.transform.localRotation = quaternion.Euler(0, 0, -_rotation * 2);
            QuestionMark.transform.localRotation = quaternion.Euler(0, 0, -_rotation * 2);

            StunEffect.transform.localPosition = _originalStunPosition;
        }
    }
    void Rotation()
    {
        if (!IsHurt && SkeletonRange.Target != null && !IsAttack && !IsStun)
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
        if (SkeletonRange.Target != null && !IsAttack && !(Mathf.Abs(SkeletonRange.Target.transform.position.x - transform.position.x) <= 2) && !IsHurt && !IsStun && !IsDie)
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
        if(IsDie || SkeletonRange.Target == null)
        {
            RigidBody2D.linearVelocityX = 0;
        }
    }
    
    void Attack()
    {
        if (SkeletonRange.Target != null &&  Mathf.Abs(SkeletonRange.Target.transform.position.x - transform.position.x) <= 2 && !IsAttack && SkeletonRange.Target.transform.position.y<transform.position.y && !IsDie && !IsStun && !IsHurt)
        {
            if (LastAttack >= attackCooltime)
            {
                IsAttack = true;
                Animator.SetTrigger("attack1");
            }
        }
    }
    void OnAttakcSoundPlay()
    {
        SkeletonAttackAudioSource.Play();
    }
    void OnAttackSoundStop()
    {
        SkeletonAttackAudioSource.Stop();
    }
    void OnAttackEnd()
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
        IsHurt = false;
    }
    IEnumerator StunDuration()
    {
        yield return TimeManager.s_Wait_3s;
        IsStun = false;
    }

    public void Stun()
    {
        IsStun = true;
        IsAttack = false;
        LastAttack = 0f;
        StartCoroutine(StunDuration());
        Animator.SetTrigger("hurt");
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
