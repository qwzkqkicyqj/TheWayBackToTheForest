using UnityEngine;

public class Stage1Message2 : MonoBehaviour
{
    public GameObject QuestionMark;
    public GameObject ExclamationMark;
    public GameObject Message;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            QuestionMark.SetActive(true);
            Message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Message.SetActive(false);
            Destroy(gameObject);
        }
    }
}
