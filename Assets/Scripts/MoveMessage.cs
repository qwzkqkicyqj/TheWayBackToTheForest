using System;
using Unity.VisualScripting;
using UnityEngine;

public class MoveMessage : MonoBehaviour
{
    public GameObject MessageCanvas;
    public GameObject HelpMessage;
    public GameObject FMessage;
    public MainMenuAndStage1Player Player;
    public StartCutscene1 StageCutscene1;

    void Start()
    {
        Application.targetFrameRate = 60;
    }



    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if(Player.IsEnd)
            {
                FMessage.SetActive(true);
            }
            if (Player.IsFPress && Player.IsEnd)
            {
                MessageCanvas.SetActive(true);
                HelpMessage.SetActive(true);
            }
            else
            {
                MessageCanvas.SetActive(false);
                HelpMessage.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player.IsFPress = false;
            FMessage.SetActive(false);
            MessageCanvas.SetActive(false);
            HelpMessage.SetActive(false);
        }
    }
}
