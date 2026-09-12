using UnityEngine;
using UnityEngine.SceneManagement;

public class ParallaxControl : MonoBehaviour
{
    GameObject _player;
    public static bool OnlyOne = false;
    private void Awake()
    {
        if(OnlyOne)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(_player.transform.position.x, gameObject.transform.position.y, 0);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            OnlyOne = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
            return;
        }
        _player = GameObject.FindWithTag("Player");
    }
}
