using UnityEngine;
using System.Collections;

public class BounceLeft : MonoBehaviour
{
    public Rigidbody2D RigidBody2D;
    public SpriteRenderer SpriteRenderer;
    BossPage1 _bossCode;

    int _flip = 0;

    float _speed = -10;
    bool _isStart = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        _bossCode = ObjectFind.ObjectFindCode.BossPage1Code;
        SpriteRenderer.flipX = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!SpriteRenderer.flipX)
        {
            RigidBody2D.linearVelocityX = _speed;
        }
        else
        {
            RigidBody2D.linearVelocityX = -_speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11 && _isStart)
        {
            if (_flip == 2)
            {
                gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                StartCoroutine(Die());
                StartCoroutine(Wait());
                return;
            }
            SpriteRenderer.flipX = SpriteRenderer.flipX ? false : true;
            _flip++;
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isStart && collision.gameObject.layer == 11)
        {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            RigidBody2D.linearVelocityX = -5;
            _isStart = true;
        }
    }

    IEnumerator Die()
    {
        yield return TimeManager.s_Wait_5s;
        Destroy(gameObject);
    }
    IEnumerator Wait()
    {
        yield return TimeManager.s_Wait_3s;
        _bossCode.isAttack = false;
    }
}
