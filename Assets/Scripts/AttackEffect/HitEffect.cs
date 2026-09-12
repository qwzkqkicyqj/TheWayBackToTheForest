using UnityEngine;

public class HitEffect : MonoBehaviour
{
    void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
