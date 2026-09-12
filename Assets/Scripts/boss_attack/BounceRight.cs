using NUnit.Framework.Constraints;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using System.Collections;

public class BounceRight : MonoBehaviour
{
    public Rigidbody2D RigidBody2D;
    public SpriteRenderer SpriteRenderer;
    bool _start = false;
    float _speed = -10;
    int _flip = 0;
    BossPage1 _bossCode;

    void Start()
    {
        Application.targetFrameRate = 60;
        _bossCode = ObjectFind.ObjectFindCode.BossPage1Code;
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
        if (collision.gameObject.layer == 11 && _start)
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
        if (!_start && collision.gameObject.layer == 11)
        {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            RigidBody2D.linearVelocityX = -5;
            _start = true;
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
