using Unity.Mathematics;
using UnityEngine;

public class GuidedMissileSet : MonoBehaviour
{
    public GameObject GuidedMissileLeft;
    public GameObject GuidedMissileRight;
    public GuidedMissile GuidedMissileLeftCode;
    public GuidedMissile GuidedMissileRightCode;
    public Transform[] Point;
    Transform _playerTransform;
    BossPage1 bossCode;
    void Start()
    {
        bossCode = GameObject.Find("Boss").GetComponent<BossPage1>();
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        if(!bossCode.isDoubleAttackStart)
        {
            if (math.abs(_playerTransform.position.x - Point[0].position.x) < math.abs(_playerTransform.position.x - Point[1].position.x))
            {
                GuidedMissileLeft.SetActive(true);
            }
            else
            {
                GuidedMissileRight.SetActive(true);
            }
        }
        else
        {
            GuidedMissileLeft.SetActive(true);
            GuidedMissileRight.SetActive(true);
        }
    }

    void Update()
    {
        if(GuidedMissileLeftCode.IsDie || GuidedMissileRightCode.IsDie)
        {
            Destroy(gameObject);
        }
    }
}
