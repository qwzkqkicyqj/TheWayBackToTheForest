using UnityEngine;

public class Meteor : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody2D RigidBody2D;
    Player _player;

    void Start()
    {
        _player = ObjectFind.ObjectFindCode.PlayerCode;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") && !_player.IsDash) || collision.gameObject.layer == 6)
        {
            RigidBody2D.gravityScale = 0;
            RigidBody2D.linearVelocityY = 0;
            Animator.SetTrigger("die");
        }
    }

    void OnNoDamage()
    {
        gameObject.tag = "NoDamage";
    }

    void OnDie()
    {
        ObjectFind.ObjectFindCode.BossPage1Code.MeteorPool.Release(this.gameObject);
    }
}
