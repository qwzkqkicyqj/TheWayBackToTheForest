using Unity.Mathematics;
using UnityEngine;
using System.Collections;

public class Tornado : MonoBehaviour
{
    public Rigidbody2D RigidBody2D;
    public SpriteRenderer SpriteRenderer;
    public Transform[] Point;
    Transform _playerTransform;
    BossPage1 _bossCode;

    bool _isDie = false;

    int _flip = 0;
    float _flipCooltime;
    float _speed;
    float _scale;

    void Start()
    {
        Point[0].SetParent(null);
        Point[1].SetParent(null);
        Application.targetFrameRate = 60;
        _bossCode = GameObject.Find("Boss").GetComponent<BossPage1>();


        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        if(!_bossCode.isDoubleAttackStart)
        {
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            if (math.abs(_playerTransform.position.x - Point[0].position.x) < math.abs(_playerTransform.position.x - Point[1].position.x))
            {
                gameObject.transform.position = Point[0].position;
            }
            else
            {
                gameObject.transform.position = Point[1].position;
                SpriteRenderer.flipX = true;
            }
        }
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
        if(collision.gameObject.layer == 11 && _flipCooltime >= 1)
        {
            Debug.Log("회전중");
            _flipCooltime = 0;
            SpriteRenderer.flipX = SpriteRenderer.flipX ? false : true;
            _flip++;
            if(_flip == 3)
            {
                Debug.Log("사망");
                _isDie = true;
            }
        }
    }
    IEnumerator Wait()  
    {
        yield return TimeManager.s_Wait_3s;
        _bossCode.isAttack = false;
        Destroy(gameObject);
        Debug.Log("오브젝트 제거");
    }
}
