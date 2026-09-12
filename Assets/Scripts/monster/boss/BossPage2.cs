using UnityEngine;
using System.Collections;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEditor.Tilemaps;

public class BossPage2 : MonoBehaviour, IEnemyStun
{
    public int _stunCount { get; set; }
    public GameObject Attack1Hitbox;
    public GameObject AttackUp;
    public GameObject[] Attack2Effect;
    public GameObject Attack2EffectSet;
    public Transform DashAttackWarningTransform;
    public GameObject Player;
    public GameObject die_cutscene;
    public GameObject key_prefab;
    public GameObject boss_door;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody2D;
    public Animator Animator;
    public bool isAttack = false;
    public bool isHurt = false;
    public bool isDie = false;
    bool _attackUpDir;
    public float Hp = 500;
    float _speed = 10;
    float _attack1Cooltime = 0.5f;
    float _attack2Cooltime = 5;
    float _jumpAttackCooltime = 3;
    float _attackUpCooltiome = 3;
    float _lastAttack1 = 0f;
    float _lastAttack2 = 0f;
    float _lastAttackUp = 0f;
    float _lastJumpAttack = 0f;
    public bool IsStun = false;
    public bool IsHurt = false;
    public bool IsStop = false;
    public SpriteRenderer PlayerSpriteRenderer;
    public GameObject StunEffect;
    public Transform StunEffectTransform;
    public Transform JumpAttackEffectTransform;
    public GameObject JumpAttackEffact;
    Vector3 _stunEffectOriginLocalPosition;
    Vector3 _dashAttackWarningOriginalLocalPosition;
    Vector3 _dashAttackWarningOriginalLocalScale;
    Vector3 _jumpAttackEffectOriginLocalScale;
    Vector3 _jumpAttackEffectOriginLocalPosition;
    float _targetPosition;
    public bool IsJumpAttack = false;
    public float IsStopFirstStop = 0;
    public GameObject DashAttackWarning;
    public Image DashAttackWarningImage;
    public int _dashAttackCount = 0;
    float _dashAttackWarningFade = 0;
    float _jumpAttackEffectOriginalLocalRotation;
    void Start()
    {
        _stunEffectOriginLocalPosition = StunEffectTransform.localPosition;
        _dashAttackWarningOriginalLocalPosition = DashAttackWarningTransform.localPosition;
        _dashAttackWarningOriginalLocalScale = DashAttackWarningTransform.localScale;
        _jumpAttackEffectOriginLocalPosition = JumpAttackEffectTransform.localPosition;
        _jumpAttackEffectOriginLocalScale = JumpAttackEffectTransform.localScale;
        _jumpAttackEffectOriginalLocalRotation = JumpAttackEffectTransform.localRotation.z;
        JumpAttackEffectTransform.localPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        JumpAttackEffectTransform.localPosition = new Vector3(0, 0, 0);

        Animator.SetBool("walk", RigidBody2D.linearVelocityX != 0 && !IsStop);
        _lastAttack1 += Time.deltaTime;
        _lastAttack2 += Time.deltaTime;
        _lastAttackUp += Time.deltaTime;
        _lastJumpAttack += Time.deltaTime;
        Page2Attack();
        Rotation();
        Move();
        if(IsStun)
        {
            StunEffect.SetActive(true);
        }
        else
        {
            StunEffect.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(IsJumpAttack && collision.gameObject.layer == 6)
        {
            IsJumpAttack = false;
            RigidBody2D.linearVelocityX = 0;
            Animator.Play("DashAttackWait");
        }
    }

    void Rotation()
    {
        if (!isHurt && !isAttack && !IsStun && !isHurt && !IsStop)
        {
            if (Player.transform.position.x <= gameObject.transform.position.x)
            {
                SpriteRenderer.flipX = true;
            }
            else if (Player.transform.position.x > gameObject.transform.position.x)
            {
                SpriteRenderer.flipX = false;
            }
        }
        if (SpriteRenderer.flipX)
        {
            Attack1Hitbox.transform.localScale = new Vector3(1, 1, 1);
            Attack2EffectSet.transform.localScale = new Vector3(1, 1, 1);
            StunEffectTransform.localPosition = _stunEffectOriginLocalPosition;
            JumpAttackEffectTransform.localScale = _jumpAttackEffectOriginLocalScale;
            //JumpAttackEffectTransform.localPosition = _jumpAttackEffectOriginLocalPosition; -> animation으로 조절중
            JumpAttackEffectTransform.localRotation = Quaternion.Euler(0, 0, 79.036f);
        }
        else if (!SpriteRenderer.flipX)
        {
            Attack1Hitbox.transform.localScale = new Vector3(-1, 1, 1);
            Attack2EffectSet.transform.localScale = new Vector3(-1, 1, 1);
            StunEffectTransform.localPosition = new Vector3(-_stunEffectOriginLocalPosition.x, _stunEffectOriginLocalPosition.y, _stunEffectOriginLocalPosition.z);
            JumpAttackEffectTransform.localScale = new Vector3(-_jumpAttackEffectOriginLocalScale.x, _jumpAttackEffectOriginLocalScale.y, _jumpAttackEffectOriginLocalScale.z);
            //JumpAttackEffectTransform.localPosition = new Vector3(-_jumpAttackEffectOriginLocalPosition.x, _jumpAttackEffectOriginLocalPosition.y, _jumpAttackEffectOriginLocalPosition.z);
            JumpAttackEffectTransform.localRotation = Quaternion.Euler(0, 0, -79.036f);
        }
    }
    void Move()
    {
        if (!isAttack && !(Mathf.Abs(Player.transform.position.x - transform.position.x) <= 2) && !isHurt && !isDie && !IsStun && !IsStop)
        {
            if (Player.transform.position.x > gameObject.transform.position.x)
            {
                RigidBody2D.linearVelocityX = _speed;
            }
            else if (Player.transform.position.x < gameObject.transform.position.x)
            {
                RigidBody2D.linearVelocityX = -_speed;
            }
        }
        else if (IsStop && IsStopFirstStop == 0)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
            IsStopFirstStop = 1;
        }
        else if(!IsStop)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
        }
    }
    void Page2Attack()
    {
        if (IsStun || IsStop) return;
        if (Mathf.Abs(Player.transform.position.x - transform.position.x) <= 2 && !isAttack && Player.transform.position.y < transform.position.y && !isDie && !isAttack)
        {
            if (_lastAttack1 >= _attack1Cooltime)
            {
                isAttack = true;
                Animator.SetTrigger("attack1");
            }
        }
        else if (Mathf.Abs(Player.transform.position.x - transform.position.x) > 2 && Mathf.Abs(Player.transform.position.x - transform.position.x) < 15 && !isAttack && Player.transform.position.y < transform.position.y && !isDie && !isAttack)
        {
            if (_lastJumpAttack >= _jumpAttackCooltime)
            {
                Animator.Play("JumpAttackWait");
            }
        }
        else if (Mathf.Abs(Player.transform.position.x - transform.position.x) > 2 && !isAttack && Player.transform.position.y < transform.position.y && !isDie && !isAttack)
        {
            if (_lastAttack2 >= _attack2Cooltime)
            {
                Rotation();
                isAttack = true;
                Animator.SetTrigger("attack2");
            }
        }
        else if (Mathf.Abs(Player.transform.position.x - transform.position.x) <= 2 && !isAttack && Player.transform.position.y > transform.position.y && !isDie && !isAttack)
        {
            if (_lastAttackUp >= _attackUpCooltiome)
            {
                if (SpriteRenderer.flipX)
                {
                    isAttack = true;
                    Animator.SetTrigger("attack_up_right");
                    _attackUpDir = true;
                }
                else
                {
                    isAttack = true;
                    Animator.SetTrigger("attack_up_left");
                    _attackUpDir = false;
                }
            }
        }
    }

    
    void OnJumpAttackStart()
    {
        IsStop = true;
        IsStopFirstStop = 0;
        IsJumpAttack = true;
    }

    void OnJump()
    {
        _targetPosition = Player.transform.position.x;
        if (Player.transform.position.x <= gameObject.transform.position.x)
        {
            SpriteRenderer.flipX = true;
            _targetPosition = -Mathf.Abs(transform.position.x - _targetPosition);
            RigidBody2D.linearVelocityX = _targetPosition / 1.35f;
        }
        else
        {
            SpriteRenderer.flipX = false;
            _targetPosition = Mathf.Abs(transform.position.x - _targetPosition);
            RigidBody2D.linearVelocityX = _targetPosition / 1.35f;
        }
        RigidBody2D.linearVelocityY = 27;
    }

    void OnAttackEffectStart()
    {
        if(SpriteRenderer.flipX)
        {
            Animator.Play("JumpAttack");
        }
        else
        {
            Animator.Play("JumpAttackRight");

        }
    }

    void OnFallStart()
    {
        RigidBody2D.linearVelocityY = -15;
    }

    void OnDashattackStart()
    {
        if (Player.transform.position.x <= gameObject.transform.position.x)
        {
            SpriteRenderer.flipX = true;
            DashAttackWarningTransform.localPosition = _dashAttackWarningOriginalLocalPosition;
            DashAttackWarningTransform.localScale = _dashAttackWarningOriginalLocalScale;
        }
        else
        {
            SpriteRenderer.flipX = false;
            DashAttackWarningTransform.localPosition = new Vector3(-_dashAttackWarningOriginalLocalPosition.x, _dashAttackWarningOriginalLocalPosition.y, _dashAttackWarningOriginalLocalPosition.z);
            DashAttackWarningTransform.localScale = new Vector3(-_dashAttackWarningOriginalLocalScale.x, _dashAttackWarningOriginalLocalScale.y, _dashAttackWarningOriginalLocalScale.z);
        }
        RigidBody2D.linearVelocityX = 0;
        DashAttackWarning.SetActive(true);
        StartCoroutine(DashAttackWarningAnimaton());
    }

    void OnAttackUpHitboxStart()
    {
        AttackUp.GetComponent<SpriteRenderer>().flipX = SpriteRenderer.flipX;
    }
    void OnAttackUpHitboxEnd()
    {
        _lastAttackUp = 0;
        isAttack = false;
    }
    void OnAttack1HitboxStart()
    {
        isAttack = true;
    }
    void OnAttack1HitboxEnd()
    {
        _lastAttack1 = 0;
        isAttack = false;
    }
    void OnAttack2End()
    {
        _lastAttack2 = 0;
        isAttack = false;
    }

    void OnDashAttackStart()
    {
        StartCoroutine(DashAttackStart());
    }

    void OnDashAttackEnd()
    {
        if (_dashAttackCount >= 3)
        {
            _dashAttackCount = 0;
            IsStop = false;
            isAttack = false;
            Animator.Play("boss_idle");
            _lastJumpAttack = 0;
            Rotation();
        }
        else
        {
            Animator.Play("DashAttackWait");
        }
    }

    void OnDie()
    {
        die_cutscene.SetActive(true);
        boss_door.SetActive(true);
        isDie = true;
        GameObject key = Instantiate(key_prefab, gameObject.transform.position, Quaternion.identity);
        key.GetComponent<Rigidbody2D>().linearVelocityY = 13;
        gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
        Destroy(gameObject);
    }

    IEnumerator StunDuration()
    {
        yield return TimeManager.s_Wait_3s;
        IsHurt = false;
        IsStop = false;
        IsStun = false;
    }

    public void Stun()
    {
        _stunCount++;
        if (_stunCount >= 5)
        {
            IsJumpAttack = false;
            RigidBody2D.linearVelocityX = 0;
            _stunCount = 0;
            IsStun = true;
            isAttack = false;
            _lastAttack1 = 0f;
            StartCoroutine(StunDuration());
            Animator.Play("boss_hurt", -1, 0);
            if (!PlayerSpriteRenderer.flipX)
            {
                SpriteRenderer.flipX = true;
                RigidBody2D.AddForceX(3, ForceMode2D.Impulse);
            }
            else if (PlayerSpriteRenderer.flipX)
            {
                SpriteRenderer.flipX = false;
                RigidBody2D.AddForceX(-3, ForceMode2D.Impulse);
            }
        }
    }
    IEnumerator DashAttackWarningAnimaton()
    {
        _dashAttackWarningFade = 0;
        DashAttackWarningImage.fillOrigin = (int)Image.OriginHorizontal.Right;
        while (_dashAttackWarningFade <= 0.2f)
        {
            _dashAttackWarningFade += Time.deltaTime;
            DashAttackWarningImage.fillAmount = _dashAttackWarningFade/0.2f;
            yield return null;
        }
        _dashAttackWarningFade = 0;
        DashAttackWarningImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        while (_dashAttackWarningFade <= 0.2f)
        {
            _dashAttackWarningFade += Time.deltaTime;
            DashAttackWarningImage.fillAmount = 1 - _dashAttackWarningFade / 0.2f;
            yield return null;
        }
        DashAttackWarning.SetActive(false);
        Animator.Play("DashAttack");
    }
    IEnumerator DashAttackStart()
    {
        if (SpriteRenderer.flipX)
        {
            RigidBody2D.linearVelocityX = -50;
        }
        else
        {
            RigidBody2D.linearVelocityX = 50;
        }
        yield return TimeManager.s_Wait_0_3s;
        RigidBody2D.linearVelocityX = 0;
        _dashAttackCount++;
    }
}
