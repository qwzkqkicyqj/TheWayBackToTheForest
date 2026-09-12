using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectFind : MonoBehaviour
{
    static public ObjectFind ObjectFindCode;
    public Player PlayerCode;
    public Rigidbody2D PlayerRigidBody2D;
    public Transform PlayerTransform;
    public BossPage1 BossPage1Code;
    public BossPage2 BossPage2Code;
    public Rigidbody2D BossRigidBody2D;
    public Transform BossTransform;
    public Eyes EyesCode;
    private void Awake()
    {
        ObjectFindCode = this;
    }
}
