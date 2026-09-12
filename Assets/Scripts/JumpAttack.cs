using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public Animator Animator;
    void OnWait()
    {
        Animator.Play("JumpAttackEffectIdle");
    }
}


