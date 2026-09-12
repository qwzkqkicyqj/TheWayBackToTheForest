using UnityEngine;

public class StageStart : MonoBehaviour
{
    public GameObject Barrier;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Barrier.SetActive(true);
        }
    }
}
