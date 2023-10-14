using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MobStatus))]
public class MobAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown = 0.5f;
    [SerializeField] private Collider attackCollider;
    private MobStatus _status;
    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<MobStatus>();
    }


    public void AttackIfPossible()
    {
        if (!_status.IsAttackable) return;
        _status.GoToAttackStateIfPossible();
    }

    public void OnAttackRangeEnter(Collider collider)
    {
        AttackIfPossible();
    }

    public void OnAttackStart()
    {
        attackCollider.enabled = true;
    }

    public void OnHitAttack(Collider collider)
    {
        var targetMob = collider.GetComponent<MobStatus>();
        if (null == targetMob) return;

        targetMob.Damage(1);
    }

    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
        StartCoroutine(CooldownCoroutine());
    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCoolDown);
        _status.GoToNormalStateIfPossible();
    }
}