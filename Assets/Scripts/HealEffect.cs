using UnityEngine;

public class HealEffect : MonoBehaviour
{
    void OnEnd()
    {
        gameObject.SetActive(false);
    }
}
