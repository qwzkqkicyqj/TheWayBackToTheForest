using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Stage1NextStage : MonoBehaviour
{
    public MainMenuAndStage1Player Player;
    public GameObject QuestionMark;
    public GameObject ExclamationMark;
    public GameObject Message1;
    public GameObject Message2;
    bool isInArea = false;
    public GameObject FireBall;

    void OnTriggerEnter2D()
    {
        if(!isInArea)
        {
            isInArea = true;
            StartCoroutine(cutscene());
        }
       
    }
    IEnumerator cutscene()
    {
        Message1.SetActive(true);
        yield return TimeManager.s_Wait_3s;
        Message1.SetActive(false);
        Message2.SetActive(true);
        yield return TimeManager.s_Wait_3s;
        Message2.SetActive(false);
        QuestionMark.SetActive(true);
        FireBall.SetActive(true);
        FireBall.GetComponent<Rigidbody2D>().linearVelocityX = 45;
    }
}
