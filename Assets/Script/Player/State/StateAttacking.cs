using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControl
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private float attackCoolDown = 0.5f;
    bool isRedyAttack = true;
    public bool attackFinished = false; // ２段攻撃に移行するかのフラグ
    public class StateAttacking : PlayerStateBase
    {
        public override void OnEnter(PlayerControl owner, PlayerStateBase preState)
        {
            owner.isRedyAttack = false;
        }

        public override void OnUpdate(PlayerControl owner)
        {
            // 現在再生中のアニメーション情報を取得
            AnimatorStateInfo stateInfo = owner._animator.GetCurrentAnimatorStateInfo(0);

            // アニメーションが"Attack"の間だけ時間を計測
            if (stateInfo.IsName("Attack"))
            {
                // 攻撃ボタンが再度押されていれば、2段攻撃に遷移
                if (Input.GetButtonDown("Attack"))
                {
                    owner.attackFinished = true;
                    return;
                }
            }
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

    //モーションの途中（攻撃コリジョンが発生してほしいタイミング）で呼ばれる
    public void AttackStart()
    {
        attackCollider.enabled = true;
    }

    //モーションの途中（攻撃コリジョンが消えてほしいタイミング）で呼ばれる
    public void AttackFinished()
    {
        attackCollider.enabled = false;

        if (attackFinished)
        {
            _animator.SetTrigger("Attack2");
            ChangeState(stateAttacking);
            attackFinished = false;
            return;
        }

        attackFinished = false;
        StartCoroutine(CoolDownCoroutine());
    }

    public void SecondAttackFinished()
    {
        attackCollider.enabled = false;
        attackFinished = false;

        StartCoroutine(CoolDownCoroutine());
    }

    private IEnumerator CoolDownCoroutine()
    {
        yield return new WaitForSeconds(attackCoolDown);
        // GoToNormalStateIfPossible();

        _animator.SetTrigger("Exit");
        isRedyAttack = true;
        _animator.SetFloat("Speed", 0);
        ChangeState(stateIdle);

    }




    // public void GoToAttackStateIfPossible()
    // { _animator.SetTrigger("Attack"); }


}
