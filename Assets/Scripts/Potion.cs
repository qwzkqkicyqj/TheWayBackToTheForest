using System;
using Unity.VisualScripting;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public AudioClip PotionGetSound;
    public GameObject FMessage;
    Player _player;


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
                SoundManager.Instance.SFXPlay(PotionGetSound);
                Player.s_PotionCount ++;
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
