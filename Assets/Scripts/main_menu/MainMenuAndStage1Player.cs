using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuAndStage1Player : MonoBehaviour
{
    public GameObject CutScene;
    public GameObject MainMenu;
    public GameObject[] Background;
    public GameObject BackgroundMain;
    public MainMenu MainMenuCode;
    public Animator Animator;
    public SpriteRenderer SpriteRenderer;

    public bool IsStop = false;
    public bool IsEnd = false;
    public bool IsFPress = false;
    bool _intertact = false;

    int _stop = 0;
    Vector2 _dir;
    private void Update()
    {
        //멈춰있다면 idle실행
        Animator.SetBool("idle", gameObject.GetComponent<Rigidbody2D>().linearVelocityX == 0);

        // -> 이를 통해 게임 시작 전 플레이어가 같은 스테이지에서 반복적으로 이동하도록 제작
        //플레이어가 특정 위치(가장 오른쪽 지역)에 도달하고, 제목이 사라지지 않았다면(아직 새 게임이 시작되지 않았다면)
        if (transform.position.x >= Background[2].GetComponent<Transform>().position.x && !MainMenuCode.TitleFadeEnd)
        {
            //가장 왼쪽에 있는 지역으로 이동
            transform.position = new Vector3(Background[0].GetComponent<Transform>().position.x, transform.position.y, transform.position.z);
            //플레이어 위치로 배경 이동(플레이어가 이동된 후 다음 프레임에 배경이 이동되는것을 막기 위해)
            BackgroundMain.transform.position = new Vector3(transform.position.x, BackgroundMain.transform.position.y, BackgroundMain.transform.position.z);
        }
        
        // -> 새 게임이 시작된 후 플레이어가 다음 스테이지로 이동하도록 제작
        //플레이어가 특정 위치에 도달하고, 제목이 사라진다면(새 게임이 시작된다면)
        if ((Mathf.Abs(transform.position.x - Background[0].transform.position.x) < 0.1f || Mathf.Abs(transform.position.x - Background[1].transform.position.x) < 0.1f || Mathf.Abs(transform.position.x - Background[2].transform.position.x) < 0.1f) && MainMenuCode.TitleFadeEnd)
        {
            //숲 스테이지로 이동
            transform.position = new Vector3(120.716f, -2.969164f, transform.position.z); 
            //플레이어 위치로 배경 이동(플레이어가 이동된 후 다음 프레임에 배경이 이동되는것을 막기 위해)
            BackgroundMain.transform.position = new Vector3(transform.position.x, BackgroundMain.transform.position.y, BackgroundMain.transform.position.z);
            //일시정지 매니저 오브젝트 활성화
            PauseManager.Instance.gameObject.SetActive(true);
            //사용하지 않는 메인 메뉴 오브젝트 비활성화
            MainMenu.SetActive(false); 
        }
        

        // 이동 방향이 오른쪽이라면
        if (_dir.x > 0)
        {
            SpriteRenderer.flipX = false;
        }
        else if (_dir.x < 0)
        {
            SpriteRenderer.flipX = true;
        }

        if (IsStop && _stop == 0)
        {
            _stop++;
        }

    }

    void FixedUpdate()
    {
        if (_stop == 0)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 5f;
        }
        else if (_stop == 1)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0f;
        }

        if (IsEnd)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 5 * _dir.x;
        }
    }
    void OnMove(InputValue value)
    {
        if (IsEnd)
        {
            _dir = value.Get<Vector2>();
        }
    }

    void OnInteract()
    {
        if (IsEnd)
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
