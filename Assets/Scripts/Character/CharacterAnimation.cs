
using UnityEngine;
using R3;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour, ISetTransform
{
    [SerializeField]
    private float _walkDamp;

    [SerializeField]
    private InterruptionAnimationInformation _interruptionAnimationInfo;
    [SerializeField]
    private AttackAnimationInformation _attackAnimationInfo;
    [SerializeField]
    private InsertAnimationSystem _insertAnimationSystem;

    private Transform _characterTransform;
    private Animator _animator;

    public InterruptionAnimationInformation InterruptionAnimationInfo { get => _interruptionAnimationInfo; }
    public AttackAnimationInformation AttackAnimationInfo { get => _attackAnimationInfo; }

    public ReactiveProperty<bool> ReactivePropertyIsAnimation { get => _insertAnimationSystem.ReactivePropertyIsAnimation; }
    public bool IsAnimation { get; private set; }

    private void Awake()
    {
        _animator = this.CheckComponentMissing<Animator>();
    }
   
   public void DoWalkAnimation(Vector3 moveDirection)
    {
        Vector2 changeInput = GetDirectionToAnimationValue(moveDirection);
        _animator.SetFloat(AnimationStringUtility.MoveInputXName, changeInput.x, _walkDamp, Time.deltaTime);
        _animator.SetFloat(AnimationStringUtility.MoveInputYName, changeInput.y, _walkDamp, Time.deltaTime);
    }

    public void DoAnimation(MatchTargetAnimationData animationData)
    {
        _insertAnimationSystem.AnimationPlay(animationData).Forget();
    }
    public void SetAnimationBool(string AnimName)
    {
        AnimationReset(AnimName);
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    private Vector2 GetDirectionToAnimationValue(Vector3 moveDirection)
    {
        MyExtensionClass.CheckArgumentNull(moveDirection, nameof(moveDirection));

        float inputX = Vector3.Dot(moveDirection, _characterTransform.right);
        float inputY = Vector3.Dot(moveDirection, _characterTransform.forward);

        return new Vector2(inputX, inputY);
    }

    private void AnimationReset(string DoAnim)
    {
      

        foreach (AnimatorControllerParameter anim in _animator.parameters)
        {
            if (anim.type  != AnimatorControllerParameterType.Bool)
            {
                continue;
            }
            if (anim.name == DoAnim)
            {              
                _animator.SetBool(anim.name, true);
            }
            else
            {
                _animator.SetBool(anim.name, false);
            }
        }
    }
}


