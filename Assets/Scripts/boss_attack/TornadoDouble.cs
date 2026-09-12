using UnityEngine;

public class TornadoDouble : MonoBehaviour
{
    public Transform LeftTornado;
    public Transform RightTornado;
    public SpriteRenderer RightTornadoSpriteRenderer;
    public Transform[] Point;
    BossPage1 bossCode;
    void Start()
    {
        bossCode = GameObject.Find("Boss").GetComponent<BossPage1>();
        LeftTornado.position = Point[0].position;
        RightTornado.position = Point[1].position;
        RightTornadoSpriteRenderer.flipX = true;
    }

    void Update()
    {
        if(!bossCode.isAttack)
        {
            Destroy(gameObject);
        }
    }
}
