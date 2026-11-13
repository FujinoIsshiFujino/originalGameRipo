using Unity.VisualScripting;
using UnityEngine;

public class BossIdleState : BossStateBase
{
    public override void OnEnter(BossController owner, BossStateBase previousState)
    {
        owner._animator.SetBool("Walk", false);
        owner._animator.SetBool("Idle", true);
    }

    public override void OnUpdate(BossController owner)
    {
        if (owner.isScreaming)
            return;
        if (owner.Target != null)
        {
            owner.ChangeState(new BossChaseState());
        }
    }

    public override void OnExit(BossController owner, BossStateBase nextState)
    {
        owner._animator.SetBool("Idle", false);
    }
}