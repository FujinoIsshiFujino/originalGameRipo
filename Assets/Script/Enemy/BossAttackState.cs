using System.Buffers;
using UnityEngine;
using System.Collections;

public class BossAttackState : BossStateBase
{
    public override void OnEnter(
        BossController owner,
        BossStateBase previousState)
    {
        // これはチェイスのイグジットにおいていい気がする
        owner._animator.SetBool("Walk", false);
    }

    public override void OnUpdate(BossController owner)
    {
        float distance = owner.DistanceToTarget;

        // 攻撃の際にプレイヤーの方により向くように調整
        Vector3 direction = owner.Target.position - owner.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            owner.transform.rotation = Quaternion.RotateTowards(
                owner.transform.rotation,
                targetRotation,
                owner.rotationSpeed * Time.deltaTime
            );
        }

        // attackFinishedが攻撃制御boolなので、安全装置としてつけておく
        if (owner.Target != null && owner.attackFinished == true)
        {
            owner.StopMovement(); // 攻撃モーション中に動かないように。
            if (distance <= owner.AttackRangeDetecter.radius)
            {
                //近接攻撃
                owner._animator.SetTrigger("Basic Attack");
                owner.attackFinished = false;
            }
            else if (distance > owner.AttackRangeDetecter.radius && distance <= owner.CollisionDetecterBossLost.radius)
            {
                // ノーマルアタックコリジョン以上、見失うコリジョン範囲以下の時
                //遠距離攻撃
                owner._animator.SetTrigger("Flame Attack");
                owner.attackFinished = false;
            }
        }
    }

    public override void OnExit(
        BossController owner,
        BossStateBase nextState)
    {
        owner.CoolDownCoroutine();
    }
}


