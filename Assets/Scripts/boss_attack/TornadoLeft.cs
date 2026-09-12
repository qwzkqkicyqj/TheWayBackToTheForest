using NUnit.Framework.Constraints;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using System.Collections;

public class TornadoLeft : MonoBehaviour
{
    float _scale;
    float _speed;
    public Rigidbody2D RigidBody2D;
    public SpriteRenderer SpriteRenderer;
    int _flip = 0;
    bool _isDie = false;
    float _flipCooltime = 0;

    void Start()
    {
        Application.targetFrameRate = 60;
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    void Update()
    {
        _flipCooltime += Time.deltaTime;
        if (_scale <= 34f && !_isDie)
        {
            _scale += Time.deltaTime * 8;
            gameObject.transform.localScale = new Vector3(_scale, _scale, _scale);
        }
        _speed = UnityEngine.Random.Range(15f, 20f);

        if (_isDie && _scale > 0)
        {
            _scale -= Time.deltaTime * 8;
            gameObject.transform.localScale = new Vector3(_scale, _scale, _scale);
            if (_scale <= 0)
            {
                StartCoroutine(Wait());
            }
        }
        gameObject.tag = _scale > 4 ? "EnemyAttack" : "No";
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 && _flipCooltime >= 1)
        {
            Debug.Log("회전중");
            _flipCooltime = 0;
            SpriteRenderer.flipX = SpriteRenderer.flipX ? false : true;
            _flip++;
            if (_flip == 3)
            {
                Debug.Log("사망");
                _isDie = true;
            }
        }
    }
    IEnumerator Wait()
    {
        yield return TimeManager.s_Wait_3s;
        Destroy(gameObject);
    }
}
