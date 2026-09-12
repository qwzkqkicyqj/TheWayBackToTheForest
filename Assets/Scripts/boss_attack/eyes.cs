using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.Pool;
public class Eyes : MonoBehaviour
{
    public BossPage1 BossCode;
    public GameObject EyesAttackPrefab;
    public Animator Animator;
    public ObjectPool<GameObject> EyesAttackPool;
    GameObject _eyesAttack;

    private void Awake()
    {
        EyesAttackPool = new ObjectPool<GameObject>(CreateEyesAttack, OnGetEyesAttack, OnReleaseEyesAttack, OnDestroyEyesAttack, true, 5, 10);
    }
    void Update()
    {
        if((BossCode.HpHalf && !BossCode.isAttack && !BossCode.isDoubleAttackStart) || BossCode.is1PageEnd)
        {
            Animator.speed = 0;
        }
        else
        {
            Animator.speed = 1;
        }
        Animator.SetBool("start", BossCode.EyesStart);
        Animator.SetBool("double", BossCode.isDoubleAttackStart);
    }

    void OnAttack()
    {
        _eyesAttack = EyesAttackPool.Get();
        _eyesAttack.transform.position = gameObject.transform.position;
    }
    GameObject CreateEyesAttack()
    {
        return Instantiate(EyesAttackPrefab);
    }
    void OnGetEyesAttack(GameObject eyesAttack)
    {
        eyesAttack.SetActive(true);
    }
    private void OnReleaseEyesAttack(GameObject eyesAttack)
    {
        eyesAttack.SetActive(false);
    }
    private void OnDestroyEyesAttack(GameObject eyesAttack)
    {
        Destroy(eyesAttack);
    }
}
