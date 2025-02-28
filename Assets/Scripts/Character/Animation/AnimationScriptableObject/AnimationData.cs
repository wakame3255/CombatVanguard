using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "Animation/AnimationData")]
public class AnimationData : ScriptableObject
{
    [SerializeField]
    private AttackAnimationInformation attackAnimation;

    [SerializeField]
    private InterruptionAnimationInformation interruptionAnimation;

    

    public AttackAnimationInformation AttackAnimation { get => attackAnimation; }
    public InterruptionAnimationInformation InterruptionAnimation { get => interruptionAnimation; }
}