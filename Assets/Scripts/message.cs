using System;
using Unity.VisualScripting;
using UnityEngine;

public class Message : MonoBehaviour
{
    public GameObject MessageCanvas;
    public GameObject HelpMessage;
    public GameObject FMessage;
    public Player Player;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            FMessage.SetActive(true);
            if(Player.IsFPress)
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
