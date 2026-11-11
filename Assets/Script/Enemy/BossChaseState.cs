using UnityEngine;

public class BossChaseState : BossStateBase
{
    public float stopDistance = 3;
    public override void OnEnter(
        BossController owner,
        BossStateBase previousState)
    {
        owner._animator.SetBool("Walk", true);
    }

    public override void OnUpdate(BossController owner)
    {
        float distance = owner.DistanceToTarget;

        if (distance <= stopDistance)
        {
            owner.StopMovement();
            owner.ReturnToIdle();
            return;
        }

        if (owner.Target != null && owner.attackFinished == true)
        {
            if (distance > owner.AttackRangeDetecter.radius && distance <= owner.CollisionDetecterBossLost.radius)
            {
                owner.ChangeState(new BossAttackState());
            }
        }

        if (owner.Target == null)
        {
            owner.StopMovement();

            // 追跡対象がいなければアイドルに戻る
            owner.ReturnToIdle();
            return;
        }
        owner.MoveToTarget();
    }

    public override void OnExit(
        BossController owner,
        BossStateBase nextState)
    {
        owner._animator.SetBool("Walk", false);
    }
}