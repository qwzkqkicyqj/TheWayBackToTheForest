using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
    public GameObject NoKey;
    public GameObject FMessage;
    public Player Player;
    public Rigidbody2D PlayerRigidBody2D;
    public GameObject LockDoor;
    public GameObject UnlockDoor;
    public Image Gauge;
    bool _isOpenSuccess = false;
    float _fHoldTimer = 0;
    float _fHoldSet = 3;
    public AudioClip OpenDoorSound;

    void Start()
    {
        Application.targetFrameRate = 60;
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (Player.s_IsKey && !_isOpenSuccess)
            {
                FMessage.SetActive(true);
                if (Player.IsFHold && !Player.IsDie && PlayerRigidBody2D.linearVelocityY == 0)
                {
                    if(_fHoldTimer == 0)
                    {
                        SoundManager.Instance.UnlockSoundPlay();
                    }
                    _fHoldTimer += Time.deltaTime;
                }
                else
                {
                    _fHoldTimer = 0;
                    SoundManager.Instance.UnlockSoundStop();
                }
                Gauge.fillAmount = _fHoldTimer / _fHoldSet;
                if (_fHoldTimer >= _fHoldSet)
                {
                    _isOpenSuccess = true ;
                    SoundManager.Instance.SFXPlay(OpenDoorSound);
                    FMessage.SetActive(false);
                    LockDoor.SetActive(false);
                    UnlockDoor.SetActive(true);
                    Player.s_IsKey = false;
                    Player.IsStop = false;
                    Destroy(gameObject);
                }
            }
            else if(!Player.s_IsKey)
            {
                NoKey.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            Player.IsFPress = false;
            NoKey.SetActive(false);
            FMessage.SetActive(false);
        }
    }
}
