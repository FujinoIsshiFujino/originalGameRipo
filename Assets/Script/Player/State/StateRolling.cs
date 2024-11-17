using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerControl
{
    [SerializeField] private float rollingCoolDown = 0.05f;
    [SerializeField] private float rollingSpeed = 2.75f;

    public class StateRolling : PlayerStateBase
    {
        Vector3 rollDirection;
        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {
            rollDirection = (owner.cameraForward * owner.inputVertical + owner.Camera.transform.right * owner.inputHorizontal).normalized;
            owner.transform.forward = rollDirection;
            owner._animator.SetBool("Roll", true);
        }

        public override void OnUpdate(PlayerControl owner)
        {
            owner.moveDirection = rollDirection + new Vector3(0, owner.moveDirection.y, 0);
            owner.characterController.Move(owner.moveDirection * owner.rollingSpeed * Time.deltaTime * owner.moveSpeed);
        }

        public override void OnExit(PlayerControl owner, PlayerStateBase nextState)
        {
            owner._animator.SetBool("Roll", false);
        }
    }
    public void RollEnd()
    {
        StartCoroutine(CoolDownCoroutineRolling());
    }
    private IEnumerator CoolDownCoroutineRolling()
    {
        yield return new WaitForSeconds(rollingCoolDown);

        isRedyAttack = true;
        _animator.SetFloat("Speed", 0);
        ChangeState(stateIdle);
    }

}
