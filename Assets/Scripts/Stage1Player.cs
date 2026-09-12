using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Stage1Player : MonoBehaviour
{
    public Animator Animator;
    public SpriteRenderer SpriteRenderer;

    public bool IsStop = false;
    public bool IsEnd = false;
    public bool IsFPress = false;

    Vector2 _dir;
    int _stop = 0;
    bool _intertact = false;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        Animator.SetBool("idle", gameObject.GetComponent<Rigidbody2D>().linearVelocityX == 0);
        if(IsStop && _stop == 0)
        {
            _stop++;
        }

        if(_dir.x > 0)
        {
            SpriteRenderer.flipX = false;
        }
        else if (_dir.x < 0)
        {
            SpriteRenderer.flipX = true;
        }
    }
    private void FixedUpdate()
    {
        if(_stop == 0)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 5f;
        }
        else if(_stop == 1)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0f;
        }

        if(IsEnd)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 5 * _dir.x;
        }
    }

    void OnMove(InputValue value)
    {
        if(IsEnd)
        {
            _dir = value.Get<Vector2>();
        }
    }

    void OnInteract()
    {
        if(IsEnd)
        {
            if (_intertact)
            {
                Debug.Log("상호작용");
                IsFPress = IsFPress ? false : true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Damage"))
        {
            SceneManager.LoadScene("Stage2Dungeon1");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Interact"))
        {
            _intertact = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Interact")) 
        {
            _intertact = false;
        }
    }
}
