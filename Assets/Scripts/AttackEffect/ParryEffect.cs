using UnityEngine;

public class ParryEffect : MonoBehaviour
{
    void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
