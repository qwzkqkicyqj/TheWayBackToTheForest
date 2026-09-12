using UnityEngine;

public class WhiteSkeletonRange : MonoBehaviour
{
    public GameObject Target;
    public Transform SkeletonObjectTransform;
    public LayerMask Ground;
    public float AirCheckRadius = 0.5f;
    public GameObject LeftAir;
    public GameObject RightAir;
    public Rigidbody2D SkeletonObjectRigidBody2D;
    public Player PlayerCode;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.transform.position.y >= SkeletonObjectTransform.transform.position.y - 0.6f && !PlayerCode.IsDie)
            { 
                if(collision.gameObject.transform.position.x < SkeletonObjectTransform.transform.position.x)
                {
                    if(Physics2D.OverlapCircle(LeftAir.transform.position, AirCheckRadius, Ground))
                    {
                        Target = collision.gameObject;
                    }
                    else
                    {
                        Target = null;
                        SkeletonObjectRigidBody2D.linearVelocityX = 0;
                    }
                }
                else if(collision.gameObject.transform.position.x > SkeletonObjectTransform.transform.position.x)
                {
                    if(Physics2D.OverlapCircle(RightAir.transform.position, AirCheckRadius, Ground))
                    {
                        Target = collision.gameObject;
                    }
                    else
                    {
                        Target = null;
                        SkeletonObjectRigidBody2D.linearVelocityX = 0;
                    }
                }
            }
            else
            {
                Target = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Target = null;
        }
    }
    void OnDrawGizmos()
    {
        if (LeftAir != null)
        {
            bool hit = Physics2D.OverlapCircle(
                LeftAir.transform.position,
                AirCheckRadius,
                Ground);

            Gizmos.color = hit ? Color.green : Color.red;
            Gizmos.DrawWireSphere(LeftAir.transform.position, AirCheckRadius);
        }

        if (RightAir != null)
        {
            bool hit = Physics2D.OverlapCircle(
                RightAir.transform.position,
                AirCheckRadius,
                Ground);

            Gizmos.color = hit ? Color.green : Color.red;
            Gizmos.DrawWireSphere(RightAir.transform.position, AirCheckRadius);
        }

    }
}
