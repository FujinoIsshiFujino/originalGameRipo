using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControl
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private float attackCoolDown = 0.5f;
    bool isRedyAttack = true;
    public class StateAttacking : PlayerStateBase
    {
        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {
            owner.isRedyAttack = false;

        }
        public override void OnUpdate(PlayerControl owner)
        {



        }

        public override void OnExit(PlayerControl owner, PlayerStateBase nextState)
        {

        }
    }


    //攻撃コリジョンにあたったらインスペクター上から呼ばれる
    public void HitAttack(Collider collider)
    {
        var targetMob = collider.GetComponent<MobStatus>();
        if (null == targetMob) return;

        targetMob.Damage(1);
    }




    //モーションの始まりと終わりに呼ばれる
    //終わりに呼ばれる方はステイト遷移もかねてる
    public void AttackStart()
    {

        attackCollider.enabled = true;
    }

    public void AttackFinished()
    {
        attackCollider.enabled = false;
        StartCoroutine(CoolDownCoroutine());
    }
    private IEnumerator CoolDownCoroutine()
    {
        yield return new WaitForSeconds(attackCoolDown);
        // GoToNormalStateIfPossible();

        _animator.SetTrigger("Exit");
        isRedyAttack = true;
        ChangeState(stateIdle);

    }




    // public void GoToAttackStateIfPossible()
    // { _animator.SetTrigger("Attack"); }


}
