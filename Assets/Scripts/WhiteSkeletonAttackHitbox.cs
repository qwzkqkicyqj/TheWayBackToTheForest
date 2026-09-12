using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WhiteSkeletonAttackHitbox : MonoBehaviour
{
    public Animator SkeletonAnimator;
    public WhiteSkeleton SkeletonCode;
    public Rigidbody2D SkeletonRigidBody2D;
    public SpriteRenderer PlayerSpriteRenderer;
    public SpriteRenderer SkeletonSpriteRenderer;
}
