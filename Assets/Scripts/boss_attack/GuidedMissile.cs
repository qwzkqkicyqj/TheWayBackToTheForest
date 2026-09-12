using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.AI;

public class GuidedMissile : MonoBehaviour
{
    public Rigidbody2D RigidBody2D;
    public bool IsDie = false;

    BossPage1 _bossCode;
    Transform _playerTransform;
            
    Vector2 _dir;
    float _timer;
    float _timerBase = 7;
    float _speed = 9f;
    
    void Start()
    {
        Application.targetFrameRate = 60;
        _bossCode = GameObject.Find("Boss").GetComponent<BossPage1>();
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        StartCoroutine(Wait());
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer <= _timerBase)
        {
            _dir = (_playerTransform.position - gameObject.transform.position).normalized;
        }
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        RigidBody2D.linearVelocity = _dir * _speed;
    }
    IEnumerator Wait()
    {
        yield return TimeManager.s_Wait_0_7s;
    }


    void OnDie()
    {
        _bossCode.isAttack = false;
        IsDie = true;
        Destroy(gameObject);
    }
    
}
