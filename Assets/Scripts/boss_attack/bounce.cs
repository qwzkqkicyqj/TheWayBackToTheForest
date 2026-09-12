using Unity.Mathematics;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public GameObject BounceLeftBounce;
    public GameObject BounceLeftBounce2;
    public GameObject BounceRightBounce;
    public GameObject BounceRightBounce2;
    public Transform[] Point;
    Transform _playerTrasform;
    BossPage1 _bossCode;

    private void Awake()
    {
        _bossCode = GameObject.Find("Boss").GetComponent<BossPage1>();
    }

    void Start()
    {
        _playerTrasform = GameObject.Find("Player").gameObject.GetComponent<Transform>();
        if(!_bossCode.isDoubleAttackStart)
        {
            if (math.abs(_playerTrasform.position.x - Point[0].position.x) < math.abs(_playerTrasform.position.x - Point[1].position.x))
            {
                BounceLeftBounce.SetActive(true);
                BounceLeftBounce2.SetActive(true);
            }
            else
            {
                BounceRightBounce.SetActive(true);
                BounceRightBounce2.SetActive(true);
            }
        }
        else
        {
            BounceLeftBounce.SetActive(true);
            BounceLeftBounce2.SetActive(true);
            BounceRightBounce.SetActive(true);
            BounceRightBounce2.SetActive(true);
        }
    }


}
