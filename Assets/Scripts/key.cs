using System;
using Unity.VisualScripting;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject FMessage;
    Player _player;
    public AudioClip GetKey;

    void Start()
    {
        Application.targetFrameRate = 60;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            FMessage.SetActive(true);
            if (_player.IsFPress)
            {
                Player.s_IsKey = true;
                SoundManager.Instance.SFXPlay(GetKey);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            _player.IsFPress = false;
            FMessage.SetActive(false);
        }
    }
}
