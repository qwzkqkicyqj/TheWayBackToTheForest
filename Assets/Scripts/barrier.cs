using UnityEngine;

public class Barrier : MonoBehaviour
{

    public GameObject BarrierMessage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            BarrierMessage.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            BarrierMessage.SetActive(false);
        }
    }
}
