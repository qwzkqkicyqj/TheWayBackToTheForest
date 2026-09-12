using UnityEngine;

public class Boom : MonoBehaviour
{
    Transform _playerTransform;

    void Start()
    {
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        gameObject.transform.position = _playerTransform.position;
    }

    void OnDie()
    {
        Destroy(gameObject);
    }
}
