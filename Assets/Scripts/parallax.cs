using UnityEngine;
using UnityEngine.SceneManagement;

public class Parallax : MonoBehaviour
{
    Material _material;
    float _distance;
    GameObject _player;
    public float Speed = 0.2f;



    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _material = GetComponent<Renderer>().material;
        SceneManager.sceneLoaded += On_scene_loaded;
    }

    void Update()
    {
        if (_player.GetComponent<Rigidbody2D>().linearVelocityX > 0)
        {
            _distance += Speed * Time.deltaTime;
        }
        else if (_player.GetComponent<Rigidbody2D>().linearVelocityX < 0)
        {
            _distance -= Speed * Time.deltaTime;
        }
        _material.SetTextureOffset("_MainTex", new Vector2(_distance, 0));
    }



    void On_scene_loaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) return;
        _player = GameObject.FindWithTag("Player");
    }
}
