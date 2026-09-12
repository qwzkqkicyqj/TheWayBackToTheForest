using UnityEngine;
using System.Collections;

public class EyesAttack : MonoBehaviour
{
    Vector2 _dir;
    Transform _playerTransform;
    public Rigidbody2D RigidBody2D;
    public Animator Animator;
    Player _playerCode;

    float _speed = 30f;
    bool _isDie = false;
    bool _isStart = false;
    void Start()
    {
        _playerTransform = ObjectFind.ObjectFindCode.PlayerTransform;
        _playerCode = ObjectFind.ObjectFindCode.PlayerCode;
    }

    private void OnEnable()
    {
        _isDie = false;
        _isStart = false;
        StartCoroutine(StartWait());
        gameObject.tag = "Damage";
    }

    private void FixedUpdate()
    {
        if(!_isDie && _isStart && !_playerCode.IsStop)
        {
            RigidBody2D.linearVelocity = _dir * _speed;
        }
        else if(_playerCode.IsStop)
        {
            RigidBody2D.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("Player") && !_playerCode.IsDash) || collision.gameObject.layer == 11 || collision.gameObject.layer == 6)
        {
            Animator.SetTrigger("die");
            _isDie = true;
            RigidBody2D.linearVelocity = Vector2.zero;
        }
    }

    void OnDie()
    {
        ObjectFind.ObjectFindCode.EyesCode.EyesAttackPool.Release(gameObject);
    }

    void OnNoDamage()
    {
        gameObject.tag = "NoDamage";
    }

    IEnumerator StartWait()
    {
        yield return TimeManager.s_Wait_0_5s;
        _dir = (_playerTransform.position - gameObject.transform.position).normalized;
        _isStart = true;
    }
}
