using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BossDieCutscene : MonoBehaviour
{
    public GameObject Message;
    public CinemachineCamera PlayerCinemachineCamera;
    public Player PlayerCode;
    public Rigidbody2D PlayerRigidBody2D;
    void Start()
    {
        StartCoroutine(Cutscene());
    }

    
    IEnumerator Cutscene()
    {
        PlayerCode.IsStop = true;
        PlayerRigidBody2D.linearVelocityX = 0;
        yield return TimeManager.s_Wait_0_3s;
        Message.SetActive(true);
        yield return TimeManager.s_Wait_2s;
        Message.SetActive(false);
        PlayerCinemachineCamera.Priority = 10;
        PlayerCode.IsStop = false;
        Destroy(gameObject);
    }
}
